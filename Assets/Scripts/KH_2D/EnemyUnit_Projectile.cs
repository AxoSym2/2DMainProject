using UnityEngine;

public class EnemyUnit_Projectile : MonoBehaviour
{
    private EnemyUnitData _enemyData;
    private Transform _target;

    public void Init(EnemyUnitData enemyData, Transform transform)
    {
        _enemyData = enemyData;
        _target = transform;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void FireProjectile()
    {
        if (_target == null) return;
        if (string.IsNullOrEmpty(_enemyData.ProjectilePath)) return;

        GameObject projectileObj = ObjectPoolManager.Instance.GetObject(_enemyData.ProjectilePath);
        if (projectileObj == null) return;

        projectileObj.transform.position = transform.position;
        Vector2 dir = (_target.position - transform.position).normalized;
        projectileObj.GetComponent<Enemy_Projectile>().Init(_enemyData.AttackDamage, _enemyData.ProjectileSpeed, dir, _enemyData.ProjectilePath);
    }
}
