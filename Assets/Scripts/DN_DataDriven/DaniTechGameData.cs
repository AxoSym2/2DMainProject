using System;
using System.Collections.Generic;

[System.Serializable]
public class GameDataBase
{
    public string Id;
}

// C# 때와 약간 달라진 점
    // Syste.Text.Json대신 유니티 내장 JsonUtility를 사용
    // 따라서 프로퍼티말고 그냥 일반 public 멤버변수로 변경함
    // [System.Serializable]가 없다면 JsonUtility는 데이터를 무시

[System.Serializable]
public class DNCharacterData : GameDataBase
{
    public string Name;
    public string SkillList;
    public string UseWeaponId;
    public string BasicCostumeId;
}

[System.Serializable]
public class DNSkillData : GameDataBase
{
    public string Name;
    public string Description;
}

[System.Serializable]
public class DNWeaponData : GameDataBase
{
    public string Name;
    public string Description;
}

[System.Serializable] 
public class DNCostumeData : GameDataBase
{
    public string Name;
    public string Description;
}

[System.Serializable]
public class DNItemData : GameDataBase
{
    public string Name;
    public string Description;
    public string ItemType;
    public string Grade;
    public string MaxStackCount;
    public string SellingPrice;
    public string IconPath;
}

[System.Serializable]
public class DNDialogueGroupData : GameDataBase
{
    public List<string> DialogueIdList;
}

[System.Serializable]
public class DNDialogueData : GameDataBase
{
    public string CharacterDataId;
    public string Description;
    public string NextDialogueId;
    public List<string> SelectionNameList;
    public List<string> SelectionDialogueIdList;
    public string TexturePath;
    public string VoicePath;
}

[System.Serializable]
public class DNFieldObjectData : GameDataBase
{
    public string Name;
    public string Description;
    public string FieldObjectType;
    public List<int> DropCountRange;
    public string DropItemDataId;
    public string IconPath;
    public string PrefabPath;
}

[System.Serializable]
public class DNMonsterData : GameDataBase
{
    public string Name;
    public string Description;
    public string IconPath;
    public string PrefabPath;
}

[System.Serializable]
public class ChapterData: GameDataBase
{
    public string Name;
    public string Description;
    public string IconPath;
    public string PrefabPath;
    public string ChapterNum;
}

[System.Serializable]
public class PlayerUnitData: GameDataBase
{
    public string Name;
    public string PrefabPath;
    public float Hp;
    public float MoveSpeed;
    public string SkillId;
}

[System.Serializable]
public class EnemyUnitData: GameDataBase
{
    public string Name;
    public string IconPath;
    public string PrefabPath;
    public string EnemyType;
    public float ExpReward;
    public float Hp;
    public float MoveSpeed;
    public float AttackDamage;
    public float AttackCoolDown;
}

[System.Serializable]
public class WaveData: GameDataBase
{
    public string ChapterId;
    public int WaveNumber;
    public string EnemyIdList;
    public int SpawnCount;
    public float SpawnInterval;
}

[System.Serializable]
public class SkillData: GameDataBase
{
    public string Name;
    public string Description;
    public string IconPath;
    public string PrefabPath;
    public float Damage;
    public float CoolDown;
    public float Range;
    public float Duration;
    public string SkillType;
    public string ProjectilePath;
}

[System.Serializable]
public class DialogueData: GameDataBase
{
    public string CharacterDataId;
    public string Description;
    public string NextDialogueId;
    public string TexturePath;
}

[System.Serializable]
public class DialogueGroupData : GameDataBase
{
    public List<string> DialogueIdList;
}