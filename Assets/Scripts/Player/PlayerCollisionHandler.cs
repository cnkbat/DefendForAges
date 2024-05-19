using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerStats playerStats;

    [SerializeField] private float triggerTimer;
    private float currentTriggerTimer;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();
        ResetTriggerTimer();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (playerStats.GetIsDead()) return;

        if (other.gameObject.TryGetComponent(out ICollectable collectable))
        {
            collectable.Collect();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerStats.GetIsDead()) return;

        if (other.gameObject.TryGetComponent(out ILoadable loadable))
        {
            if (!playerMovement.HasJoystickInput())
            {
                currentTriggerTimer -= Time.deltaTime;

                if (currentTriggerTimer < 0)
                {
                    loadable.Load();
                    ResetTriggerTimer();
                }
            }
            else
            {
                ResetTriggerTimer();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ILoadable loadable))
        {
            loadable.Unload();
        }
    }

    private void ResetTriggerTimer()
    {
        currentTriggerTimer = triggerTimer;
    }
}
