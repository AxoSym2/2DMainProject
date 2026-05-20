using UnityEngine;

public class LaboratoryUI : DaniTechUIBase
{
    [SerializeField] private DaniTechUIButton Select_Jade;
    [SerializeField] private DaniTechUIButton Select_Chris;
    [SerializeField] private DaniTechUIButton Select_Alice;
    [SerializeField] private DaniTechUIButton Select_Rochelle;

    [SerializeField] private GameObject Checkmark_Jade;
    [SerializeField] private GameObject Checkmark_Chris;
    [SerializeField] private GameObject Checkmark_Alice;
    [SerializeField] private GameObject Checkmark_Rochelle;

    private void OnEnable()
    {
        Select_Jade.BindOnClickButtonEvent(OnClick_Jade);
        Select_Chris.BindOnClickButtonEvent(OnClick_Chris);
        Select_Alice.BindOnClickButtonEvent(OnClick_Alice);
        Select_Rochelle.BindOnClickButtonEvent(OnClick_Rochelle);

        SetCheckMark(DaniTechGameManager.Inst.GetSelectedPlayerUnitId());
    }

    private void SetCheckMark(string selectedId)
    {
        //Debug.Log($"SetCheckMark 호출됨: {selectedId}");
        Checkmark_Jade.SetActive(selectedId == "Player_Unit_Male_01");
        Checkmark_Chris.SetActive(selectedId == "Player_Unit_Male_02");
        Checkmark_Alice.SetActive(selectedId == "Player_Unit_FeMale_01");
        Checkmark_Rochelle.SetActive(selectedId == "Player_Unit_FeMale_02");

        DaniTechGameManager.Inst.SetSelectedPlayerUnit(selectedId);
    }

    public void OnClick_Jade()
    {
        SetCheckMark("Player_Unit_Male_01");
    }

    public void OnClick_Chris()
    {
        SetCheckMark("Player_Unit_Male_02");
    }

    public void OnClick_Alice()
    {
        SetCheckMark("Player_Unit_FeMale_01");
    }

    public void OnClick_Rochelle()
    {
        SetCheckMark("Player_Unit_FeMale_02");
    }

}
