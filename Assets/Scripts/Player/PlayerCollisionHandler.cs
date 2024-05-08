using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    PlayerMovement playerMovement;

    [SerializeField] private float triggerTimer;
    private float currentTriggerTimer;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        ResetTriggerTimer();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out ICollectable collectable))
        {
            collectable.Collect();
        }
    }

    private void OnTriggerStay(Collider other)
    {
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

    private void ResetTriggerTimer()
    {
        currentTriggerTimer = triggerTimer;
    }
}
