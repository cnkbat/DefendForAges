using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    UIManager uiManager;
    public void Start()
    {
        uiManager = UIManager.instance;
    }
    public void Kill()
    {
        gameObject.SetActive(false);
        Time.timeScale = 0;
        uiManager.HandleReviveUI();
    }
    // will be connected to revive button
    public void Revive()
    {

        CityManager cityManager = FindObjectOfType<CityManager>();
        transform.position = cityManager.GetRevivePoint().position;
        this.GetComponent<PlayerStats>().FillCurrentHealth();
        gameObject.SetActive(true);
        uiManager.HandleReviveUI();
        Time.timeScale = 1;
    }
}
