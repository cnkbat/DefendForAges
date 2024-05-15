using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAngleCalculator : MonoBehaviour
{
    public static PlayerAngleCalculator instance;

    [SerializeField] private Joystick joystick;
    [SerializeField] private Animator animator;
    private void Awake()
    {
        instance = this;
        joystick = FindObjectOfType<Joystick>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Vector2 vectors = CalculateThePlayerRotationAngle(joystick.result);
        animator.SetFloat("Vx", vectors.x);
        animator.SetFloat("Vy", vectors.y);
    }

    private Vector2 CalculateThePlayerRotationAngle(Vector2 input)
    {
        float currentPlayerAngle = transform.eulerAngles.y;

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

        print(String.Format("Diff : {0}, Return : {1}, Input : {2}", diff, returnValue, input));
        
        return returnValue;
    }
}
