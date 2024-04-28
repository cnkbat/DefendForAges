using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObjects/RPG System")]

public class RPGSystemSO : ScriptableObject
{

    [Header("Base Stats")]
    public List<float> attackSpeedValues;
    public List<float> damageValues;
    public List<float> movementSpeedValues;
    public List<float> lifeStealValues;
    public List<float> powerupDurValues;
    public List<float> maxHealthValues;

}
