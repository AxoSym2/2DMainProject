using UnityEngine;

public class PlayerUnit_Attack : MonoBehaviour
{
    [SerializeField] private LayerMask _enemyLayer;
    private SkillData _skillData;
    private float _lastAttackTime = 0f;
    private Transform _target;

    public void Init(string skillId)
    {
        _skillData = DaniTechGameDataManager.Instance.GetSkillsData(skillId);
        if (_skillData == null )
        {
            Debug.LogError($"스킬 데이터 없음: {skillId}");
            return;
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;
        if (_skillData == null) return;
        FindNearestEnemy();

        if (_target != null && Time.time - _lastAttackTime >= _skillData.CoolDown)
        {
            FireSkill();
            _lastAttackTime = Time.time;
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

    private void FireSkill()
    {
        GameObject skillObj = ObjectPoolManager.Instance.GetObject(_skillData.PrefabPath);
        if (skillObj == null) return;

        Vector2 dir = GetComponent<PlayerUnit_Move>().GetLastMoveDir();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        skillObj.transform.position = transform.position + new Vector3(dir.x, dir.y, 0) * 1f;

        switch(_skillData.SkillType)
        {
            case "Instance":
                skillObj.transform.rotation = Quaternion.Euler(0, 0, angle);
                skillObj.GetComponent<Skill_Instance>().Init(_skillData, _enemyLayer, transform, dir);
                break;
            case "Projectile":
                skillObj.transform.rotation = Quaternion.Euler(0, 0, angle);
                skillObj.GetComponent<Skill_Projectile>().Init(_skillData, _enemyLayer, dir);
                break;
            case "Area":
                skillObj.transform.rotation = Quaternion.identity;
                skillObj.GetComponent<Skill_Area>().Init(_skillData, _enemyLayer, transform);
                break;
        }

    }
}
