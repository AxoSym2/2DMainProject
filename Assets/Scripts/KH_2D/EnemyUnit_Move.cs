using UnityEngine;

public class EnemyUnit_Move : MonoBehaviour
{
    private EnemyUnit_Base _enemyBase;
    private Rigidbody2D _rb;
    private Transform _target;
    private float _moveSpeed;
    private EnemyUnit_AnimationController _animController;

    public void Init(float moveSpeed)
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyBase = GetComponent<EnemyUnit_Base>();
        _animController = GetComponent<EnemyUnit_AnimationController>();
        _moveSpeed = moveSpeed;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void FixedUpdate()
    {
        if (_target == null) return;
        Vector2 direction = (_target.position - transform.position).normalized;
        _rb.linearVelocity = direction * _moveSpeed;
        _animController.SetState(EnemyUnitState.Run);
        _animController.SetDirection(direction);
    }
}
