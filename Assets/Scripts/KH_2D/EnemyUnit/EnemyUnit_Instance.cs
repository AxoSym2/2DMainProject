using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class EnemyUnit_Instance : MonoBehaviour
{
    private EnemyUnitData _enemyData;
    private Transform _target;
    private Rigidbody2D _rb;
    private EnemyUnit_Move _enemyMove;
    private bool _isDashing = false;
    private bool _isBombing = false;

    public bool IsDashing { get { return _isDashing; } }

    public void Init(EnemyUnitData enemyData, Transform target)
    {
        _enemyData = enemyData;
        _target = target;
        _rb = GetComponent<Rigidbody2D>();
        _enemyMove = GetComponent<EnemyUnit_Move>();
        _isDashing = false;
        _isBombing = false;
    }

    public void DoAttack()
    {
        switch (_enemyData.AttackType)
        {
            case "Dash":
                if (_isDashing == false)
                    Dash().Forget();
                break;
            case "Bombing":
                if (_isBombing == false)
                    Bombing().Forget();
                break;
            default:
                MeleeAttack().Forget();
                break;
        }
    }

    private async UniTaskVoid MeleeAttack()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
        Collider2D check = Physics2D.OverlapCircle(_enemyMove.GetAttackRangeCheck().position, _enemyMove.GetAttackRange(), LayerMask.GetMask("Player"));
        if (check == null) return;
        check.GetComponent<PlayerUnit_Base>()?.TakeDamage(_enemyData.AttackDamage);
    }

    private async UniTaskVoid Dash()
    {
        _isDashing = true;
        if (_target == null) return;

        Vector2 dir = (_target.position - transform.position).normalized;
        float dashDuration = 0.5f;
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            _rb.linearVelocity = dir * _enemyData.ProjectileSpeed;
            elapsed += Time.deltaTime;
            await UniTask.Yield();
        }

        _rb.linearVelocity = Vector2.zero;
        _isDashing = false;

        //Debug.Log($"대쉬 후 범위체크, AttackRange: {_enemyData.AttackRange}");
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _enemyData.AttackRange, LayerMask.GetMask("Player"));
        //Debug.Log($"hit: {hit}");
        if ( hit != null)
        {
            PlayerUnit_Base player = hit.GetComponent<PlayerUnit_Base>();
            if (player != null)
            {
                player.TakeDamage(_enemyData.AttackDamage);
            }
        }
    }
    
    public void DashAttack()
    {
        if (_isDashing == false)
            Dash().Forget();
    }

    private async UniTaskVoid Bombing()
    {
        _isBombing = true;
        await UniTask.Delay(TimeSpan.FromSeconds(_enemyData.PointAttackDelay));

        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, _enemyData.AttackRange, LayerMask.GetMask("Player"));
        foreach (var p in players)
        {
            PlayerUnit_Base playerBase = p.GetComponent<PlayerUnit_Base>();
            if (playerBase != null)
            {
                playerBase.TakeDamage(_enemyData.AttackDamage);
            }
        }
        GetComponent<EnemyUnit_Base>().TakeDamage(99999f);
    }
}
