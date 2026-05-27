using UnityEngine;
using UnityEngine.UI;

public class InGameUI : DaniTechUIBase
{
    [SerializeField] private DaniTechUIButton Button_Pause;
    [SerializeField] private Text Text_WaveNum;

    [SerializeField] private Slider Slider_Health;
    [SerializeField] private Text Text_Health;

    [SerializeField] private Slider Slider_Exp;
    [SerializeField] private Text Text_Level;

    [SerializeField] private Text Text_Umbra;

    private void OnEnable()
    {
        Button_Pause.BindOnClickButtonEvent(OnClick_Pause);
    }

    public void OnClick_Pause()
    {
        DaniTechUIManager.Instance.OpenPopupUI(DaniTechUIType.PausePopup);
    }

    public void SetWaveNum(int waveNum)
    {
        Text_WaveNum.text = $"Wave {waveNum}";
    }

    public void SetHealthBar(float currentHp, float maxHp)
    {
        Slider_Health.value = currentHp / maxHp;
        Text_Health.text = $"{(int)currentHp} / {(int)maxHp}";
    }

    public void SetExpBar(float currentExp, float maxExp, int level)
    {
        Slider_Exp.value = currentExp / maxExp;
        Text_Level.text = $"Lv.{level:D2}";
    }

    public void SetUmbra(int amount)
    {
        Text_Umbra.text = $"{amount}";
    }

    public void RefreshUmbra()
    {
        Text_Umbra.text = $"{DaniTechGameManager.Inst.GetUmbra()}";
    }
}
