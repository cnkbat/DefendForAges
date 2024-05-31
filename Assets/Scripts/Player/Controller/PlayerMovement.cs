using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private GameManager gameManager;
    private NearestEnemyFinder nearestEnemyFinder;
    private PlayerStats playerStats;
    private Rigidbody rb;
    private Animator anim;

    [SerializeField] private Joystick joystick;

    [Header("Walk")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float speed = 5f;


    private Vector3 direction;
    private float horizontal;
    private float vertical;
    private float targetAngle = 0;
    private Transform playerAsset;

    private void OnEnable()
    {
        playerStats = GetComponent<PlayerStats>();
        gameManager = GameManager.instance;

        playerStats.OnMovementSpeedUpgraded += UpdateMovementSpeed;
    }

    private void OnDisable()
    {
        playerStats.OnMovementSpeedUpgraded -= UpdateMovementSpeed;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        joystick = FindObjectOfType<Joystick>(true);
        nearestEnemyFinder = GetComponent<NearestEnemyFinder>();
        playerAsset = GameObject.Find("playerAsset").transform;
        anim = GetComponentInChildren<Animator>();

        UpdateMovementSpeed();
    }

    private void FixedUpdate()
    {
        ToInputAxis();
    }
    private void Update()
    {
        if (gameManager.isPlayerFreezed) return;
        if (playerStats.GetIsDead()) return;

        ToMove();
        rb.velocity = Vector3.zero;
    }

    void ToInputAxis()
    {
        horizontal = joystick.result.x;
        vertical = joystick.result.y;
        direction = new Vector3(horizontal, 0, vertical).normalized;
        if (direction != Vector3.zero)
            anim.SetBool("isWalking", true);
        else
            anim.SetBool("isWalking", false);
    }

    void ToMove()
    {
        if (Input.GetMouseButton(0))
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            transform.position = new Vector3(transform.position.x + horizontal * speed * Time.deltaTime, transform.position.y, transform.position.z + vertical * speed * Time.deltaTime);

            if (nearestEnemyFinder.GetNearestEnemy() == null)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, targetAngle, transform.rotation.z),
                     rotationSpeed * Time.deltaTime);
                playerAsset.forward = transform.forward;
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


