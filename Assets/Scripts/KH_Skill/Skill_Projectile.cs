using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

public class Skill_Projectile : SkillBase
{
    [SerializeField] private SpriteRenderer SpriteRenderer_Effect;
    [SerializeField] private float _projectileSpeed = 5f;
    [SerializeField] private float _lifeTime = 10f;
    private Vector2 _moveDirection;

    public void Init(SkillData skillData, LayerMask enemyLayer, Vector2 direction)
    {
        _skillData = skillData;
        _enemyLayer = enemyLayer;
        _moveDirection = direction.normalized;
        AutoReturn().Forget();
    }

    private async UniTaskVoid AutoReturn()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_lifeTime));
        if (gameObject.activeSelf)
        {
            ObjectPoolManager.Instance.ReturnObject(_skillData.PrefabPath, gameObject);
        }
    }

    private void Update()
    {
        transform.position += (Vector3)_moveDirection * _projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyUnit_Base enemy = collision.GetComponent<EnemyUnit_Base>();
        if (enemy != null)
        {
            DealDamage(enemy);
            ObjectPoolManager.Instance.ReturnObject(_skillData.PrefabPath, gameObject);
        }
    }
}
