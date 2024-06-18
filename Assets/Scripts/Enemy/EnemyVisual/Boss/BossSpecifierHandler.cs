using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BossSpecifierHandler : MonoBehaviour
{
    EnemyStats enemyStats;

    GameManager gameManager;
    [SerializeField] private CinemachineVirtualCamera pannedCam;

    [Header("VFX")]
    [SerializeField] private ParticleSystem spawnParticle;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    private void OnEnable()
    {
        gameManager = GameManager.instance;
        enemyStats.OnEnemySpawned += SpawnSequence;
    }

    private void OnDisable()
    {
        enemyStats.OnEnemySpawned -= SpawnSequence;
    }

    public void SpawnSequence()
    {
        // signifer çalıştırma

        spawnParticle.Play();
        gameManager.OnCameraPan?.Invoke(pannedCam);

        StartCoroutine(StopSpawnParticle());
    }

    IEnumerator StopSpawnParticle()
    {
        if (!gameObject.activeSelf) yield return null;

        yield return new WaitForSeconds(gameManager.bossVFXDisappearTimer);

        spawnParticle.Stop();
    }
}
