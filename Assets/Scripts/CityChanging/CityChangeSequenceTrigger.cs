using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityChangeSequenceTrigger : MonoBehaviour
{
    public Action OnSequencetTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerStats playerStats))
        {
            OnSequencetTriggered?.Invoke();
        }
    }
}
