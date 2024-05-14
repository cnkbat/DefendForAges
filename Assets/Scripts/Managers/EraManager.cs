using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EraManager : MonoBehaviour
{


    GameManager gameManager;
    SaveManager saveManager;

    [Header("Scenes")]
    [SerializeField] List<String> allSceneNames;

    [Header("Loading Screen")]
    public AsyncLoader asyncLoader;

    [Header("Saved Indexes")]
    public int currentTimelineIndex;
    public int eraIndex;

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;

        saveManager = SaveManager.instance;
        gameManager = GameManager.instance;

        gameManager.OnEraChanged += IncrementEraIndex;

    }

    private void Start()
    {
        asyncLoader = FindObjectOfType<AsyncLoader>();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            LoadCurrentEra();
        }
    }

    #region  Era Related
    public void IncrementEraIndex()
    {
        eraIndex++;
        saveManager.OnSaved?.Invoke();

        asyncLoader.LoadScene(allSceneNames[eraIndex]);
    }

    private void LoadCurrentEra()
    {
        asyncLoader.LoadScene(allSceneNames[eraIndex]);
    }

    #endregion
}
