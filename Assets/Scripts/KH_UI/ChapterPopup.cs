using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterPopup : DaniTechUIBase
{
    [SerializeField] private DaniTechUIButton Button_StartChapter;
    [SerializeField] private DaniTechUIButton Button_PreviousChapter;
    [SerializeField] private DaniTechUIButton Button_NextChapter;

    [SerializeField] private Text Text_ChapterNum;
    [SerializeField] private Text Text_ChapterName;
    [SerializeField] private RawImage RawImage_Thumbnail;
    [SerializeField] private Text Text_Description;

    private int _currentChapterIdx = 1;
    private int _maxChapterIdx = 7;


    private void OnEnable()
    {
        Button_StartChapter.BindOnClickButtonEvent(OnClick_StartChpater);
        Button_PreviousChapter.BindOnClickButtonEvent(OnClick_PreviousChpater);
        Button_NextChapter.BindOnClickButtonEvent(OnClick_NextChpater);
        RefreshUI();
    }

    private void RefreshUI()
    {
        string chapterId = $"Chapter_Earth_{_currentChapterIdx}";
        ChapterData data = DaniTechGameDataManager.Instance.GetChapterData(chapterId);
        if (data == null)
        {
            Debug.LogWarning($"챕터 데이터 없음: {chapterId}");
            return;
        }

        Text_ChapterName.text = data.Name;
        Text_Description.text = data.Description.Replace("\\n", "\n");
        Text_ChapterNum.text = data.ChapterNum;
        DaniTechGameUtil.LoadAndSetTexture(RawImage_Thumbnail, data.IconPath).Forget();
    }

    public void OnClick_PreviousChpater()
    {
        if(_currentChapterIdx >1)
        {
            _currentChapterIdx--;
            RefreshUI();
        }

        //Debug.Log("이전챕터");
    }

    public void OnClick_NextChpater()
    {
        if( _currentChapterIdx < _maxChapterIdx)
        {
            _currentChapterIdx++;
            RefreshUI();
        }

        //Debug.Log("다음챕터");
    }

    public void OnClick_StartChpater()
    {
        DaniTechUIManager.Instance.OpenLoadingUI();
        DaniTechGameManager.Inst.StartChapter(_currentChapterIdx);
        //Debug.Log("챕터시작");
    }
}
