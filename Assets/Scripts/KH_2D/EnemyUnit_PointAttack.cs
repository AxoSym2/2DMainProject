using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

public class EnemyUnit_PointAttack : MonoBehaviour
{
    private EnemyUnitData _enemyData;
    private Transform _target;

    public void Init(EnemyUnitData enemyData, Transform target)
    {
        _enemyData = enemyData;
        _target = target;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void FirePointAttack()
    {
        //Debug.Log("FirePointAttack 호출됨");
        if (_target == null)
        {
            //Debug.Log("타겟 없음");
            return;
        }
        Vector2 targetPos = _target.position;

        if (string.IsNullOrEmpty(_enemyData.ProjectilePath)) return;
        GameObject pointAttack = ObjectPoolManager.Instance.GetObject(_enemyData.ProjectilePath);
        if (pointAttack == null) return;

        pointAttack.transform.position = transform.position;
        pointAttack.GetComponent<Enemy_PointAttack>().Init(_enemyData.AttackDamage, _enemyData.AttackRange, _enemyData.ProjectileSpeed, targetPos, _enemyData.ProjectilePath);
    }
}
