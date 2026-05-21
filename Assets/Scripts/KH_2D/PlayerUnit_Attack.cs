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

        Vector2 direction = (_target.position - transform.position).normalized;
        float dirX = direction.x > 0 ? 1f : -1f;
        skillObj.transform.position = transform.position;
        skillObj.GetComponent<Skill_Instance>().Init(_skillData, _enemyLayer, dirX);
    }
}
