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
    private Queue<string> _descriptionQueue = new Queue<string>();


    private void OnEnable()
    {
        Button_Next.BindOnClickButtonEvent(OnClick_Next);
    }

    public void OnClick_Next()
    {
        bool isNextDescriptionExist = CheckAndStartNextDialogue();

        if (isNextDescriptionExist) 
        {
            return;
        }

        bool isNextDialogueExist = CheckAndStartNextDialogue();
        if (isNextDialogueExist)
        {
            DaniTechUIManager.Instance.CloseContentUI(DaniTechUIType.DialogueUI);
        }
    }

    private bool CheckAndStartNextDialogue()
    {
        // var dialogueData = DaniTechGameDataManager.Instance.Ge
        return false;
    }

    public void StartDialogue(string dialogueId)
    {

    }

    private bool CheckAndSetDescription()
    {
        return false;
    }

    private void SetCharactername(string characterDataId)
    {

    }

    private void SetCurrentDialogueDescription(string description)
    {

    }
}
