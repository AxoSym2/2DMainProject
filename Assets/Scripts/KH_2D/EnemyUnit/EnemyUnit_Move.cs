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
    private float _colliderRadius;

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
        var col = GetComponent<Collider2D>();
        _colliderRadius = 0.5f;
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

                if (data.EnemyType == "Boss") return;

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
        Vector2 toTarget = (_target.position - transform.position).normalized;
        Vector2 wallAvoidance = Vector2.zero;

        float checkDistance = 0.8f;
        bool isWallNear = false;

        int rayCount = 16;
        for (int i = 0; i < rayCount; i++)
        {
            float angle = i*(360f/rayCount) * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            RaycastHit2D hit = Physics2D.CircleCast(transform.position, _colliderRadius, dir, checkDistance, _wallLayer);
            if (hit.collider != null)
            {
                float strength = Mathf.Pow(1f-(hit.distance/checkDistance), 2);
                wallAvoidance += hit.normal * strength;
                isWallNear = true;
            }
        }    
        if (isWallNear)
        {
            float dot = Vector2.Dot(wallAvoidance, toTarget);
            if (dot < 0)
            {
                wallAvoidance -= toTarget * dot;
            }
            return (toTarget + wallAvoidance * 2f).normalized;
        }
        return toTarget;
    }

    private void OnDrawGizmos()
    {
        if (_attackRangeCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackRangeCheck.position, _attackRange);
    }
}
