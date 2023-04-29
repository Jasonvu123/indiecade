using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SpawnModes
{
    Fixed,
    Random
}

public class Spawner : MonoBehaviour
{
    public static Action OnWaveCompleted;
    
    [Header("Settings")]
    [SerializeField] private SpawnModes spawnMode = SpawnModes.Fixed;
    [SerializeField] public int enemyCount = 10;
    [SerializeField] private float delayBtwWaves = 1f;

    [Header("Fixed Delay")]
    [SerializeField] private float delayBtwSpawns;
    
    [Header("Random Delay")]
    [SerializeField] private float minRandomDelay;
    [SerializeField] private float maxRandomDelay;

    [Header("Poolers")] 
    [SerializeField] private ObjectPooler enemyWave1Pooler;
    [SerializeField] private ObjectPooler enemyWave2Pooler;
    [SerializeField] private ObjectPooler enemyWave3Pooler;
    [SerializeField] private ObjectPooler enemyWave4Pooler;
    [SerializeField] private ObjectPooler enemyWave5Pooler;
    [SerializeField] private ObjectPooler enemyWave6Pooler;
    [SerializeField] private ObjectPooler enemyWave7Pooler;
    [SerializeField] private ObjectPooler enemyWave8Pooler;
    [SerializeField] private ObjectPooler enemyWave9Pooler;
    [SerializeField] private ObjectPooler enemyWave10Pooler;

    [SerializeField] private GameObject AugmentPanel;


    private float _spawnTimer;
    public int _enemiesSpawned;
    public int _enemiesRamaining;
    
    [SerializeField] private Waypoint _waypoint;
    [SerializeField] private Waypoint _waypoint2;

    private void Start()
    {
     //   _waypoint = GetComponent<Waypoint>();
      //  _waypoint2 = GetComponent<Waypoint>();

        _enemiesRamaining = enemyCount;
    }

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer < 0)
        {
            _spawnTimer = GetSpawnDelay();
            if (_enemiesSpawned < enemyCount)
            {
                _enemiesSpawned++;
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        int currentWave = LevelManager.Instance.CurrentWave;
        GameObject newInstance = GetPooler().GetInstanceFromPool();

        Enemy enemy = newInstance.GetComponent<Enemy>();
        if(currentWave%2 == 0)
        {
               enemy.Waypoint = _waypoint2;
        }
        else
        {
            enemy.Waypoint = _waypoint;
        }
        //enemy.Waypoint = _waypoint;
        enemy.ResetEnemy();

        enemy.transform.localPosition = transform.position;
        newInstance.SetActive(true);
    }

    private float GetSpawnDelay()
    {
        float delay = 0f;
        if (spawnMode == SpawnModes.Fixed)
        {
            delay = delayBtwSpawns;
        }
        else
        {
            delay = GetRandomDelay();
        }

        return delay;
    }
    
    private float GetRandomDelay()
    {
        float randomTimer = Random.Range(minRandomDelay, maxRandomDelay);
        return randomTimer;
    }

    private ObjectPooler GetPooler()
    {
        int currentWave = LevelManager.Instance.CurrentWave;
        if (currentWave <= 1) // 1- 10
        {
            return enemyWave1Pooler;
        }

        if (currentWave > 1 && currentWave <= 2) // 11- 20
        {
            return enemyWave2Pooler;
        }
        
        if (currentWave > 2 && currentWave <= 3) // 21- 30
        {
            return enemyWave3Pooler;
        }
        
        if (currentWave > 3 && currentWave <= 4) // 21- 30
        {
            return enemyWave4Pooler;
        }
        
        if (currentWave > 4 && currentWave <= 5) // 21- 30
        {
            return enemyWave5Pooler;
        }
        if (currentWave > 5 && currentWave <= 6) // 21- 30
        {
            return enemyWave6Pooler;
        }
        if (currentWave > 6 && currentWave <= 7) // 21- 30
        {
            return enemyWave7Pooler;
        }
         if (currentWave > 7 && currentWave <= 8) // 21- 30
        {
            return enemyWave8Pooler;
        }
        if (currentWave > 8 && currentWave <= 9) // 21- 30
        {
            return enemyWave9Pooler;
        }                              
        if (currentWave > 9 && currentWave <= 10) // 21- 30
        {
            enemyCount = 1;
            return enemyWave10Pooler;
        }
        if (currentWave > 10 && currentWave <= 11) // 21- 30
        {
            AugmentPanel.SetActive(true);
            return null;
        }

        return null;
    }
    
    private IEnumerator NextWave()
    {
        yield return new WaitForSeconds(delayBtwWaves);
        _enemiesRamaining = enemyCount;
        _spawnTimer = 0f;
        _enemiesSpawned = 0;
    }
    
    private void RecordEnemy(Enemy enemy)
    {
        
            int currentWave = LevelManager.Instance.CurrentWave;
        _enemiesRamaining--;
        /*
        if(currentWave == 5)
        {
                    if (_enemiesRamaining < 3)
        {
            OnWaveCompleted?.Invoke();
            StartCoroutine(NextWave());
        }
        }
        if (_enemiesRamaining <= 0)
        {
            OnWaveCompleted?.Invoke();
            StartCoroutine(NextWave());
        }
        */
        /*
        if (_enemiesSpawned == enemyCount)
        {
            OnWaveCompleted?.Invoke();
            StartCoroutine(NextWave());
        }
        
*/
        if(currentWave == 8)
        {
            if (_enemiesRamaining <= 0)
            {
                OnWaveCompleted?.Invoke();
                StartCoroutine(NextWave());
            }
        }

        else if (_enemiesSpawned == enemyCount)
        {
            OnWaveCompleted?.Invoke();
            StartCoroutine(NextWave());
        }
    }
    
    private void OnEnable()
    {
        Enemy.OnEndReached += RecordEnemy;
        EnemyHealth.OnEnemyKilled += RecordEnemy;
    }

    private void OnDisable()
    {
        Enemy.OnEndReached -= RecordEnemy;
        EnemyHealth.OnEnemyKilled -= RecordEnemy;
        
    }
}
