using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance { get; set; }

    private Transform _playerTransform;
    private List<WaveData> _waveList;
    private int _currentWaveIndex =0;
    private bool _isSpawning;

    private ChapterClearType _clearType;
    private bool _isAllWaveSpawned = false;
    private bool _isChapterCleared = false;

    [SerializeField] private float _spawnRadius = 10f;

    private ChapterUI _chapterUI;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (_clearType != ChapterClearType.AllKill) return;
        if (_isAllWaveSpawned == false) return;
        if (_isChapterCleared) return;
        //Debug.Log("클리어 체크 중");

        EnemyUnit_Base[] enemies = FindObjectsByType<EnemyUnit_Base>(FindObjectsSortMode.None);
        bool hasAliveEnemy = false;
        foreach (var enemy in enemies)
        {
            if (enemy.gameObject.activeSelf)
            {
                hasAliveEnemy = true;
                break;
            }
        }
        if (hasAliveEnemy == false)
        {
            _isChapterCleared = true;
            OnChapterClear().Forget();
        }
    }

    public void Init(string chapterId, Transform playerTransform)
    {
        _playerTransform = playerTransform;
        _currentWaveIndex = 0;
        _isAllWaveSpawned = false;
        _isChapterCleared = false;
        _waveList = DaniTechGameDataManager.Instance.GetChapterWaveList(chapterId);
        _chapterUI = FindAnyObjectByType<ChapterUI>();

        if (_chapterUI != null)
        {
            _clearType = _chapterUI.ClearType;
        }

        StartNextWave();
    }

    private void StartNextWave()
    {
        if (_currentWaveIndex >= _waveList.Count)
        {
            Debug.Log("모든 웨이브 클리어");
            _isAllWaveSpawned = true;
            return;
        }

        WaveData waveData = _waveList[_currentWaveIndex];

        var inGameUI = DaniTechUIManager.Instance.GetCreatedUI(DaniTechUIRootType.MainUI, DaniTechUIType.InGameUI);
        if (inGameUI is InGameUI ui)
        {
            ui.SetWaveNum(waveData.WaveNumber);
        }

        SpawnWave(waveData).Forget();
        _currentWaveIndex++;
    }

    private async UniTaskVoid SpawnWave(WaveData waveData)
    {
        _isSpawning = true;

        string[] enemyIds = waveData.EnemyIdList.Split(',');

        for (int i = 0; i < waveData.SpawnCount; i++)
        {
            if (_playerTransform == null) return;
            string enemyId = enemyIds[UnityEngine.Random.Range(0, enemyIds.Length)];
            SpawnEnemy(enemyId, GetRandomSpawnPos());
            await UniTask.Delay(TimeSpan.FromSeconds(waveData.SpawnInterval));
        }

        _isSpawning = false;

        if (_playerTransform == null) return;
        
        //Debug.Log($"현재 웨이브 인덱스: {_currentWaveIndex}, 전체 웨이브 수: {_waveList.Count}");

        if (_currentWaveIndex >= _waveList.Count)
        {
            //Debug.Log("_isAllWaveSpawned = true 설정됨");
            _isAllWaveSpawned = true;
            return;
        }

        await UniTask.Delay(TimeSpan.FromSeconds(10f));
        if (_playerTransform == null) return;
        StartNextWave();
    }

    private void SpawnEnemy(string enemyDataId, Vector2 spawnPos)
    {
        EnemyUnitData data = DaniTechGameDataManager.Instance.GetEnemyUnitData(enemyDataId);
        if (data == null)
        {
            Debug.LogError($"적 데이터 없음: {enemyDataId}");
            return;
        }

        GameObject enemyObj = ObjectPoolManager.Instance.GetObject(data.PrefabPath);
        if (enemyObj == null) 
        {
            return;
        } 

        enemyObj.transform.position = spawnPos;
        enemyObj.GetComponent<EnemyUnit_Base>().Init(enemyDataId);
        enemyObj.GetComponent<EnemyUnit_Move>().Init(data.MoveSpeed, data.AttackCoolDown);
        enemyObj.GetComponent<EnemyUnit_Move>().SetTarget(_playerTransform);

        EnemyUnit_Projectile projectileAttack = enemyObj.GetComponent<EnemyUnit_Projectile>();
        if (projectileAttack != null)
        {
            projectileAttack.Init(data, _playerTransform);
        }

        EnemyUnit_PointAttack pointAttack = enemyObj.GetComponent<EnemyUnit_PointAttack>();
        if (pointAttack != null)
        {
            pointAttack.Init(data, _playerTransform);
        }
    }

    private Vector2 GetRandomSpawnPos()
    {
        Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
        return (Vector2)_playerTransform.position + randomDir * _spawnRadius;
    }

    public void ClearAllEnemies()
    {
        EnemyUnit_Base[] enemies = FindObjectsByType<EnemyUnit_Base>(FindObjectsSortMode.None);
        foreach (var enemy in enemies) 
        {
            EnemyUnitData data = DaniTechGameDataManager.Instance.GetEnemyUnitData(enemy.GetEnemyDataId());
            if (data != null)
            {
                ObjectPoolManager.Instance.ReturnObject(data.PrefabPath, enemy.gameObject);
            }
        }
    }

    private async UniTaskVoid OnChapterClear()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(3f));

        if (_chapterUI != null && string.IsNullOrEmpty(_chapterUI.ClearDialogueGroupId) == false)
        {
            DaniTechGameManager.Inst.OnChapterClear(_chapterUI.ClearDialogueGroupId);
        }
        else
        {
            DaniTechGameManager.Inst.ReturnToMainUI();
        }
    }
}
