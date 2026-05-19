using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class DaniTechGameManager : MonoBehaviour
{
    public static DaniTechGameManager Inst { get; set; }

    // 플레이 중에 저장되어야 하는 정보들이 있는 위치
    private DaniTechPlayerModel _playerModel = new DaniTechPlayerModel();
    private GameObject _currentMap;
    private GameObject _currentPlayer;

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
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

    public void IncreasePlayerExp(int exp)
    {
        // 추후에 한곳에서 관리할 수 있게 익스텐션으로 빼도 된다
        _playerModel.PlayerTotalExp += exp;
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

    public void StartChapter(int chapterIdx)
    {
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

        PlayerUnitData playerdata = DaniTechGameDataManager.Instance.GetPlayerUnitData("Player_Unit_FeMale_01");
        if (playerdata == null)
        {
            Debug.LogError("플레이어 데이터 없음");
            return;
        }

        GameObject playerPrefab = Resources.Load<GameObject>(playerdata.PrefabPath);
        if (playerPrefab == null) 
        {
            Debug.LogError("플레이어 프리팹을 찾을 수 없습니다.");
            return;
        }

        _currentPlayer = Instantiate(playerPrefab);
        Camera.main.GetComponent<Camera_Tracking>().SetTarget(_currentPlayer.transform);

        DaniTechUIManager.Instance.CloseUI(DaniTechUIRootType.MainUI, DaniTechUIType.MainUI);
        DaniTechUIManager.Instance.ClosePopupUI(DaniTechUIType.ChapterUI);
        DaniTechUIManager.Instance.OpenInGameUI();
    }

    public void ReturnToMainUI()
    {
        if (_currentMap != null) Destroy(_currentMap);
        if (_currentPlayer != null) Destroy(_currentPlayer);

        DaniTechUIManager.Instance.ClosePopupUI(DaniTechUIType.PausePopup);
        DaniTechUIManager.Instance.CloseUI(DaniTechUIRootType.MainUI, DaniTechUIType.InGameUI);
        DaniTechUIManager.Instance.OpenUI(DaniTechUIRootType.MainUI, DaniTechUIType.MainUI);
    }
}
