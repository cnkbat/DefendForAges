using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAngleCalculator : MonoBehaviour
{
    public static PlayerAngleCalculator instance;

    [SerializeField] private Joystick joystick;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        CalculateThePlayerRotationAngle(joystick.result);
    }

    private Vector2 CalculateThePlayerRotationAngle(Vector2 input)
    {
        float currentPlayerAngle = transform.eulerAngles.y;

        float inputAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;

        float diff = inputAngle - currentPlayerAngle;

        Vector2 returnValue = new Vector2(Mathf.Sin(diff * Mathf.Deg2Rad), Mathf.Cos(diff * Mathf.Deg2Rad));

        print(String.Format("Diff : {0}, Return : {1}, Input : {2}", diff, returnValue, input));
        
        return returnValue;
    }
}
