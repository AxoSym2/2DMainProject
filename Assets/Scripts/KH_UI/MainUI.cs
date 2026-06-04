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

    [SerializeField] private RawImage RawImage_BG;
    [SerializeField] private string NormalBGPath;
    [SerializeField] private string EndingBGPath;

    [SerializeField] private bool _isDebugMode = false;

    private void OnEnable()
    {
        Button_Dictionary.BindOnClickButtonEvent(OnClick_Dictionary);
        Button_Lab.BindOnClickButtonEvent(OnClick_Lab);
        Button_Chapter.BindOnClickButtonEvent(OnClick_Chapter);
        Button_Chamber.BindOnClickButtonEvent(OnClick_Chamber);
        Button_Ending.BindOnClickButtonEvent(OnClick_Ending);

        RefreshUmbra();
        RefreshBG();

        if(_isDebugMode)
        {
            DaniTechGameManager.Inst.AddUmbra(999999999);
        }
    }

    private void OnDisable()
    {
        //SoundManager.Instance.StopBGM();
    }

    public void RefreshUmbra()
    {
        Text_Umbra.text = $"{DaniTechGameManager.Inst.GetUmbra()}";
    }

    public void RefreshBG()
    {
        bool isEndingCleared = DaniTechGameManager.Inst.GetEndingCleared();
        string bgPath = isEndingCleared ? EndingBGPath : NormalBGPath;
        DaniTechGameUtil.LoadAndSetTexture(RawImage_BG, bgPath).Forget();

        if (isEndingCleared)
        {
            SoundManager.Instance.PlayBGMOnLoop("Sound/BGM_MainUI_Ending");
        }
        else
            SoundManager.Instance.PlayBGMOnLoop("Sound/BGM_MainUI");
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

        if (_isDebugMode == false && DaniTechGameManager.Inst.GetClearedChapterCount() < 7)
        {
            DaniTechUIManager.Instance.OpenPopupUI(DaniTechUIType.CautionPopup_Ending);
            {
                return;
            }
        }
        DaniTechUIManager.Instance.OpenEndingUI();
    }
}
