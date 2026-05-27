using UnityEngine;

public class SkillBase : MonoBehaviour
{
    protected SkillData _skillData;
    protected LayerMask _enemyLayer;

    protected void DealDamage(EnemyUnit_Base enemy)
    {
        float finalDamage = _skillData.Damage * DaniTechGameManager.Inst.GetAttackMultiplier();
        //Debug.Log($"스킬: {_skillData.Name}, 데미지: {finalDamage}");
        enemy.TakeDamage(finalDamage);
    }
}
