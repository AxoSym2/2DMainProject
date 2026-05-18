using UnityEngine;

public class RobbyUI : DaniTechUIBase
{
    [SerializeField] private DaniTechUIButton Button_GameStart;
    [SerializeField] private DaniTechUIButton Button_GameSetting;
    [SerializeField] private DaniTechUIButton Button_GameQuit;

    private void OnEnable()
    {
        Button_GameStart.BindOnClickButtonEvent(OnClick_GameStart);
        Button_GameSetting.BindOnClickButtonEvent(OnClick_GameSetting);
        Button_GameQuit.BindOnClickButtonEvent(OnClick_GameQuit);
    }

    private void OnDisable()
    {
        
    }

    public void OnClick_GameStart()
    {
        //Debug.Log("게임시작");
        DaniTechUIManager.Instance.CloseContentUI(DaniTechUIType.RobbyUI);
    }

    public void OnClick_GameSetting()
    {
        Debug.Log("세팅열기");
    }

    public void OnClick_GameQuit()
    {
        //Debug.Log("게임종료");
        DaniTechGameManager.Inst.SaveAndEndGame();
    }
}
