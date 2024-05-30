using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TutorialSequencer : MonoBehaviour
{
    GameManager gameManager;
    private CinemachineVirtualCamera pannedCam;
    private void Awake()
    {
        pannedCam = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    void Start()
    {
        gameManager = GameManager.instance;
        PlayTutorialSequence();
    }


    public void PlayTutorialSequence()
    {
        gameManager.OnCameraPan?.Invoke(pannedCam);
    }

}
