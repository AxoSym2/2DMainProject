using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit_Base : MonoBehaviour
{
    [SerializeField] private Slider Slider_Health;

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
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= damage;
        UpdateHealthBar();
        if (_currentHp <= 0) 
        {
            OnDie();
        }
    }

    private void UpdateHealthBar()
    {
        if (Slider_Health == null) return;
        Slider_Health.value = _currentHp / _enemyData.Hp;
    }

    public float GetAttackDamage()
    {
        return _enemyData.AttackDamage;
    }

    private void DropHealKit()
    {
        GameObject obj = ObjectPoolManager.Instance.GetObject("Prefabs/Item/HealKit");
        if (obj != null)
        {
            obj.transform.position = transform.position;
        }
    }

    private void DropUmbra()
    {
        GameObject obj = ObjectPoolManager.Instance.GetObject("Prefabs/Item/Umbra");
        if (obj != null)
        {
            obj.transform.position = transform.position;
            obj.GetComponent<Umbra>().Init(_enemyData.DropUmbraAmount);
        }
    }


    private void OnDie()
    {
        DaniTechGameManager.Inst.IncreasePlayerExp((int)_enemyData.ExpReward);

        if (UnityEngine.Random.value <= _enemyData.DropUmbraChance)
            DropUmbra();

        if (UnityEngine.Random.value <= _enemyData.DropHealKitChance)
            DropHealKit();

        ObjectPoolManager.Instance.ReturnObject(_enemyData.PrefabPath, gameObject);
    }

    public string GetEnemyDataId()
    {
        return _enemyDataId;
    }
}
