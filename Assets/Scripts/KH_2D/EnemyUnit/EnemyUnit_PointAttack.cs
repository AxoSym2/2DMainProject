using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

public class EnemyUnit_PointAttack : MonoBehaviour
{
    private EnemyUnitData _enemyData;
    private Transform _target;

    private string _overrideProjectilePath;

    public void Init(EnemyUnitData enemyData, Transform target)
    {
        _enemyData = enemyData;
        _target = target;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetPointAttackPath(string path)
    {
        _overrideProjectilePath = path;
    }

    public void FirePointAttack()
    {
        //Debug.Log("FirePointAttack 호출됨");
        if (_target == null) return;
        Vector2 targetPos = _target.position;

        if (_enemyData.AttackType == "Throw")
        {
            ThrowAttack(targetPos);
        }
        else if (_enemyData.AttackType == "Instance")
        {
            InstanceAttack(targetPos).Forget();
        }
    }

    private void ThrowAttack(Vector2 targetPos)
    {
        string path = string.IsNullOrEmpty(_overrideProjectilePath) ? _enemyData.ProjectilePath : _overrideProjectilePath;
        if (string.IsNullOrEmpty(path)) return; 
        GameObject pointAttack = ObjectPoolManager.Instance.GetObject(path);
        if (pointAttack == null) return;

        pointAttack.transform.position = transform.position;
        pointAttack.GetComponent<Enemy_PointAttack>().Init(_enemyData.AttackDamage, _enemyData.AttackRange, _enemyData.ProjectileSpeed, targetPos, path);
    }

    private async UniTaskVoid InstanceAttack(Vector2 targetPos)
    {
        string path = string.IsNullOrEmpty(_overrideProjectilePath) ? _enemyData.ProjectilePath: _overrideProjectilePath;
        if (string.IsNullOrEmpty(path) == false)
        {
            GameObject effect = ObjectPoolManager.Instance.GetObject(path);
            if (effect != null)
            {
                effect.transform.position = targetPos;
                effect.GetComponent<Enemy_PointAttack>().Init(path, _enemyData.PointAttackDelay + 0.5f);
            }
        }

        await UniTask.Delay(TimeSpan.FromSeconds(_enemyData.PointAttackDelay));

        Collider2D[] players = Physics2D.OverlapCircleAll(targetPos, _enemyData.AttackRange, LayerMask.GetMask("Player"));
        foreach (var player in players)
        {
            PlayerUnit_Base playerBase = player.GetComponent<PlayerUnit_Base>();
            if (playerBase != null)
            {
                playerBase.TakeDamage(_enemyData.AttackDamage);
            }
        }
    }
}
