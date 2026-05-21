using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

public class Skill_Area : SkillBase
{
    [SerializeField] private bool _isFollow = false;
    private Transform _playerTransform;

    public void Init(SkillData skillData, LayerMask enemyLayer, Transform playerTransform)
    {
        _skillData = skillData;
        _enemyLayer = enemyLayer;
        _playerTransform = playerTransform;
        StartArea().Forget();
    }

    public void Update()
    {
        if( _isFollow && _playerTransform != null)
            transform.position = _playerTransform.position;
    }

    private async UniTaskVoid StartArea()
    {
        float elapsed = 0f;
        float tickInterval = 1f;

        while (elapsed < _skillData.Duration)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _skillData.Range, _enemyLayer);
            foreach (var enemy in enemies)
            {
                EnemyUnit_Base enemyBase = enemy.GetComponent<EnemyUnit_Base>();
                if (enemyBase != null)
                {
                    enemyBase.TakeDamage(_skillData.Damage);
                }
            }

            await UniTask.Delay(TimeSpan.FromSeconds(tickInterval));
            elapsed += tickInterval;
        }
        ObjectPoolManager.Instance.ReturnObject(_skillData.PrefabPath, gameObject);
    }
}
