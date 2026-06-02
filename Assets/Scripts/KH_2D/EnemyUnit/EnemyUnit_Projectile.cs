using System.Threading;
using UnityEngine;

public class EnemyUnit_Projectile : MonoBehaviour
{
    private EnemyUnitData _enemyData;
    private Transform _target;

    public string _overrideProjectilePath;

    public void Init(EnemyUnitData enemyData, Transform transform)
    {
        _enemyData = enemyData;
        _target = transform;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetProjectilePath(string path)
    {
        _overrideProjectilePath = path;
    }

    public void FireProjectile()
    {
        if (_target == null) return;
        string path = string.IsNullOrEmpty(_overrideProjectilePath) ? _enemyData.ProjectilePath : _overrideProjectilePath;
        if (string.IsNullOrEmpty(path)) return;
        GameObject projectileObj = ObjectPoolManager.Instance.GetObject(path);

        projectileObj.transform.position = transform.position;
        Vector2 dir = (_target.position - transform.position).normalized;
        projectileObj.GetComponent<Enemy_Projectile>().Init(_enemyData.AttackDamage, _enemyData.ProjectileSpeed, dir, path);
    }

    public void FireSpreadProjectile(int count, float spreadAngle)
    {
        if (_target == null) return;
        string path = string.IsNullOrEmpty(_overrideProjectilePath) ? _enemyData.ProjectilePath : _overrideProjectilePath;
        if(string.IsNullOrEmpty(path)) return;

        Vector2 baseDir = (_target.position - transform.position).normalized;
        float angleStep = spreadAngle / (count - 1);
        float startAngle = -spreadAngle / 2f;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + angleStep * i;
            float rad = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            Vector2 dir = new Vector2(cos * baseDir.x - sin * baseDir.y, sin * baseDir.x + cos * baseDir.y);

            GameObject projectileObj = ObjectPoolManager.Instance.GetObject(path);
            if (projectileObj == null) continue;
            projectileObj.transform.position = transform.position;
            projectileObj.GetComponent<Enemy_Projectile>().Init(_enemyData.AttackDamage, _enemyData.ProjectileSpeed, dir, path);
        }
    }
}
