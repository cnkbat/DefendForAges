using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUIHandler : MonoBehaviour
{
    UIManager uiManager;

    void Start()
    {
        uiManager = UIManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerCollisionHandler playerCollisionHandler))
        {
            Debug.Log("trigger");
            EnableUpgradeUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerCollisionHandler playerCollisionHandler))
        {
            DisableUpgradeUI();
        }
    }

    private void EnableUpgradeUI()
    {
        uiManager.EnableUpgradingButton();
    }

    private void DisableUpgradeUI()
    {
        uiManager.DisableUpgradingButton();
    }

}
