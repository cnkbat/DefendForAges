using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpecifierHandler : MonoBehaviour
{
    EnemyBehaviour enemyBehaviour;
    EnemyStats enemyStats;
    GameManager gameManager;

    [Header("VFX")]
    [SerializeField] private ParticleSystem spawnParticle;

    private void Awake()
    {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        enemyStats = GetComponent<EnemyStats>();
    }

    private void OnEnable()
    {
        gameManager = GameManager.instance;
        enemyBehaviour.OnEnemySpawned += SpawnSequence;
    }

    private void OnDisable()
    {
        enemyBehaviour.OnEnemySpawned += SpawnSequence;
    }

    public void SpawnSequence()
    {
        // kamera muhabbeti
        // signifer çalıştırma
        
        spawnParticle.Play();

        StartCoroutine(StopSpawnParticle());
    }

    IEnumerator StopSpawnParticle()
    {
        yield return new WaitForSeconds(gameManager.bossVFXDisappearTimer);

        spawnParticle.Stop();
    }
}
