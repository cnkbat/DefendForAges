using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyableManager : MonoBehaviour
{
    PlayerStats playerStats;

    [SerializeField] List<BuyableArea> buyableAreas;
    [SerializeField] List<int> buyableAreaEnablingIndexes;

    private void OnEnable()
    {
        playerStats = PlayerStats.instance;
    }

    void Start()
    {
        for (int i = 0; i < buyableAreas.Count; i++)
        {
            
        }
    }   


    void Update()
    {

    }
}
