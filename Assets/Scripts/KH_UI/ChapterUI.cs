using UnityEngine;

public class ChapterUI : MonoBehaviour
{
    [SerializeField] private string BGMPath;

    private void OnEnable()
    {
        SoundManager.Instance.PlayBGMOnLoop(BGMPath);
    }

    private void OnDisable()
    {
        SoundManager.Instance.StopBGM();    
    }
}
