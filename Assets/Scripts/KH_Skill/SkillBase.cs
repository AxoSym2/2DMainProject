using UnityEngine;

public class SkillBase : MonoBehaviour
{
    protected SkillData _skillData;
    protected LayerMask _enemyLayer;

    protected void DealDamage(EnemyUnit_Base enemy)
    {
        float finalDamage = _skillData.Damage + DaniTechGameManager.Inst.GetAttackBonus();
        //Debug.Log($"스킬: {_skillData.Name}, 데미지: {finalDamage}");
        enemy.TakeDamage(finalDamage);

        float healAmount = finalDamage * 0.005f;
        var player = FindAnyObjectByType<PlayerUnit_Base>();
        if (player != null)
        {
            player.Heal(healAmount);
        }
    }

    protected Collider2D[] GetEnemiesInRange()
    {
        if (_skillData.RangeType == "Box")
        {
            return Physics2D.OverlapBoxAll(transform.position, new Vector2(_skillData.RangeX, _skillData.RangeY), transform.eulerAngles.z, _enemyLayer); 
        }
        else
        {
            return Physics2D.OverlapCircleAll(transform.position, _skillData.Range, _enemyLayer);
        }
    }
}
