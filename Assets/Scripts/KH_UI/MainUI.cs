using UnityEngine;
using UnityEngine.UI;

public class MainUI : DaniTechUIBase
{
    [SerializeField] private Text Text_Umbra;

    [SerializeField] private DaniTechUIButton Button_Store;
    [SerializeField] private DaniTechUIButton Button_Lab;
    [SerializeField] private DaniTechUIButton Button_Chapter;
    [SerializeField] private DaniTechUIButton Button_Chipset;
    [SerializeField] private DaniTechUIButton Button_Buddy;

    private void OnEnable()
    {
        Button_Store.BindOnClickButtonEvent(OnClick_Store);
        Button_Lab.BindOnClickButtonEvent(OnClick_Lab);
        Button_Chapter.BindOnClickButtonEvent(OnClick_Chapter);
        Button_Chipset.BindOnClickButtonEvent(OnClick_Chipset);
        Button_Buddy.BindOnClickButtonEvent(OnClick_Buddy);

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

    public void OnClick_Store()
    {
        DaniTechUIManager.Instance.CloseAllContentUI();
        Debug.Log("상점열기");
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

    public void OnClick_Chipset()
    {
        DaniTechUIManager.Instance.CloseAllContentUI();
        Debug.Log("칩셋열기");

    }

    public void OnClick_Buddy()
    {
        DaniTechUIManager.Instance.CloseAllContentUI();
        Debug.Log("버디열기");

    }
}
