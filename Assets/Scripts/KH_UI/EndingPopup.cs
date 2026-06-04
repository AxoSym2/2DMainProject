using UnityEngine;
using UnityEngine.UI;

public class EndingPopup : DaniTechUIBase
{
    [SerializeField] private Image Image_Ending;
    [SerializeField] private Sprite[] EndingSprites;
    [SerializeField] private DaniTechUIButton Button_Next;

    private int _currentIndex = 0;

    private void OnEnable()
    {
        _currentIndex = 0;
        Button_Next.BindOnClickButtonEvent(OnClick_Next);
        RefreshUI();
        SoundManager.Instance.PlayBGMOnLoop("Sound/");
    }

    private void OnDisable()
    {
        Button_Next.UnBindOnClickButtonEvent(OnClick_Next);
        SoundManager.Instance.StopBGM();
    }

    private void RefreshUI()
    {
        if (EndingSprites == null || EndingSprites.Length == 0) return;
        Image_Ending.sprite = EndingSprites[_currentIndex];
    }

    private void OnClick_Next()
    {
        if (_currentIndex < EndingSprites.Length - 1)
        {
            _currentIndex++;
            RefreshUI();
        }
        else
        {
            //Debug.Log("엔딩 클리어");
            DaniTechGameManager.Inst.SetEndingCleared();
            DaniTechGameManager.Inst.SaveData();
            DaniTechUIManager.Instance.CloseContentUI(DaniTechUIType.EndingPopup);

            var mainUI = DaniTechUIManager.Instance.GetCreatedUI(DaniTechUIRootType.MainUI, DaniTechUIType.MainUI);
            if (mainUI is MainUI mUi)
            {
                mUi.RefreshBG();
            }
        }
    }
}
