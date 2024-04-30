using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObjects/RPG System")]

public class RPGSystemSO : ScriptableObject
{

    [Header("Attack Speed")]

    public List<float> attackSpeedValues;
    public List<int> attackSpeedCosts;
    
    [Header("Damage")]
    public List<float> damageValues;
    public List<int> damageCosts;

    [Header("Movement Speed")]
    public List<float> movementSpeedValues;
    public List<int> movementSpeedCosts;


    [Header("Life Steal")]
    public List<float> lifeStealValues;
    public List<int> lifeStealCosts;

    [Header("Power Up")]
    public List<float> powerupDurValues;
    public List<int> powerupDurCosts;

    [Header("Max Health")]
    public List<float> maxHealthValues;
    public List<int> maxHealthCosts;


}
