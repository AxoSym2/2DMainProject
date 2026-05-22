using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : DaniTechUIBase
{
    [SerializeField] private Image Image_Character;
    [SerializeField] private Text Text_Character;
    [SerializeField] private Text Text_Description;
    [SerializeField] private DaniTechUIButton Button_Next;

    private string _currentDialogueId;

    private void OnEnable()
    {
        Button_Next.BindOnClickButtonEvent(OnClick_Next);
    }

    private void OnDisable()
    {
        Button_Next.UnBindOnClickButtonEvent(OnClick_Next);
    }

    public void StartDialogue(string dialogueGroupId)
    {
        DialogueGroupData groupData = DaniTechGameDataManager.Instance.GetDialogueGroupData(dialogueGroupId);
        if (groupData == null)
        {
            Debug.LogError($"다이얼로그 그룹 없음: {dialogueGroupId}");
            return;
        }
        _currentDialogueId = groupData.DialogueIdList[0];
        ShowCurrentDialogue();
    }

    public void OnClick_Next()
    {
        bool isNextDescriptionExist = CheckAndStartNextDialogue();

        if (isNextDescriptionExist) 
        {
            return;
        }

        DaniTechUIManager.Instance.CloseContentUI(DaniTechUIType.DialogueUI);
        Time.timeScale = 1f;
        DaniTechGameManager.Inst.ReturnToMainUI();
    }

    private bool CheckAndStartNextDialogue()
    {
        DialogueData dialogueData = DaniTechGameDataManager.Instance.GetDialogueData(_currentDialogueId);
        if (dialogueData == null) return false;

        if (string.IsNullOrEmpty(dialogueData.NextDialogueId)) return false;

        _currentDialogueId = dialogueData.NextDialogueId;
        ShowCurrentDialogue();
        return true;
    }

    private void ShowCurrentDialogue()
    {
        DialogueData dialogueData = DaniTechGameDataManager.Instance.GetDialogueData(_currentDialogueId);
        if (dialogueData == null) return;

        SetCurrentDialogueDescription(dialogueData.Description);
        SetCharactername(dialogueData.CharacterDataId);

        if (string.IsNullOrEmpty(dialogueData.TexturePath) == false)
        {
            string[] pathParts = dialogueData.TexturePath.Split('/');
            string spriteName = pathParts[pathParts.Length - 1];
            string folderPath = dialogueData.TexturePath.Substring(0, dialogueData.TexturePath.LastIndexOf('/'));

            Sprite[] sprites = Resources.LoadAll<Sprite>(folderPath);
            Sprite sprite = null;
            foreach(var s in sprites)
            {
                if (s.name == spriteName)
                {
                    sprite = s;
                    break;
                }
            }

            if(sprite != null)
            {
                Image_Character.sprite = sprite;
            }
        }
    }

    private void SetCharactername(string characterDataId)
    {
        string id = string.IsNullOrEmpty(characterDataId) ? DaniTechGameManager.Inst.GetSelectedPlayerUnitId() : characterDataId;

        PlayerUnitData playerUnitData = DaniTechGameDataManager.Instance.GetPlayerUnitData(id);
        if (playerUnitData == null) return;
        Text_Character.text = playerUnitData.Name;  
        
    }

    private void SetCurrentDialogueDescription(string description)
    {
        Text_Description.text = description;
    }
}
