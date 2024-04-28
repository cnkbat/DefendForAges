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
        public string WaveName;
        public List<EnemyGroup> EnemyGroups;
        public int EnemyQuota; // the total number of eneies to spawn in this wave
        public float SpawnGap; // Time Between Two Wave Spawns
        public int SpawnCount; //the number of enemies already enemies spawned in this wave
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string EnemyName;
        public int EnemyCount; // the total number of eneies to spawn in this wave
        public GameObject EnemyPrefab;
        public int spawnedEnemyCounter; //the number of enemies already enemies spawned in this wave
    }

    public List<Wave> Waves; // a list of all the waves in the game
    public int CurrentWaveCount; //the index of current Wave [Remember, a list starts from  0]

    [Header("Spawn Atrributes")]
    float SpawnTimer; // Timer use to spawn next enemy
    public int EnemiesAlive;
    public int MaxEnemiesAllowed; // maxnumber of allowed enemies to enhance gameplay
    public bool MaxEnemiesReached = false;   // a flag indicating if the maximum number of enemies has been reached
    public float WaveGap; // the interval between each wave

    [Header("Spawn Positions")]
    public List<Transform> enemySpawnPoints; //a list to store all the spawn points of enemies
    int spawnIndex;
    bool CanBeginNextWave = false;
    Transform playerTransfrom;

    [Header("Total Health")]
    public float totalHealthValue;

    private void Start()
    {
        
        objectPooler = ObjectPooler.instance;
        gameManager = GameManager.instance;


        AssignSpawnTransforms();
        CalculateEnemyQuota();
        CalculateTotalHealth();
        spawnIndex = 0;

    }



    private void Update()
    {
        //if (!gameManager.canSpawnEnemy) return;

        if (CurrentWaveCount < Waves.Count && Waves[CurrentWaveCount].SpawnCount == Waves[CurrentWaveCount].EnemyQuota) // check if the wave end and is there more?
        {
            StartCoroutine(BeginNextWave());
            CanBeginNextWave = true;
        }
        SpawnTimer += Time.deltaTime;

        if (SpawnTimer >= Waves[CurrentWaveCount].SpawnGap)
        {
            SpawnTimer = 0;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        //wave will spawn after the wavegap ends
        yield return new WaitForSeconds(WaveGap);

        //move on to the next wave
        if (CurrentWaveCount < Waves.Count - 1 && CanBeginNextWave)
        {
            CurrentWaveCount++;
            CanBeginNextWave = false;
            CalculateEnemyQuota();
        }

    }
    void CalculateEnemyQuota()
    {
        int CurrentEnemyQuota = 0;

        foreach (var EnemyGroup in Waves[CurrentWaveCount].EnemyGroups)
        {
            CurrentEnemyQuota += EnemyGroup.EnemyCount;
        }

        Waves[CurrentWaveCount].EnemyQuota = CurrentEnemyQuota;
    }

    //<<SUMMARY>>
    // this method will stop spawning enemies if amount of enemiesalive is max. 
    //<<SUMMARY>>
    void SpawnEnemies()
    {
        //check if quota is full
        if (Waves[CurrentWaveCount].SpawnCount < Waves[CurrentWaveCount].EnemyQuota && !MaxEnemiesReached)
        {
            // spawn each type of enemy until the quota is filled
            foreach (var EnemyGroup in Waves[CurrentWaveCount].EnemyGroups)
            {

                //check if the minimum number of enemies of this type have been spawned
                if (EnemyGroup.spawnedEnemyCounter < EnemyGroup.EnemyCount)
                {

                    // checking for the maxenemies for not breaking the game
                    if (EnemiesAlive >= MaxEnemiesAllowed)
                    {
                        MaxEnemiesReached = true;
                        return;
                    }


                    GameObject spawnedEnemy = objectPooler.SpawnFromPool(EnemyGroup.EnemyName,
                        enemySpawnPoints[spawnIndex].position);


                    spawnIndex++;

                    if (spawnIndex >= enemySpawnPoints.Count)
                    {
                        spawnIndex = 0;
                    }

                    gameManager.allSpawnedEnemies.Add(spawnedEnemy);

                    EnemyGroup.spawnedEnemyCounter++;
                    Waves[CurrentWaveCount].SpawnCount++;
                    EnemiesAlive++;
                }
            }
        }

        //reseting the maxenemiesreached to continue spawning enemies
        if (EnemiesAlive < MaxEnemiesAllowed)
        {
            MaxEnemiesReached = false;
        }

    }
    private void AssignSpawnTransforms()
    {
        // komple değişecek
        enemySpawnPoints.Add(GameObject.Find("EnemySpawnTransform1").transform);
        enemySpawnPoints.Add(GameObject.Find("EnemySpawnTransform2").transform);
        enemySpawnPoints.Add(GameObject.Find("EnemySpawnTransform3").transform);
        enemySpawnPoints.Add(GameObject.Find("EnemySpawnTransform4").transform);
        enemySpawnPoints.Add(GameObject.Find("EnemySpawnTransform5").transform);
        enemySpawnPoints.Add(GameObject.Find("EnemySpawnTransform6").transform);

    }
    //Call when the enemy dies
    public void OnEnemyKilled()
    {
        //decrement the numberof enemiesalive on death
        EnemiesAlive--;
    }


    public void CalculateTotalHealth()
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
    }
}
