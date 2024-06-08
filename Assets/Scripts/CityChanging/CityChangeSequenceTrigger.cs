using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityChangeSequenceTrigger : MonoBehaviour
{
    public Action OnSequencetTriggered;
    BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerStats playerStats))
        {
            OnSequencetTriggered?.Invoke();
            boxCollider.enabled = false;
        }
    }

}
