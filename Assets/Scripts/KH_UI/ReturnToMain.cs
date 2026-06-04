using UnityEngine;

public class ReturnToMain : DaniTechUIBase
{
    [SerializeField] DaniTechUIButton ReturnToMain_Btn;

    private void OnEnable()
    {
        ReturnToMain_Btn.BindOnClickButtonEvent(OnClick_ReturnToMain);
    }

    private void OnDisable()
    {
        ReturnToMain_Btn.UnBindOnClickButtonEvent(OnClick_ReturnToMain);
    }

    private void OnClick_ReturnToMain()
    {
        Time.timeScale = 1f;
        DaniTechUIManager.Instance.ClosePopupUI(DaniTechUIType.ReturnToMainPopup);
        DaniTechGameManager.Inst.ReturnToMainUI();
    }
}
