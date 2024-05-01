using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    public void Kill()
    {
        gameObject.SetActive(false);
        Time.timeScale = 0;
        // activate UI with revive button
    }
    // will be connected to revive button
    public void Revive()
    {

        CityManager cityManager = FindObjectOfType<CityManager>();
        transform.position = cityManager.GetRevivePoint().position;
        this.GetComponent<PlayerStats>().FillCurrentHealth();
        gameObject.SetActive(true);
    }
}
