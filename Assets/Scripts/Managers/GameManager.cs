using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [Header("Enemies")]
    public List<GameObject> allSpawnedEnemies;

    [Header("Cities")]
    [SerializeField] private List<CityManager> allCities;
    [SerializeField] private int currentCityIndex;
    
    void Start()
    {

    }


    #region  Getters & Setters
    public void SetCurrentCityIndex(int newIndex)
    {
        currentCityIndex = newIndex;
    }

    #endregion
}
