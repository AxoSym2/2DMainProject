using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class EndingUI : DaniTechUIBase
{
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private RawImage _rawImage;

    private void OnEnable()
    {
        _videoPlayer.loopPointReached += OnVideoEnd;
        _videoPlayer.Play();
    }

    private void OnDisable()
    {
        _videoPlayer.loopPointReached -= OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        DaniTechGameManager.Inst.ReturnToMainUI();
    }
}
