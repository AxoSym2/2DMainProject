using UnityEngine;

public class EnemyUnit_Move : MonoBehaviour
{
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private Transform _attackRangeCheck;
    [SerializeField] private LayerMask _playerLayer;

    private EnemyUnit_Base _enemyBase;
    private Rigidbody2D _rb;
    private Transform _target;
    private float _moveSpeed;
    private float _attackCoolDown;
    private float _lastAttackTime = 0f;
    private EnemyUnit_AnimationController _animController;
    private EnemyUnit_Projectile _projectileAttack;
    private EnemyUnit_PointAttack _pointAttack;
    private EnemyUnit_Instance _InstanceAttack;
    private LayerMask _wallLayer;

    public Transform GetAttackRangeCheck() {  return _attackRangeCheck; }
    public float GetAttackRange() { return _attackRange; }

    public void Init(float moveSpeed, float attackCoolDown)
    {
        enabled = true;
        _rb = GetComponent<Rigidbody2D>();
        _enemyBase = GetComponent<EnemyUnit_Base>();
        _animController = GetComponent<EnemyUnit_AnimationController>();
        _projectileAttack = GetComponent<EnemyUnit_Projectile>();
        _pointAttack = GetComponent<EnemyUnit_PointAttack>();
        _InstanceAttack = GetComponent<EnemyUnit_Instance>();
        _moveSpeed = moveSpeed;
        _attackCoolDown = attackCoolDown;
        _wallLayer = LayerMask.GetMask("Wall");
    }

    public void Flip(Vector2 direction)
    {
        if (direction.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (direction.x < 0) 
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void FixedUpdate()
    {
        if (_target == null) return;
        if (_InstanceAttack != null && _InstanceAttack.IsDashing) return;
        Collider2D player = Physics2D.OverlapCircle(_attackRangeCheck.position, _attackRange, _playerLayer);

        if (player != null)
        {
            _rb.linearVelocity = Vector2.zero;
            if (Time.time - _lastAttackTime >= _attackCoolDown) 
            {
                _lastAttackTime = Time.time;
                EnemyUnitData data = DaniTechGameDataManager.Instance.GetEnemyUnitData(_enemyBase.GetEnemyDataId());

                if(data.EnemyType == "Projectile")
                {
                    _animController.SetState(EnemyUnitState.Attack);
                    if(_projectileAttack != null)
                    {
                        _projectileAttack.FireProjectile();
                    }    
                }
                else if (data.EnemyType == "PointAttack")
                {
                    _animController.SetState(EnemyUnitState.Attack);
                    if (_pointAttack != null)
                    {
                        _pointAttack.FirePointAttack();
                    }
                }
                else if (data.EnemyType == "Instance")
                {
                    _animController.SetState(EnemyUnitState.Attack);
                    if (_InstanceAttack != null)
                    {
                        _InstanceAttack.DoAttack();
                    }
                }
            }
        }
        else
        {
            Vector2 direction = GetMoveDirection();
            _rb.linearVelocity = direction * _moveSpeed;
            _animController.SetState(EnemyUnitState.Run);
            _animController.SetDirection(direction);
            Flip(direction);
        }
    }

    private Vector2 GetMoveDirection()
    {
        Vector2 direction = (_target.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.5f, _wallLayer);
        if (hit.collider == null) return direction;

        Vector2 leftDir = Vector2.Perpendicular(direction).normalized;
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, leftDir, 1.5f, _wallLayer);
        if (leftHit.collider == null) return leftDir;

        Vector2 rightDir = -Vector2.Perpendicular(direction).normalized;
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, rightDir, 1.5f, _wallLayer);
        if (rightHit.collider == null) return rightDir;

        return direction;
    }

    private void OnDrawGizmos()
    {
        if (_attackRangeCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackRangeCheck.position, _attackRange);
    }
}
