using UnityEngine;

public class EnemyUnit_Base : MonoBehaviour
{
    private EnemyUnitData _enemyData;
    private string _enemyDataId;
    private float _currentHp;

    public void Init(string enemyDataId)
    {
        _enemyDataId = enemyDataId;
        _enemyData = DaniTechGameDataManager.Instance.GetEnemyUnitData(enemyDataId);
        if (_enemyData == null)
        {
            Debug.LogError($"적 데이터 없음: {enemyDataId}");
            return;
        }

        _currentHp = _enemyData.Hp;
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= damage;
        if (_currentHp <= 0) 
        {
            OnDie();
        }
    }

    private void OnDie()
    {
        DaniTechGameManager.Inst.IncreasePlayerExp((int)_enemyData.ExpReward);
        ObjectPoolManager.Instance.ReturnObject(_enemyData.PrefabPath, gameObject);
    }

    public string GetEnemyDataId()
    {
        return _enemyDataId;
    }
}
