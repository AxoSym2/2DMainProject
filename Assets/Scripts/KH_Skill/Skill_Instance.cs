using UnityEngine;

public class Skill_Instance : SkillBase
{
    private Transform _playerTransform;
    private Vector2 _direction;

    public void Init(SkillData skillData, LayerMask enemyLayer, Transform playerTransform, Vector2 direction)
    {
        _skillData = skillData;
        _enemyLayer = enemyLayer;
        _playerTransform = playerTransform;
        _direction = direction;

        ApplyDamage();
    }

    public void Update()
    {
        if (_playerTransform == null) return;
        transform.position = _playerTransform.position + new Vector3(_direction.x, _direction.y, 0) * 1f;
    }

    private void ApplyDamage()
    {
        Collider2D[] enemies = GetEnemiesInRange();
        foreach (var enemy in enemies)
        {
            EnemyUnit_Base enemyBase = enemy.GetComponent<EnemyUnit_Base>();
            if (enemyBase != null)
            {
                DealDamage(enemyBase);
            }
        }
    }

    public void OnAnimationEnd()
    {
        ObjectPoolManager.Instance.ReturnObject(_skillData.PrefabPath, gameObject);
    }
}
