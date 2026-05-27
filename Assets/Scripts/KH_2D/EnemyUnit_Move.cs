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
    private EnemyUnit_PointAttack _pointattack;

    public void Init(float moveSpeed, float attackCoolDown)
    {
        enabled = true;
        _rb = GetComponent<Rigidbody2D>();
        _enemyBase = GetComponent<EnemyUnit_Base>();
        _animController = GetComponent<EnemyUnit_AnimationController>();
        _projectileAttack = GetComponent<EnemyUnit_Projectile>();
        _pointattack = GetComponent<EnemyUnit_PointAttack>();
        _moveSpeed = moveSpeed;
        _attackCoolDown = attackCoolDown;
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
                    if (_pointattack != null)
                    {
                        _pointattack.FirePointAttack();
                    }
                }
                else
                {
                    _animController.SetState(EnemyUnitState.Attack);
                    player.GetComponent<PlayerUnit_Base>()?.TakeDamage(_enemyBase.GetAttackDamage());
                }
            }
        }
        else
        {
            Vector2 direction = (_target.position - transform.position).normalized;
            _rb.linearVelocity = direction * _moveSpeed;
            _animController.SetState(EnemyUnitState.Run);
            _animController.SetDirection(direction);
            Flip(direction);
        }
    }

    private void OnDrawGizmos()
    {
        if (_attackRangeCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackRangeCheck.position, _attackRange);
    }
}
