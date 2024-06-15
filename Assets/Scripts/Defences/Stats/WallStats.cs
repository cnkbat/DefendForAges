using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallStats : DefencesStatsBase
{
    [Tooltip("Köşeler")] public List<GameObject> wallHolderParts;
    [Tooltip("Ortadaki parçalar")] public List<GameObject> wallParts;
    [Tooltip("Ortadaki parçaların başlangıç pozisyonu")] public List<List<Vector3>> wallPartLocations;
    [HideInInspector] public List<float> healthParts;

    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < wallParts.Count; i++)
        {
            healthParts[i] = base.GetMaxHealth() / (i + 1);
        }
    }


    public override void BuyDone()
    {
        base.BuyDone();
    }
}
