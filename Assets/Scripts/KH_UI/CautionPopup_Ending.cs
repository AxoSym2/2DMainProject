using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

public class CautionPopup_Ending : DaniTechUIBase
{
    private void OnEnable()
    {
        AutoClose().Forget();
    }

    private async UniTaskVoid AutoClose()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(3f));
        DaniTechUIManager.Instance.ClosePopupUI(DaniTechUIType.CautionPopup_Ending);
    }
}
