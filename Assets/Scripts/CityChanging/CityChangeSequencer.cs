using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CityChangeSequencer : MonoBehaviour
{
    GameManager gameManager;
    CityManager cityManager;
    [SerializeField] List<CityChangeSequenceTrigger> sequenceTriggers = new List<CityChangeSequenceTrigger>();
    CinemachineVirtualCamera pannedCam;

    private void Awake()
    {
        cityManager = transform.root.GetComponent<CityManager>();
    }

    private void OnEnable()
    {
        gameManager = GameManager.instance;
        cityManager.OnCityWon += StartSequence;
    }

    private void OnDisable()
    {
        cityManager.OnCityWon -= StartSequence;
    }

    private void StartSequence()
    {
        for (int i = 0; i < sequenceTriggers.Count; i++)
        {
            sequenceTriggers[i].gameObject.SetActive(true);
        }
    }

    public void PanCam()
    {
        gameManager.OnCameraPan?.Invoke(pannedCam);
    }
}
