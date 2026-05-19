using UnityEngine;

public class InGameUI : DaniTechUIBase
{
    [SerializeField] private DaniTechUIButton Button_Pause;

    private void OnEnable()
    {
        Button_Pause.BindOnClickButtonEvent(OnClick_Pause);
    }

    public void OnClick_Pause()
    {
        DaniTechUIManager.Instance.OpenPopupUI(DaniTechUIType.PausePopup);
    }
}
