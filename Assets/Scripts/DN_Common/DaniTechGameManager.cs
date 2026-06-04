using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;


public class DaniTechGameManager : MonoBehaviour
{
    public static DaniTechGameManager Inst { get; set; }

    // 플레이 중에 저장되어야 하는 정보들이 있는 위치
    private DaniTechPlayerModel _playerModel = new DaniTechPlayerModel();
    private GameObject _currentMap;
    private GameObject _currentPlayer;
    private string _selectedPlayerUnitId = "Player_Unit_Male_01";
    private int _currentLevel = 1;
    private float _currentExp = 0f;
    private string _pendingStartDialogueGroupId;
    private int _currentChapterUmbra = 0;

    private int _currentChapterIdx;
    private int _pendingSkillSelectionCount;

    private void Awake()
    {
        Inst = this;
        LoadSaveData();
    }

    public void SaveData()
    {
        DaniTechNetworkManager.Inst.RequstSaveData(_playerModel);
    }

    public void SaveAndEndGame()
    {
        SaveData();
        Application.Quit();
    }

    private void LoadSaveData()
    {
        _playerModel = DaniTechNetworkManager.Inst.RequstLoadSaveData();
    }

    public void AddItem(string itemDataId, int addItemCount)
    {
        // 저장할때 고유값 ID를 부여하기 위해 사용
        long uniqueId = DaniTechGameUtil.GenerateUniqueId();

        // TODO : 우선 쉽게 사용할 수 있도록 중복 처리는 빼두었다. 습득할때마다 아이템이 하나씩 추가되도록 해두고
        // 추후에 중복값은 StackCount가 다 찰때까지 누적해줄 수 있도록 로직을 추가하자
        var newItem = new DaniTechItemModel();
        newItem.ItemUniqueId = uniqueId;
        newItem.ItemDataId = itemDataId;
        newItem.ItemStackCount = addItemCount;

        _playerModel.ItemList.Add(newItem);
    }

    public List<DaniTechItemModel> GetPlayerItemList()
    {
        // _playerModel이 Private이므로 외부에서 ItemList를 받아올 수 있게 Get함수를 사용한다
        return _playerModel.ItemList;
    }

    public void OnLoadingComplete()
    {
        if (string.IsNullOrEmpty(_pendingStartDialogueGroupId) == false)
        {
            Time.timeScale = 0f;
            var ui = DaniTechUIManager.Instance.OpenContentUI(DaniTechUIType.DialogueUI);
            if (ui is DialogueUI dialogueUI)
            {
                dialogueUI.StartDialogue(_pendingStartDialogueGroupId, DialogueOpenType.ChapterStart);
            }
            _pendingStartDialogueGroupId = string.Empty;
            return;
        }

        if (_currentChapterIdx == 7)
        {
            ShowInitialSkillSelection().Forget();
            return;
        }
    }

    public int GetCurrentChapterIdx()
    {
        return _currentChapterIdx;
    }

    public void ShowBossChapterSkillSelection()
    {
        ShowInitialSkillSelection().Forget();
    }

    private async UniTaskVoid ShowInitialSkillSelection()
    {
        for (int i =0; i<3; i++)
        {
            OnLevelUp();
            await UniTask.WaitUntil(IsLevelUpPopupClosed);
        }
        Time.timeScale = 1f;
    }

    private bool IsLevelUpPopupClosed()
    {
        var popup = DaniTechUIManager.Instance.GetCreatedUI(DaniTechUIRootType.PopupUI, DaniTechUIType.LevelUpPopup);
        return popup == null || popup.gameObject.activeSelf == false;
    }

    public void OnReturnLoadingComplete()
    {
        DaniTechUIManager.Instance.OpenUI(DaniTechUIRootType.MainUI, DaniTechUIType.MainUI);
    }

    public void StartChapter(int chapterIdx)
    {
        _currentChapterIdx = chapterIdx;

        if (_currentMap != null)
        {
            Destroy(_currentMap);
        }

        if (_currentPlayer != null)
        {
            Destroy(_currentPlayer);
        }

        string chapterId = $"Chapter_Earth_{chapterIdx}";
        ChapterData data = DaniTechGameDataManager.Instance.GetChapterData(chapterId);

        if (data == null) 
        {
            Debug.LogError($"챕터 데이터 없음: {data}");
            return;
        }

        GameObject mapPrefab = Resources.Load<GameObject>(data.PrefabPath);
        if (mapPrefab == null) 
        {
            Debug.LogError($"맵을 찾을 수 없습니다.: {data.PrefabPath}");
            return;
        }

        _currentMap = Instantiate(mapPrefab);

        PlayerUnitData playerdata = DaniTechGameDataManager.Instance.GetPlayerUnitData(_selectedPlayerUnitId);
        //Debug.Log($"선택된 캐릭터 ID: {_selectedPlayerUnitId}");
        //Debug.Log($"playerdata: {playerdata}");
        if (playerdata == null)
        {
            Debug.LogError("플레이어 데이터 없음");
            return;
        }

        //Debug.Log($"PrefabPath: {playerdata.PrefabPath}");
        GameObject playerPrefab = Resources.Load<GameObject>(playerdata.PrefabPath);
        if (playerPrefab == null) 
        {
            Debug.LogError("플레이어 프리팹을 찾을 수 없습니다.");
            return;
        }

        _currentPlayer = Instantiate(playerPrefab);
        _currentPlayer.GetComponent<PlayerUnit_Base>().Init(_selectedPlayerUnitId);
        _currentPlayer.GetComponent<PlayerUnit_Attack>().Init(playerdata.SkillId);
        Camera.main.GetComponent<Camera_Tracking>().SetTarget(_currentPlayer.transform);

        DaniTechUIManager.Instance.CloseUI(DaniTechUIRootType.MainUI, DaniTechUIType.MainUI);
        DaniTechUIManager.Instance.ClosePopupUI(DaniTechUIType.ChapterUI);
        DaniTechUIManager.Instance.OpenInGameUI();

        EnemySpawnManager.Instance.Init($"Chapter_Earth_{chapterIdx}", _currentPlayer.transform);

        ChapterUI chapterUI = UnityEngine.Object.FindAnyObjectByType<ChapterUI>();
        if (chapterUI != null && string.IsNullOrEmpty(chapterUI.StartDialogueGroupId) == false)
        {
            _pendingStartDialogueGroupId = chapterUI.StartDialogueGroupId;
            var loadingUI = DaniTechUIManager.Instance.GetCreatedUI(DaniTechUIRootType.VeryFrontUI, DaniTechUIType.LoadingUI);
            if (loadingUI is LoadingUI loading)
            {
                loading.SetOnLoadingComplete(OnLoadingComplete);
            }
        }
        else if (chapterIdx == 7)
        {
            var loadingUI = DaniTechUIManager.Instance.GetCreatedUI(DaniTechUIRootType.VeryFrontUI, DaniTechUIType.LoadingUI);
            if (loadingUI is LoadingUI loading)
            {
                loading.SetOnLoadingComplete(OnLoadingComplete);
            }
        }
    }

    public void ReturnToMainUI()
    {
        _currentLevel = 1;
        _currentExp = 0;
        _currentChapterUmbra = 0;

        EnemySpawnManager.Instance.ClearAllEnemies();
        ClearAllItems();
        if (_currentMap != null) Destroy(_currentMap);
        if (_currentPlayer != null) Destroy(_currentPlayer);

        DaniTechUIManager.Instance.CloseUI(DaniTechUIRootType.VeryFrontUI, DaniTechUIType.PausePopup);
        DaniTechUIManager.Instance.ClosePopupUI(DaniTechUIType.LevelUpPopup);
        DaniTechUIManager.Instance.CloseUI(DaniTechUIRootType.MainUI, DaniTechUIType.InGameUI);

        DaniTechUIManager.Instance.OpenLoadingUI();
        var loadingUI = DaniTechUIManager.Instance.GetCreatedUI(DaniTechUIRootType.VeryFrontUI, DaniTechUIType.LoadingUI);
        if (loadingUI is LoadingUI loading)
        {
            loading.SetOnLoadingComplete(OnReturnLoadingComplete);
        }
    }

    public void SetSelectedPlayerUnit(string playerId)
    {
        _selectedPlayerUnitId = playerId;
    }

    public string GetSelectedPlayerUnitId() 
    {
        return _selectedPlayerUnitId; 
    }

    public void OnPlayerDie()
    {
        Time.timeScale = 0f;
        DaniTechUIManager.Instance.OpenPopupUI(DaniTechUIType.ReturnToMainPopup);
    }

    public void IncreasePlayerExp(int exp)
    {
        _playerModel.PlayerTotalExp += exp;
        _currentExp += exp;
        CheckLevelUp();
    }

    private float GetExpToNextLevel()
    {
        return 100 * Mathf.Pow(1.5f, _currentLevel - 1);
    }

    private void CheckLevelUp()
    {
        float expToNextLevel = GetExpToNextLevel();

        var inGameUI = DaniTechUIManager.Instance.GetCreatedUI(DaniTechUIRootType.MainUI, DaniTechUIType.InGameUI);
        if (inGameUI is InGameUI ui)
        {
            ui.SetExpBar(_currentExp, expToNextLevel, _currentLevel);
        }

        if (_currentExp >= expToNextLevel)
        {
            _currentExp -= expToNextLevel;
            _currentLevel++;
            OnLevelUp();
        }
    }

    private void OnLevelUp()
    {
        //Debug.Log($"레벨업, 현재 레벨: {_currentLevel}");
        Time.timeScale = 0f;

        List<SkillData> allSkills = new List<SkillData>(DaniTechGameDataManager.Instance.SkillsDataList.Values);
        List<SkillData> randomSkills = new List<SkillData>();

        while (randomSkills.Count < 3 && allSkills.Count > 0)
        {
            int idx = Random.Range(0, allSkills.Count);
            randomSkills.Add(allSkills[idx]);
            allSkills.RemoveAt(idx);
        }

        var inGameUI = DaniTechUIManager.Instance.GetCreatedUI(DaniTechUIRootType.MainUI, DaniTechUIType.InGameUI);
        var popup = DaniTechUIManager.Instance.OpenPopupUI(DaniTechUIType.LevelUpPopup);
        if (popup is LevelUpPopup levelUpPopup)
        {
            levelUpPopup.Init(randomSkills);
        }
    }

    public void AddPlayerSkill(string skillId)
    {
        _currentPlayer.GetComponent<PlayerUnit_Attack>().AddSkill(skillId);
    }

    public void OnChapterClear(string dialogueGroupId)
    {
        var ui = DaniTechUIManager.Instance.OpenContentUI(DaniTechUIType.DialogueUI);
        if (ui is DialogueUI dialogueUI)
        {
            dialogueUI.StartDialogue(dialogueGroupId, DialogueOpenType.ChapterClear);
        }
    }

    public void OnChapterClearSave(string chapterId)
    {
        if (_playerModel.ClearedChapterList.Contains(chapterId) == false)
        {
            _playerModel.ClearedChapterList.Add(chapterId);
        }
        SaveData();
    }

    public int GetClearedChapterCount()
    {
        return _playerModel.ClearedChapterList.Count;
    }

    public bool IsChapterCleared(string chapterId)
    {
        return _playerModel.ClearedChapterList.Contains(chapterId);
    }

    public void AddUmbra(int amount)
    {
        _playerModel.Umbra += amount;
        _currentChapterUmbra += amount;
        UpdateUmbraUI();
    }

    public int GetUmbra()
    {
        return _playerModel.Umbra;
    }

    private void UpdateUmbraUI()
    {
        var inGameUI = DaniTechUIManager.Instance.GetCreatedUI(DaniTechUIRootType.MainUI, DaniTechUIType.InGameUI);
        if (inGameUI is InGameUI ui)
        {
            ui.SetUmbra(_currentChapterUmbra);
        }
    }

    private void ClearAllItems()
    {
        ItemManager.Instance.ClearAllItems();

        HealKit[] healKits = FindObjectsByType<HealKit>(FindObjectsSortMode.None);
        foreach (var item in healKits)
        {
            if (item.gameObject.activeSelf)
            {
                ObjectPoolManager.Instance.ReturnObject("Prefabs/Item/HealKit", item.gameObject);
            }
        }

        Umbra[] umbra = FindObjectsByType<Umbra>(FindObjectsSortMode.None);
        foreach (var item in umbra)
        {
            if (item.gameObject.activeSelf)
            {
                ObjectPoolManager.Instance.ReturnObject("Prefabs/Item/Umbra", item.gameObject);
            }
        }

        Magnet[] magnet = FindObjectsByType<Magnet>(FindObjectsSortMode.None);
        foreach (var item in magnet)
        {
            if (item.gameObject.activeSelf)
            {
                ObjectPoolManager.Instance.ReturnObject("Prefabs/Item/Magnet", item.gameObject);
            }
        }
    }

    public int GetUpGradeLevel(string statType)
    {
        switch (statType)
        {
            case "Attack":
                return _playerModel.AttackUpgradeLevel;
            case "Hp":
                return _playerModel.HpUpgradeLevel;
            case "Defense":
                return _playerModel.DefenseUpgradeLevel;
            case "CoolDown":
                return _playerModel.CoolDownUpgradeLevel;
            default: return 0;
        }
    }

    public void SetUpGradeLevel(string statType, int level)
    {
        switch (statType)
        {
            case "Attack":
                _playerModel.AttackUpgradeLevel = level;
                break;
            case "Hp":
                _playerModel.HpUpgradeLevel = level;
                break;
            case "Defense":
                _playerModel.DefenseUpgradeLevel = level;
                break;
            case "CoolDown":
                _playerModel.CoolDownUpgradeLevel = level;
                break;
        }
    }

    public void SpendUmbra(int amount)
    {
        _playerModel.Umbra -= amount;
    }

    public float GetAttackBonus()
    {
        int level = _playerModel.AttackUpgradeLevel;
        if (level == 0) return 0f;

        float total = 0f;

        for (int i = 1; i <= level; i++)
        {
            UpgradeData data = DaniTechGameDataManager.Instance.GetUpgradeData($"upgrade_attack_{i}");
            if (data != null)
            {
                total += data.IncreaseAmount;
            }
        }
        return total;
    }

    public float GetDefenseBonus()
    {
        int level = _playerModel.DefenseUpgradeLevel;
        if (level == 0) return 0f;

        float total = 0f;

        for (int i = 1; i <= level; i++)
        {
            UpgradeData data = DaniTechGameDataManager.Instance.GetUpgradeData($"upgrade_defense_{i}");
            if (data != null)
            {
                total += data.IncreaseAmount;
            }
        }
        return total;
    }

    public float GetCoolDownBonus()
    {
        int level = _playerModel.CoolDownUpgradeLevel;
        if (level == 0) return 0f;

        float total = 0f;

        for (int i = 1; i <= level; i++)
        {
            UpgradeData data = DaniTechGameDataManager.Instance.GetUpgradeData($"upgrade_cooldown_{i}");
            if (data != null)
            {
                total += data.IncreaseAmount;
            }
        }
        return total;
    }

    public float GetHpBonus()
    {
        int level = _playerModel.HpUpgradeLevel;
        if (level == 0) return 0f;

        float total = 0f;

        for (int i = 1; i <= level; i++)
        {
            UpgradeData data = DaniTechGameDataManager.Instance.GetUpgradeData($"upgrade_hp_{i}");
            if (data != null)
            {
                total += data.IncreaseAmount;
            }
        }
        return total;
    }

    public void SetEndingCleared()
    {
        _playerModel.IsEndingCleared = true;
    }

    public bool GetEndingCleared()
    {
        return _playerModel.IsEndingCleared;
    }
}
