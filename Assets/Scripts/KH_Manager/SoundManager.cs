using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {  get; set; }

    [SerializeField] private AudioSource AudioSource_BGM;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayBGMOnLoop(string bgmPath)
    {
        DaniTechGameUtil.LoadAndPlayAudioClip(AudioSource_BGM, bgmPath, isLoop: true).Forget();
    }

    public void PlayBGMOffLoop(string bgmPath)
    {
        DaniTechGameUtil.LoadAndPlayAudioClip(AudioSource_BGM, bgmPath, isLoop: false).Forget();
    }

    public void StopBGM()
    {
        if (AudioSource_BGM == null) return;
        AudioSource_BGM.Stop();
    }
}
