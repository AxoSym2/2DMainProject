using UnityEngine;

public class RobbyUI : DaniTechUIBase
{
    [SerializeField] private DaniTechUIButton Button_GameStart;
    [SerializeField] private DaniTechUIButton Button_GameQuit;

    private void OnEnable()
    {
        Button_GameStart.BindOnClickButtonEvent(OnClick_GameStart);
        Button_GameQuit.BindOnClickButtonEvent(OnClick_GameQuit);
    }

    private void OnDisable()
    {
        Button_GameStart.UnBindOnClickButtonEvent(OnClick_GameStart);
        Button_GameQuit.UnBindOnClickButtonEvent(OnClick_GameQuit);
    }

    public void OnClick_GameStart()
    {
        //Debug.Log("게임시작");

        DaniTechUIManager.Instance.CloseContentUI(DaniTechUIType.RobbyUI);
        DaniTechUIManager.Instance.OpenLoadingUI();
    }

    public void OnClick_GameQuit()
    {
        //Debug.Log("게임종료");
        DaniTechGameManager.Inst.SaveAndEndGame();
    }
}
