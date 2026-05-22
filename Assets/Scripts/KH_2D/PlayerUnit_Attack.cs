using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit_Attack : MonoBehaviour
{
    [SerializeField] private LayerMask _enemyLayer;
    private SkillData _skillData;
    private Transform _target;

    private List<SkillData> _skillDataList = new List<SkillData>();
    private List<float> _lastAttackTimeList = new List<float>();

    public void Init(string skillId)
    {
        _skillDataList.Clear();
        _lastAttackTimeList.Clear();
        AddSkill(skillId);
    }

    public void AddSkill(string skillId)
    {
        SkillData data = DaniTechGameDataManager.Instance.GetSkillsData(skillId);
        if (data == null)
        {
            Debug.LogError($"스킬데이터 없음: {skillId}");
            return;
        }
        _skillDataList.Add(data);
        _lastAttackTimeList.Add(0f);
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;
        if (_skillDataList.Count == 0) return;
        FindNearestEnemy();

        for (int i = 0; i < _skillDataList.Count; i++)
        {
            if (_target != null && Time.time - _lastAttackTimeList[i] >= _skillDataList[i].CoolDown)
            {
                FireSkill(_skillDataList[i]);
                _lastAttackTimeList[i] = Time.time;
            }
        }
    }

    private void FindNearestEnemy()
    {
        EnemyUnit_Base[] enemies = FindObjectsByType<EnemyUnit_Base>(FindObjectsSortMode.None);
        float minDist = float.MaxValue;
        _target = null;

        foreach(var enemy in enemies)
        {
            if (enemy.gameObject.activeSelf == false) continue;
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist <  minDist)
            {
                minDist = dist;
                _target = enemy.transform;
            }
        }
    }

    private void FireSkill(SkillData skillData)
    {
        GameObject skillObj = ObjectPoolManager.Instance.GetObject(skillData.PrefabPath);
        if (skillObj == null) return;

        Vector2 dir = GetComponent<PlayerUnit_Move>().GetLastMoveDir();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        skillObj.transform.position = transform.position + new Vector3(dir.x, dir.y, 0) * 1f;

        switch(skillData.SkillType)
        {
            case "Instance":
                skillObj.transform.rotation = Quaternion.Euler(0, 0, angle);
                skillObj.GetComponent<Skill_Instance>().Init(skillData, _enemyLayer, transform, dir);
                break;
            case "Projectile":
                skillObj.transform.rotation = Quaternion.Euler(0, 0, angle);
                skillObj.GetComponent<Skill_Projectile>().Init(skillData, _enemyLayer, dir);
                break;
            case "Area":
                skillObj.transform.rotation = Quaternion.identity;
                skillObj.GetComponent<Skill_Area>().Init(skillData, _enemyLayer, transform);
                break;
            default:
                break;
        }

    }
}
