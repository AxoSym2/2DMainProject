using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPopup : DaniTechUIBase
{
    [SerializeField] private SkillSelectSlot Slot_01;
    [SerializeField] private SkillSelectSlot Slot_02;
    [SerializeField] private SkillSelectSlot Slot_03;


    public void Init(List<SkillData> skillList)
    {
        Slot_01.Init(skillList[0]);
        Slot_02.Init(skillList[1]);
        Slot_03.Init(skillList[2]);
    }
}
