using UnityEngine;
using UnityEngine.UI;

public class InGameUI : DaniTechUIBase
{
    [SerializeField] private DaniTechUIButton Button_Pause;
    [SerializeField] private Text Text_WaveNum;

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
}
