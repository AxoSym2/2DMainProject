using UnityEngine;

public enum ChapterClearType
{
    None = 0,
    AllKill,
    BossKill
}

public class ChapterUI : MonoBehaviour
{
    [SerializeField] private string BGMPath;
    [SerializeField] public string ChapterId;
    [SerializeField] public ChapterClearType ClearType;
    [SerializeField] public string StartDialogueGroupId;
    [SerializeField] public string ClearDialogueGroupId;

    private void OnEnable()
    {
        //Debug.Log("ChapterUI OnEnable 호출됨");
        if (SoundManager.Instance == null) return;
        SoundManager.Instance.PlayBGMOnLoop(BGMPath);
    }

    private void OnDisable()
    {
        if (SoundManager.Instance == null) return;
        SoundManager.Instance.StopBGM();    
    }
}
