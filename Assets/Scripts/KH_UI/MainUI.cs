using UnityEngine;

public class MainUI : DaniTechUIBase
{
    [SerializeField] private DaniTechUIButton Button_Energy;
    [SerializeField] private DaniTechUIButton Button_Chip;
    [SerializeField] private DaniTechUIButton Button_Jewel;

    [SerializeField] private DaniTechUIButton Button_Store;
    [SerializeField] private DaniTechUIButton Button_Lab;
    [SerializeField] private DaniTechUIButton Button_Chapter;
    [SerializeField] private DaniTechUIButton Button_Chipset;
    [SerializeField] private DaniTechUIButton Button_Buddy;

    private void OnEnable()
    {
        Button_Energy.BindOnClickButtonEvent(OnClick_Energy);
        Button_Chip.BindOnClickButtonEvent(OnClick_Chip);
        Button_Jewel.BindOnClickButtonEvent(OnClick_Jewel);

        Button_Store.BindOnClickButtonEvent(OnClick_Store);
        Button_Lab.BindOnClickButtonEvent(OnClick_Lab);
        Button_Chapter.BindOnClickButtonEvent(OnClick_Chapter);
        Button_Chipset.BindOnClickButtonEvent(OnClick_Chipset);
        Button_Buddy.BindOnClickButtonEvent(OnClick_Buddy);

        SoundManager.Instance.PlayBGMOnLoop("Sound/BGM_MainUI");
    }

    private void OnDisable()
    {
        SoundManager.Instance.StopBGM();
    }

    public void OnClick_Energy()
    {
        Debug.Log("에너지상점열기");
    }

    public void OnClick_Chip()
    {
        Debug.Log("칩셋상점열기");
    }

    public void OnClick_Jewel()
    {
        Debug.Log("보석상점열기");
    }


    public void OnClick_Store()
    {
        Debug.Log("상점열기");
    }

    public void OnClick_Lab()
    {
        Debug.Log("연구실열기");

    }

    public void OnClick_Chapter()
    {
        Debug.Log("챕터열기");
        DaniTechUIManager.Instance.OpenChapterPopup();
    }

    public void OnClick_Chipset()
    {
        Debug.Log("칩셋열기");

    }

    public void OnClick_Buddy()
    {
        Debug.Log("버디열기");

    }
}
