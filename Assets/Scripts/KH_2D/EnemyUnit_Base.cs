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
        GetComponent<Collider2D>().enabled = true;
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
        float defense = DaniTechGameManager.Inst.GetDefenseMultiplier();
        float finalDamage = damage * (1f - defense);

        //Debug.Log($"받은 데미지: {damage}, 방어력: {defense}, 최종 데미지: {finalDamage}");

        _currentHp -= finalDamage;
        UpdateHealthBar();
        if (_currentHp <= 0) 
        {
            OnDie();
        }
    }

    public string GetEnemyDataId()
    {
        return _enemyDataId;
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

        GetComponent<Collider2D>().enabled = false;
        GetComponent<EnemyUnit_Move>().enabled = false;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        GetComponent<EnemyUnit_AnimationController>().SetState(EnemyUnitState.Dead);

        ReturnToPool().Forget();
    }

    
    private async Cysharp.Threading.Tasks.UniTaskVoid ReturnToPool()
    {
        await Cysharp.Threading.Tasks.UniTask.Delay(System.TimeSpan.FromSeconds(1f));
        ObjectPoolManager.Instance.ReturnObject(_enemyDataId, gameObject);
    }
}
