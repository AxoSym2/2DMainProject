using UnityEngine;
using UnityEngine.UI;

public class EndingIcon : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Sprite[] _iconSprites;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        int clearedCount = DaniTechGameManager.Inst.GetClearedChapterCount();
        int spriteIdx = Mathf.Clamp(clearedCount, 0, _iconSprites.Length - 1);
        _iconImage.sprite = _iconSprites[spriteIdx];
    }
}
