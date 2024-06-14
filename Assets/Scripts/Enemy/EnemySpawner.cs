using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    ObjectPooler objectPooler;
    GameManager gameManager;
    PlayerStats playerStats;

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public int enemyQuota; // the total number of eneies to spawn in this wave
        public float spawnGap; // Time Between Two Wave Spawns
        public int spawnCount; //the number of enemies already enemies spawned in this wave

        [HideInInspector] public int totalNumOfEnemiesInWave;

        private void Start()
        {
            totalNumOfEnemiesInWave = 0;
        }

    }

    [System.Serializable]
    public class EnemyGroup
    {
        public GameObject enemyPrefab;
        public int enemyCount; // the total number of eneies to spawn in this wave
        public int spawnedEnemyCounter; //the number of enemies already enemies spawned in this wave
    }

    CityManager cityManager;
    UIManager uiManager;


    [SerializeField] private List<Wave> waves; // a list of all the waves in the game
    [SerializeField] private int currentWaveCount; //the index of current Wave [Remember, a list starts from  0]

    [Header("Spawn Atrributes")]
    float spawnTimer; // Timer use to spawn next enemy
    [SerializeField] private int enemiesAlive;
    [SerializeField] private float waveGap; // the interval between each wave

    [Header("Spawn Positions")]
    [SerializeField] private List<Transform> enemySpawnPoints; //a list to store all the spawn points of enemies


    [Header("Private Variables")]
    int spawnIndex;
    bool canBeginNextWave = false;
    int totalNumOfEnemiesOfSpawner;
    int killedEnemies;

    [Header("Total Health")]
    [HideInInspector] public float totalHealthValue;

    [Header("Events")]
    public Action OnWaveCompleted;
    public Action<float> OnWaveProgressed;
    public Action<int> OnEnemySpawned;

    private void OnEnable()
    {
        objectPooler = ObjectPooler.instance;
        gameManager = GameManager.instance;
        playerStats = PlayerStats.instance;

        uiManager = UIManager.instance;
        OnWaveProgressed += uiManager.UpdateInWaveProgressBarValue;

        cityManager = transform.root.GetComponent<CityManager>();
    }

    private void OnDisable()
    {
        OnWaveProgressed -= uiManager.UpdateInWaveProgressBarValue;
    }

    private void Start()
    {
        totalNumOfEnemiesOfSpawner = 0;
        killedEnemies = 0;

        for (int i = 0; i < waves.Count; i++)
        {
            for (int j = 0; j < waves[i].enemyGroups.Count; j++)
            {
                totalNumOfEnemiesOfSpawner += waves[i].enemyGroups[j].enemyCount;
            }
        }

        OnWaveProgressed?.Invoke(0f);

        CalculateEnemyQuota();
        spawnIndex = 0;
        OnAssignEnemySpawnPoints();
    }

    private void Update()
    {

        if (!gameManager.canSpawnEnemy) return;

        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == waves[currentWaveCount].enemyQuota) // check if the wave end and is there more?
        {
            StartCoroutine(BeginNextWave());
            canBeginNextWave = true;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= waves[currentWaveCount].spawnGap)
        {
            spawnTimer = 0;
            SpawnEnemies();
        }

    }

    IEnumerator BeginNextWave()
    {
        //wave will spawn after the wavegap ends
        yield return new WaitForSeconds(waveGap);

        //move on to the next wave
        if (currentWaveCount < waves.Count - 1 && canBeginNextWave)
        {
            currentWaveCount++;
            canBeginNextWave = false;
            CalculateEnemyQuota();
        }

    }
    void CalculateEnemyQuota()
    {
        int currentEnemyQuota = 0;

        foreach (var EnemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentEnemyQuota += EnemyGroup.enemyCount;
        }

        waves[currentWaveCount].enemyQuota = currentEnemyQuota;
    }

    //<<SUMMARY>>
    // this method will stop spawning enemies if amount of enemiesalive is max. 
    //<<SUMMARY>>
    void SpawnEnemies()
    {
        //check if quota is full
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].enemyQuota)
        {
            int localEnemySpawnCounter = 0;

            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                if (enemyGroup.spawnedEnemyCounter < enemyGroup.enemyCount)
                {
                    if (enemyGroup.spawnedEnemyCounter < enemyGroup.enemyCount)
                    {
                        localEnemySpawnCounter++;
                    }
                }

            }

            // spawn each type of enemy until the quota is filled
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {

                //check if the minimum number of enemies of this type have been spawned
                if (enemyGroup.spawnedEnemyCounter < enemyGroup.enemyCount)
                {

                    NavMesh.SamplePosition(enemySpawnPoints[spawnIndex].position, out NavMeshHit hit, 3, 63);


                    EnemyDeathHandler spawnedEnemy = objectPooler.SpawnEnemyFromPool(enemyGroup.enemyPrefab.name,
                        hit.position, cityManager.transform);

                    OnEnemySpawned?.Invoke(localEnemySpawnCounter);

                    spawnIndex++;

                    if (spawnIndex >= enemySpawnPoints.Count)
                    {
                        spawnIndex = 0;
                    }

                    gameManager.allSpawnedEnemies.Add(spawnedEnemy);

                    enemyGroup.spawnedEnemyCounter++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;

                }
            }


        }

    }

    //Call when the enemy dies
    public void OnEnemyKilled()
    {
        //decrement the numberof enemiesalive on death
        enemiesAlive--;

        killedEnemies++;

        OnWaveProgressed?.Invoke((float)killedEnemies / (float)totalNumOfEnemiesOfSpawner);

        if (killedEnemies >= totalNumOfEnemiesOfSpawner)
        {
            OnWaveCompleted?.Invoke();
        }
    }

    private void OnAssignEnemySpawnPoints()
    {
        List<Transform> tempList = new List<Transform>(gameManager.allCities[playerStats.GetCityIndex()].GetEnemySpawnPoses());

        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].gameObject.activeSelf)
            {
                enemySpawnPoints.Add(tempList[i]);
            }
        }

        tempList.Clear();
    }


    // Total health calculator

    /* public void CalculateTotalHealth()
     {
         for (int i = 0; i < Waves.Count; i++)
         {
             for (int a = 0; a < Waves[i].EnemyGroups.Count; a++)
             {
                 for (int j = 0; j < Waves[i].EnemyGroups[a].EnemyCount; j++)
                 {
                     totalHealthValue += Waves[i].EnemyGroups[a].EnemyPrefab.GetComponent<EnemyStats>().EnemySO.GetMaxHealth();
                 }

             }
         }
     } */

}
