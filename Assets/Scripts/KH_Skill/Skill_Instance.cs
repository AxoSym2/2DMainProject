using UnityEngine;

public class Skill_Instance : MonoBehaviour
{
    private SkillData _skillData;
    private LayerMask _enemyLayer;

    public void Init(SkillData skillData, LayerMask enemyLayer)
    {
        _skillData = skillData;
        _enemyLayer = enemyLayer;

        ApplyDamage();
    }

    private void ApplyDamage()
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
    }

    public void OnAnimationEnd()
    {
        ObjectPoolManager.Instance.ReturnObject(_skillData.PrefabPath, gameObject);
    }
}
