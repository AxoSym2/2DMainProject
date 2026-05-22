using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectSlot : DaniTechUIBase
{
    [SerializeField] private Text Text_SkillName;
    [SerializeField] private Text Text_SkillDescription;
    [SerializeField] private Image Image_SkillIcon;
    [SerializeField] private DaniTechUIButton Button_Select;

    private SkillData _skillData;

    private void OnDisable()
    {
        Button_Select.UnBindOnClickButtonEvent(OnClick_Select);
    }

    public void Init(SkillData skillData)
    {
        _skillData = skillData;
        Text_SkillName.text = skillData.Name;
        Text_SkillDescription.text = skillData.Description;
        Button_Select.BindOnClickButtonEvent(OnClick_Select);

        if(string.IsNullOrEmpty(skillData.IconPath) == false)
        {
            Sprite Icon = Resources.Load<Sprite>(skillData.IconPath);
            if (Icon != null )
            {
                Image_SkillIcon.sprite = Icon;
            }
        }
    }

    private void OnClick_Select()
    {
        Debug.Log($"스킬 선택: {_skillData.Name}");
        DaniTechGameManager.Inst.AddPlayerSkill(_skillData.Id);
        DaniTechUIManager.Instance.ClosePopupUI(DaniTechUIType.LevelUpPopup);
        Time.timeScale = 1f;
    }
}
