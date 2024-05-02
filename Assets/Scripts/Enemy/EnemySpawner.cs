using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    ObjectPooler objectPooler;
    GameManager gameManager;

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
            Debug.Log("enemy group start");
            totalNumOfEnemiesInWave = 0;
        }

    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount; // the total number of eneies to spawn in this wave
        public int spawnedEnemyCounter; //the number of enemies already enemies spawned in this wave
    }

    [SerializeField] private List<Wave> waves; // a list of all the waves in the game
    [SerializeField] private int currentWaveCount; //the index of current Wave [Remember, a list starts from  0]

    [Header("Spawn Atrributes")]
    float spawnTimer; // Timer use to spawn next enemy
    [SerializeField] private int enemiesAlive;
    [SerializeField] private int maxEnemiesAllowed; // maxnumber of allowed enemies to enhance gameplay
    [SerializeField] private bool maxEnemiesReached = false;   // a flag indicating if the maximum number of enemies has been reached
    [SerializeField] private float waveGap; // the interval between each wave

    [Header("Spawn Positions")]
    [SerializeField] private List<Transform> enemySpawnPoints; //a list to store all the spawn points of enemies
    [SerializeField] private CityManager currentCity;

    [Header("Private Variables")]
    int spawnIndex;
    bool CanBeginNextWave = false;
    int totalNumOfEnemiesOfSpawner;
    int killedEnemies;

    [Header("Total Health")]
    [HideInInspector] public float totalHealthValue;

    [Header("Events")]
    public Action OnWaveCompleted;

    private void Start()
    {
        totalNumOfEnemiesOfSpawner = 0;
        killedEnemies = 0;

        objectPooler = ObjectPooler.instance;
        gameManager = GameManager.instance;

        for (int i = 0; i < waves.Count; i++)
        {
            for (int j = 0; j < waves[i].enemyGroups.Count; j++)
            {
                totalNumOfEnemiesOfSpawner += waves[i].enemyGroups[j].enemyCount;
            }
        }

        currentCity.OnEnemySpawnPosesUpdated += OnAssignEnemySpawnPoints;

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
            CanBeginNextWave = true;
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
        if (currentWaveCount < waves.Count - 1 && CanBeginNextWave)
        {
            currentWaveCount++;
            CanBeginNextWave = false;
            CalculateEnemyQuota();
        }

    }
    void CalculateEnemyQuota()
    {
        int CurrentEnemyQuota = 0;

        foreach (var EnemyGroup in waves[currentWaveCount].enemyGroups)
        {
            CurrentEnemyQuota += EnemyGroup.enemyCount;
        }

        waves[currentWaveCount].enemyQuota = CurrentEnemyQuota;
    }

    //<<SUMMARY>>
    // this method will stop spawning enemies if amount of enemiesalive is max. 
    //<<SUMMARY>>
    void SpawnEnemies()
    {
        //check if quota is full
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].enemyQuota && !maxEnemiesReached)
        {
            // spawn each type of enemy until the quota is filled
            foreach (var EnemyGroup in waves[currentWaveCount].enemyGroups)
            {

                //check if the minimum number of enemies of this type have been spawned
                if (EnemyGroup.spawnedEnemyCounter < EnemyGroup.enemyCount)
                {

                    // checking for the maxenemies for not breaking the game
                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }


                    GameObject spawnedEnemy = objectPooler.SpawnFromPool(EnemyGroup.enemyName,
                        enemySpawnPoints[spawnIndex].position);

                    spawnedEnemy.GetComponent<EnemyTargeter>().SetCityManager(currentCity);
                    spawnedEnemy.GetComponent<EnemyBehaviour>().OnEnemyKilled += OnEnemyKilled;
                    spawnIndex++;

                    if (spawnIndex >= enemySpawnPoints.Count)
                    {
                        spawnIndex = 0;
                    }

                    gameManager.allSpawnedEnemies.Add(spawnedEnemy);

                    EnemyGroup.spawnedEnemyCounter++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                }
            }
        }

        //reseting the maxenemiesreached to continue spawning enemies
        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }

    }

    //Call when the enemy dies
    public void OnEnemyKilled()
    {
        //decrement the numberof enemiesalive on death
        enemiesAlive--;

        killedEnemies++;

        if (killedEnemies >= totalNumOfEnemiesOfSpawner)
        {
            OnWaveCompleted?.Invoke();
        }
    }

    private void OnAssignEnemySpawnPoints()
    {
        enemySpawnPoints = currentCity.GetEnemySpawnPoses();
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
