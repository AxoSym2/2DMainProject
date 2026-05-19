using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : DaniTechUIBase
{
    [SerializeField] private RawImage RawImage_Loading;
    [SerializeField] private Slider Slider_LoadingBar;

    private CancellationTokenSource _cancelToken;
    float[] _pausePoints = { 0.1f, 0.1f, 0.1f };
    int _pauseIndex = 0;

    private void OnEnable()
    {
        LoadAndSetLoadingImage();
    }

    private void LoadAndSetLoadingImage()
    {
        int randomIdx = UnityEngine.Random.Range(0, 5);

        string texturePath = string.Empty;
        switch (randomIdx)
        {
            case 0:
                texturePath = "Texture2D/Apocalypse_Project_Loading_Image1";
                break;
            case 1:
                texturePath = "Texture2D/Apocalypse_Project_Loading_Image2";
                break;
            case 2:
                texturePath = "Texture2D/Apocalypse_Project_Loading_Image3";
                break;
            case 3:
                texturePath = "Texture2D/Apocalypse_Project_Loading_Image4";
                break;
            case 4:
                texturePath = "Texture2D/Apocalypse_Project_Loading_Image5";
                break;
        }

        DaniTechGameUtil.LoadAndSetTexture(RawImage_Loading, texturePath).Forget();
        StartLoadingResource(1f).Forget();
    }

    private async UniTaskVoid StartLoadingResource(float duration)
    {
        _cancelToken = new CancellationTokenSource();

        float elapsed = 0f;
        Slider_LoadingBar.value = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / duration);

            if(_pauseIndex <  _pausePoints.Length && progress >= _pausePoints[_pauseIndex])
            {
                float pausePointValue = _pausePoints[_pauseIndex];
                Slider_LoadingBar.value = pausePointValue;

                await UniTask.Delay(TimeSpan.FromSeconds(pausePointValue), cancellationToken: _cancelToken.Token);
                _pauseIndex++;
            }

            Slider_LoadingBar.value = progress;

            await UniTask.Yield(PlayerLoopTiming.Update, _cancelToken.Token);
        }

        Slider_LoadingBar.value = 1.0f;
        DaniTechUIManager.Instance.CloseLoadingUI();
    }
}
