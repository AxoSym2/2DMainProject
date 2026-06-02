using UnityEngine;

public class PlayerUnit_Base : MonoBehaviour
{
    private PlayerUnitData _playerData;
    private float _currentHp;

    public void Init(string playerUnitId)
    {
        _playerData = DaniTechGameDataManager.Instance.GetPlayerUnitData(playerUnitId);

        if (_playerData == null)
        {
            Debug.LogError($"플레이어 데이터 없음: {playerUnitId}");
            return;
        }

        _currentHp = GetMaxHp();
        UpdateHealthBar();
    }

    private float GetMaxHp()
    {
        return _playerData.Hp + DaniTechGameManager.Inst.GetHpBonus();
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage * (1f - DaniTechGameManager.Inst.GetDefenseBonus());
        _currentHp -= finalDamage;
        UpdateHealthBar();
        //Debug.Log($"플레이어 체력: {_currentHp}");
        if ( _currentHp <= 0 )
        {
            OnDie();
        }
    }

    private void UpdateHealthBar()
    {
        var inGameUI = DaniTechUIManager.Instance.GetCreatedUI(DaniTechUIRootType.MainUI, DaniTechUIType.InGameUI);
        if (inGameUI is InGameUI ui)
        {
            ui.SetHealthBar(_currentHp, GetMaxHp());
        }
    }

    public void Heal(float amount)
    {
        _currentHp = Mathf.Min(_currentHp + amount, GetMaxHp());
        UpdateHealthBar();
    }

    private void OnDie() 
    {
        Debug.Log("플레이어 사망");
        _currentHp = 0;
        UpdateHealthBar();

        DaniTechGameManager.Inst.OnPlayerDie();
    }
}


