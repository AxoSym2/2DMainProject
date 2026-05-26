using UnityEngine;

public class ChamberUI : DaniTechUIBase
{
    [SerializeField] private UpgradeSlot Slot_Attack; 
    [SerializeField] private UpgradeSlot Slot_Hp; 
    [SerializeField] private UpgradeSlot Slot_Defense; 
    [SerializeField] private UpgradeSlot Slot_CoolDown;

    private void OnEnable()
    {
        Slot_Attack.Init("Attack");
        Slot_Hp.Init("Hp");
        Slot_Defense.Init("Defense");
        Slot_CoolDown.Init("CoolDown");
    }
}
