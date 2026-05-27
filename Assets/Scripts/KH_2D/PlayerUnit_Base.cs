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
        int level = DaniTechGameManager.Inst.GetUpGradeLevel("Hp");
        if (level == 0) 
        {
            return _playerData.Hp;
        }

        UpgradeData data = DaniTechGameDataManager.Instance.GetUpgradeData($"upgrade_hp_{level}");
        if (data == null)
        {
            return _playerData.Hp;
        }
        return _playerData.Hp + data.IncreaseAmount;
    }

    public void TakeDamage(float damage)
    {
        float defense = DaniTechGameManager.Inst.GetDefenseMultiplier();
        float finalDamage = damage * (1f - defense);
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
        _currentHp += amount;
        if(_currentHp > _playerData.Hp)
        {
            _currentHp = _playerData.Hp;
        }
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


