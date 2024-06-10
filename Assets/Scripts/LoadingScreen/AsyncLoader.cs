using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoader : MonoBehaviour
{
    public static AsyncLoader instance { get; private set; }
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(int levelToLoad)
    {
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelAsync(levelToLoad));
    }


    IEnumerator LoadLevelAsync(int levelToLoad)
    {
        DOTween.Clear();
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        if (loadingSlider)
        {
            while (!loadOperation.isDone)
            {
                float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);

                loadingSlider.value = progressValue;
                yield return null;
            }
        }
    }

    public void LoadScene(string levelName)
    {
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelAsync(levelName));
    }

    IEnumerator LoadLevelAsync(string levelName)
    {
        DOTween.Clear();
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelName);

        if (loadingSlider)
        {
            while (!loadOperation.isDone)
            {
                float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);

                loadingSlider.value = progressValue;
                yield return null;
            }
        }
    }
}
