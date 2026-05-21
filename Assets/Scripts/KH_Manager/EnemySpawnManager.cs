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

    [SerializeField] private float _spawnRadius = 10f;

    private void Awake()
    {
        Instance = this;
    }

    public void Init(string chapterId, Transform playerTransform)
    {
        _playerTransform = playerTransform;
        _currentWaveIndex = 0;
        _waveList = DaniTechGameDataManager.Instance.GetChapterWaveList(chapterId);
        StartNextWave();
    }

    private void StartNextWave()
    {
        if (_currentWaveIndex >= _waveList.Count)
        {
            Debug.Log("모든 웨이브 클리어");
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

        await UniTask.Delay(TimeSpan.FromSeconds(30f));
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
}
