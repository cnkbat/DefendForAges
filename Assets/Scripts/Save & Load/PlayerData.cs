using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int money;
    

//    public bool isHapticActive, isSoundFXActive;

   public PlayerData(PlayerStats playerStats)
    {
        
        money = playerStats.money;

       // isHapticActive = playerStats.isHapticActive;
       // isSoundFXActive = playerStats.isSoundFXActive;
    } 
}
