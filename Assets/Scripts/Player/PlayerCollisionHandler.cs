using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerDeathHandler playerDeathHandler;

    [SerializeField] private float triggerTimer;
    private float currentTriggerTimer;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerDeathHandler = GetComponent<PlayerDeathHandler>();
        ResetTriggerTimer();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (playerDeathHandler.GetIsDead()) return;

        if (other.gameObject.TryGetComponent(out ICollectable collectable))
        {
            collectable.Collect();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerDeathHandler.GetIsDead()) return;

        if (other.gameObject.TryGetComponent(out ILoadable loadable))
        {
            if (!playerMovement.HasJoystickInput())
            {
                currentTriggerTimer -= Time.deltaTime;

                if (currentTriggerTimer < 0)
                {
                    loadable.Load();
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
