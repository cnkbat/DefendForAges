using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAngleCalculator : MonoBehaviour
{
    GameManager gameManager;

    PlayerDeathHandler playerDeathHandler;
    private Joystick joystick;
    private NearestEnemyFinder nearestEnemyFinder;
    public Action<float, float> OnPlayerMoved;

    private void Awake()
    {
        joystick = FindObjectOfType<Joystick>(true);
        nearestEnemyFinder = GetComponent<NearestEnemyFinder>();
        playerDeathHandler = GetComponent<PlayerDeathHandler>();
    }
    private void OnEnable()
    {
        gameManager = GameManager.instance;
    }

    private void Update()
    {
        if (gameManager.isGameFreezed) return;
        if (gameManager.isPlayerFreezed) return;
        if (playerDeathHandler.GetIsDead()) return;

        Vector2 vectors = CalculateThePlayerRotationAngle(joystick.result);

        OnPlayerMoved?.Invoke(vectors.x, vectors.y);
    }

    private Vector2 CalculateThePlayerRotationAngle(Vector2 input)
    {
        float currentPlayerAngle = transform.localEulerAngles.y;

        float inputAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;

        float diff = inputAngle - currentPlayerAngle;
        Vector2 returnValue;

        if (joystick.hasJoystickInput)
        {
            returnValue = new Vector2(Mathf.Sin(diff * Mathf.Deg2Rad), Mathf.Cos(diff * Mathf.Deg2Rad));
        }
        else
        {
            returnValue = new Vector2(0, 0);
        }

        return returnValue;
    }
}
