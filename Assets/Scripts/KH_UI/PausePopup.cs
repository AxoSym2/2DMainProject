using UnityEngine;

public class PausePopup : DaniTechUIBase
{
    [SerializeField] private DaniTechUIButton Button_Resume;
    [SerializeField] private DaniTechUIButton Button_Setting;
    [SerializeField] private DaniTechUIButton Button_Quit;

    private void OnEnable()
    {
        Button_Resume.BindOnClickButtonEvent(OnClick_Resume);
        Button_Setting.BindOnClickButtonEvent(OnClick_Setting);
        Button_Quit.BindOnClickButtonEvent(OnClick_Quit);
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void OnClick_Resume()
    {
        DaniTechUIManager.Instance.ClosePopupUI(DaniTechUIType.PausePopup);
    }

    public void OnClick_Setting()
    {
        Debug.Log("설정팝업");
    }

    public void OnClick_Quit()
    {
        Time.timeScale = 1f;
        DaniTechGameManager.Inst.ReturnToMainUI();
    }

}
