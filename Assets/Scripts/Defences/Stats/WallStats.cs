using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallStats : DefencesStatsBase
{
    public List<GameObject> wallParts;
    public List<float> healthParts;

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
