using UnityEngine;
using UnityEngine.UI;

public class ChapterPopup : DaniTechUIBase
{
    [SerializeField] private DaniTechUIButton Button_StartChapter;
    [SerializeField] private DaniTechUIButton Button_PreviousChapter;
    [SerializeField] private DaniTechUIButton Button_NextChapter;

    private void OnEnable()
    {
        Button_StartChapter.BindOnClickButtonEvent(OnClick_StartChpater);
        Button_PreviousChapter.BindOnClickButtonEvent(OnClick_PreviousChpater);
        Button_NextChapter.BindOnClickButtonEvent(OnClick_NextChpater);
    }

    public void OnClick_PreviousChpater()
    {
        Debug.Log("이전챕터");
    }

    public void OnClick_NextChpater()
    {
        Debug.Log("다음챕터");
    }

    public void OnClick_StartChpater()
    {
        Debug.Log("챕터시작");
    }
}
