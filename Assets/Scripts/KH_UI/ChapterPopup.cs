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

    [SerializeField] private GameObject LockObject;
    [SerializeField] private Text Text_LockCondition;
    [SerializeField] private Text Text_LockChapterNum;
    [SerializeField] private bool _isDebugMode = false;

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

        bool isUnlocked = IsChapterUnlocked(_currentChapterIdx);
        Button_StartChapter.gameObject.SetActive(isUnlocked);
        LockObject.SetActive(!isUnlocked);

        if (!isUnlocked)
        {
            Text_LockCondition.text = $"챕터{_currentChapterIdx - 1} 클리어 필요";
            Text_LockChapterNum.text = $"챕터 {_currentChapterIdx}";
        }
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
        if (IsChapterUnlocked(_currentChapterIdx) == false) return;
        DaniTechUIManager.Instance.OpenLoadingUI();
        DaniTechGameManager.Inst.StartChapter(_currentChapterIdx);
        //Debug.Log("챕터시작");
    }

    private bool IsChapterUnlocked(int chapterIdx)
    {
        if (_isDebugMode) return true;
        if (chapterIdx == 1) return true;
        string preChapterId = $"Chapter_Earth_{chapterIdx - 1}";
        return DaniTechGameManager.Inst.IsChapterCleared(preChapterId);
    }
}
