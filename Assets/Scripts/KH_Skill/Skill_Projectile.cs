using UnityEngine;

public class Skill_Projectile : SkillBase
{
    [SerializeField] private SpriteRenderer SpriteRenderer_Effect;
    [SerializeField] private float _projectileSpeed = 5f;
    private Vector2 _moveDirection;

    public void Init(SkillData skillData, LayerMask enemyLayer, Vector2 direction)
    {
        _skillData = skillData;
        _enemyLayer = enemyLayer;
        _moveDirection = direction.normalized;
        //SpriteRenderer_Effect.flipX = direction.x > 0;
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
            enemy.TakeDamage(_skillData.Damage);
            ObjectPoolManager.Instance.ReturnObject(_skillData.PrefabPath, gameObject);
        }
    }
}
