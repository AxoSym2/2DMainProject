using UnityEngine;

public class ChapterUI : MonoBehaviour
{
    [SerializeField] private string BGMPath;

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
