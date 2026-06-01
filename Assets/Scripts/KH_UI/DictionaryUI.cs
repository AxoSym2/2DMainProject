using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DictionaryCategory
{
    None,
    PlayerUnit,
    EnemyUnit,
    Item,
    Skill
}

public class DictionaryUI : MonoBehaviour
{
    [SerializeField] private Text Text_Name;
    [SerializeField] private Text Text_Description;
    [SerializeField] private Image Image_Icon;

    [SerializeField] private DaniTechUIButton PlayerUnit_Btn;
    [SerializeField] private DaniTechUIButton EnemyUnit_Btn;
    [SerializeField] private DaniTechUIButton Item_Btn;
    [SerializeField] private DaniTechUIButton Skill_Btn;

    [SerializeField] private DaniTechUIButton Next_Btn;
    [SerializeField] private DaniTechUIButton Previous_Btn;

    private DictionaryCategory _currentCategory = DictionaryCategory.EnemyUnit;
    private int _currentIndex = 0;
    private List<string> _currentIdList = new List<string>();

    private void OnEnable()
    {
        PlayerUnit_Btn.BindOnClickButtonEvent(OnClick_PlayerUnit);
        EnemyUnit_Btn.BindOnClickButtonEvent(OnClick_EnemyUnit);
        Item_Btn.BindOnClickButtonEvent(OnClick_Item);
        Skill_Btn.BindOnClickButtonEvent(OnClick_Skill);
        Next_Btn.BindOnClickButtonEvent(OnClick_Next);
        Previous_Btn.BindOnClickButtonEvent(OnClick_Previous);
        SetCategory(DictionaryCategory.EnemyUnit);
    }

    private void OnDisable()
    {
        PlayerUnit_Btn.UnBindOnClickButtonEvent(OnClick_PlayerUnit);
        EnemyUnit_Btn.UnBindOnClickButtonEvent(OnClick_EnemyUnit);
        Item_Btn.UnBindOnClickButtonEvent(OnClick_Item);
        Skill_Btn.UnBindOnClickButtonEvent(OnClick_Skill);
        Next_Btn.UnBindOnClickButtonEvent(OnClick_Next);
        Previous_Btn.UnBindOnClickButtonEvent(OnClick_Previous);
    }

    private void SetCategory(DictionaryCategory category)
    {
        _currentCategory = category;
        _currentIndex = 0;
        _currentIdList.Clear();

        switch (category)
        {
            case DictionaryCategory.PlayerUnit:
                foreach(var key in DaniTechGameDataManager.Instance.PlayerUnitDataList.Keys)
                    _currentIdList.Add(key);
                break;
            case DictionaryCategory.EnemyUnit:
                foreach (var key in DaniTechGameDataManager.Instance.EnemyUnitDataList.Keys)
                    _currentIdList.Add(key);
                break;
            case DictionaryCategory.Item:
                //foreach (var key in DaniTechGameDataManager.Instance.PlayerUnitDataList.Keys)
                break;
            case DictionaryCategory.Skill:
                foreach (var key in DaniTechGameDataManager.Instance.SkillDataList.Keys)
                    _currentIdList.Add(key);
                break;
        }
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (_currentIdList.Count == 0) return;
        string id = _currentIdList[_currentIndex];

        switch (_currentCategory)
        {
            case DictionaryCategory.PlayerUnit:
                PlayerUnitData playerData = DaniTechGameDataManager.Instance.GetPlayerUnitData(id);
                if (playerData == null) return;
                Text_Name.text = playerData.Name;
                Text_Description.text = playerData.Description;
                //아이콘 추가
                break;
            case DictionaryCategory.EnemyUnit:
                EnemyUnitData enemyData = DaniTechGameDataManager.Instance.GetEnemyUnitData(id);
                if (enemyData == null) return;
                Text_Name.text = enemyData.Name;
                Text_Description.text = enemyData.Description;
                LoadIcon(enemyData.IconPath);
                break;
            case DictionaryCategory.Skill:
                SkillData skillData = DaniTechGameDataManager.Instance.GetSkillsData(id);
                if (skillData == null) return;
                Text_Name.text = skillData.Name;
                Text_Description.text = skillData.Description;
                LoadIcon(skillData.IconPath);
                break;
        }
    }

    private void LoadIcon(string iconPath)
    {
        if (string.IsNullOrEmpty(iconPath))
        {
            Image_Icon.gameObject.SetActive(false);
            return;
        }

        Sprite sprite = Resources.Load<Sprite>(iconPath);
        if(sprite != null)
        {
            Image_Icon.gameObject.SetActive(true);
            Image_Icon.sprite = sprite;
        }
    }

    private void OnClick_PlayerUnit() { SetCategory(DictionaryCategory.PlayerUnit); }
    private void OnClick_EnemyUnit() { SetCategory(DictionaryCategory.EnemyUnit); }
    private void OnClick_Skill() { SetCategory(DictionaryCategory.Skill); }
    private void OnClick_Item() { SetCategory(DictionaryCategory.Item); }

    private void OnClick_Previous()
    {
        if (_currentIndex > 0)
        {
            _currentIndex--;
            RefreshUI();
        }
    }

    private void OnClick_Next()
    {
        if (_currentIndex < _currentIdList.Count - 1)
        {
            _currentIndex++;
            RefreshUI();
        }
    }
}
