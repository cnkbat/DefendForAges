using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    NearestEnemyFinder nearestEnemyFinder;


    private PlayerStats playerStats;
    private Rigidbody rb;
    [SerializeField] private Joystick joystick;

    [Header("Walk")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float speed = 5f;



    private Vector3 direction;
    private float horizontal;
    private float vertical;
    private float targetAngle = 0;


    private void OnEnable()
    {
        playerStats = GetComponent<PlayerStats>();
        playerStats.OnMovementSpeedUpgraded += UpdateMovementSpeed;
    }

    private void OnDisable()
    {
        playerStats.OnMovementSpeedUpgraded -= UpdateMovementSpeed;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        joystick = FindObjectOfType<Joystick>();
        nearestEnemyFinder = GetComponent<NearestEnemyFinder>();

        UpdateMovementSpeed();
    }

    private void FixedUpdate()
    {
        ToInputAxis();
    }
    private void Update()
    {
        ToMove();
        rb.velocity = Vector3.zero;
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

            if (nearestEnemyFinder.GetNearestEnemy() == null)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, targetAngle, 0f), rotationSpeed * Time.deltaTime);
            }
        }
    }


    private void UpdateMovementSpeed()
    {
        speed = playerStats.GetMovementSpeed();
    }

    public bool HasJoystickInput()
    {
        return joystick.hasJoystickInput;
    }
    public Vector3 GetDirection()
    {
        return direction;
    }
}


