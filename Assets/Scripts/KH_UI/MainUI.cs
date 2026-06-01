using UnityEngine;
using UnityEngine.UI;

public class MainUI : DaniTechUIBase
{
    [SerializeField] private Text Text_Umbra;

    [SerializeField] private DaniTechUIButton Button_Dictionary;
    [SerializeField] private DaniTechUIButton Button_Lab;
    [SerializeField] private DaniTechUIButton Button_Chapter;
    [SerializeField] private DaniTechUIButton Button_Chamber;
    [SerializeField] private DaniTechUIButton Button_Ending;

    private void OnEnable()
    {
        Button_Dictionary.BindOnClickButtonEvent(OnClick_Dictionary);
        Button_Lab.BindOnClickButtonEvent(OnClick_Lab);
        Button_Chapter.BindOnClickButtonEvent(OnClick_Chapter);
        Button_Chamber.BindOnClickButtonEvent(OnClick_Chamber);
        Button_Ending.BindOnClickButtonEvent(OnClick_Ending);

        SoundManager.Instance.PlayBGMOnLoop("Sound/BGM_MainUI");
        RefreshUmbra();
    }

    private void OnDisable()
    {
        //SoundManager.Instance.StopBGM();
    }

    public void OnClick_Energy()
    {
        Debug.Log("에너지상점열기");
    }

    public void OnClick_Chip()
    {
        Debug.Log("칩셋상점열기");
    }

    public void RefreshUmbra()
    {
        Text_Umbra.text = $"{DaniTechGameManager.Inst.GetUmbra()}";
    }

    public void OnClick_Dictionary()
    {
        DaniTechUIManager.Instance.CloseAllContentUI();
        //Debug.Log("도감열기");
        DaniTechUIManager.Instance.OpenDictionaryUI();
    }

    public void OnClick_Lab()
    {
        DaniTechUIManager.Instance.CloseAllContentUI();
        //Debug.Log("실험실열기");
        DaniTechUIManager.Instance.OpenLabUI();
    }

    public void OnClick_Chapter()
    {
        DaniTechUIManager.Instance.CloseAllContentUI();
        //Debug.Log("챕터열기");
        DaniTechUIManager.Instance.OpenChapterUI();
    }

    public void OnClick_Chamber()
    {
        DaniTechUIManager.Instance.CloseAllContentUI();
        //Debug.Log("챔버열기");
        DaniTechUIManager.Instance.OpenChamberUI();

    }

    public void OnClick_Ending()
    {
        DaniTechUIManager.Instance.CloseAllContentUI();
        //Debug.Log("엔딩열기");
        DaniTechUIManager.Instance.OpenEndingUI();
    }
}
