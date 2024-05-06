using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private PlayerStats playerStats;
    [SerializeField] private Joystick joystick;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float speed = 5f;

    private Vector3 direction;

    private float horizontal;
    private float vertical;
    private float targetAngle = 0;

    private void OnEnable()
    {
        playerStats = GetComponent<PlayerStats>();
        playerStats.OnMovementChanged += UpdateMovementSpeed;
    }

    private void OnDisable()
    {
        playerStats.OnMovementChanged -= UpdateMovementSpeed;
    }
    private void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        UpdateMovementSpeed();
    }

    private void FixedUpdate()
    {
        ToInputAxis();

    }
    private void Update()
    {
        ToMove();
    }

    void ToInputAxis()
    {
        horizontal = joystick.result.x;
        vertical = joystick.result.y;
        direction = new Vector3(horizontal, 0, vertical).normalized;
    }

    void ToMove()
    {
        if (Input.GetMouseButton(0))
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            transform.position = new Vector3(transform.position.x + horizontal * speed * Time.deltaTime, transform.position.y, transform.position.z + vertical * speed * Time.deltaTime);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, targetAngle, 0f), rotationSpeed * Time.deltaTime);
        }
    }


    private void UpdateMovementSpeed()
    {
        speed = playerStats.GetMovementSpeed();
    }
}


