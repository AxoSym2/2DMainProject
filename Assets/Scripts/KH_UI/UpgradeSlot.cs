using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    [SerializeField] private Text Text_StatName;
    [SerializeField] private Text Text_Level;
    [SerializeField] private Text Text_Cost;
    [SerializeField] private DaniTechUIButton Upgrade_Btn;
    [SerializeField] private Image[] UmbraGages;

    private string _statType;
    private int _maxLevel = 10;

    public void Init(string statType)
    {
        _statType = statType;
        Upgrade_Btn.BindOnClickButtonEvent(OnClick_Upgrade);
        Refresh();
    }

    private void Refresh()
    {
        int currentLevel = DaniTechGameManager.Inst.GetUpGradeLevel(_statType);
        int nextLevel = currentLevel + 1;

        Text_StatName.text = _statType;
        Text_Level.text = $"Lv.{currentLevel}/{_maxLevel}";

        for (int i = 0; i < UmbraGages.Length; i++)
        {
            UmbraGages[i].gameObject.SetActive(i < currentLevel);
        }

        if (currentLevel >= _maxLevel)
        {
            Text_Cost.text = "Max";
            Upgrade_Btn.gameObject.SetActive(false);
            return;
        }

        UpgradeData data = DaniTechGameDataManager.Instance.GetUpgradeData($"upgrade_{_statType.ToLower()}_{nextLevel}");
        if (data != null)
        {
            Text_Cost.text = $"{data.Cost}";
        }
    }

    private void OnClick_Upgrade()
    {
        int currentLevel = DaniTechGameManager.Inst.GetUpGradeLevel( _statType);
        int nextLevel = currentLevel + 1;

        UpgradeData data = DaniTechGameDataManager.Instance.GetUpgradeData($"upgrade_{_statType.ToLower()}_{nextLevel}");
        if (data == null) return;

        if (DaniTechGameManager.Inst.GetUmbra() < data.Cost)
        {
            Debug.Log("Umbra 부족");
            return;
        }

        DaniTechGameManager.Inst.SpendUmbra(data.Cost);
        DaniTechGameManager.Inst.SetUpGradeLevel(_statType, nextLevel);
        Refresh();

        var mainUI = DaniTechUIManager.Instance.GetCreatedUI(DaniTechUIRootType.MainUI, DaniTechUIType.MainUI);
        if (mainUI is MainUI mUi)
        {
            mUi.RefreshUmbra();
        }

    }
}
