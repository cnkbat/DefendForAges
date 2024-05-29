using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BossSpecifierHandler : MonoBehaviour
{
    EnemyBehaviour enemyBehaviour;

    GameManager gameManager;
    [SerializeField] private CinemachineVirtualCamera pannedCam;

    [Header("VFX")]
    [SerializeField] private ParticleSystem spawnParticle;

    private void Awake()
    {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    private void OnEnable()
    {
        gameManager = GameManager.instance;
        enemyBehaviour.OnEnemySpawned += SpawnSequence;
    }

    private void OnDisable()
    {
        enemyBehaviour.OnEnemySpawned -= SpawnSequence;
    }

    public void SpawnSequence()
    {
        // kamera muhabbeti
        // signifer çalıştırma
        
        spawnParticle.Play();   
        gameManager.OnCameraPan?.Invoke(pannedCam);

        StartCoroutine(StopSpawnParticle());
    }

    IEnumerator StopSpawnParticle()
    {
        if(!gameObject.activeSelf) yield return null;

        yield return new WaitForSeconds(gameManager.bossVFXDisappearTimer);

        spawnParticle.Stop();
    }
}
