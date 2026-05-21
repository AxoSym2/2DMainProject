using UnityEngine;

public class PlayerUnit_Base : MonoBehaviour
{
    private PlayerUnitData _playerData;
    private float _currentHp;

    public void Init(string playerUnitId)
    {
        _playerData = DaniTechGameDataManager.Instance.GetPlayerUnitData(playerUnitId);
        _currentHp = _playerData.Hp;
        UpdateHealthBar();

        if (_playerData == null)
        {
            Debug.LogError($"플레이어 데이터 없음: {playerUnitId}");
            return;
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= damage;
        UpdateHealthBar();
        Debug.Log($"플레이어 체력: {_currentHp}");
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
            ui.SetHealthBar(_currentHp, _playerData.Hp);
        }
    }

    private void OnDie() 
    {
        Debug.Log("플레이어 사망");
        _currentHp = 0;
        UpdateHealthBar();

        DaniTechGameManager.Inst.OnPlayerDie();
    }

    public float GetAttackDamage()
    {
        return _playerData.AttackDamage;
    }
}


