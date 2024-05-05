using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStats : DefencesStatsBase
{
    public List<GameObject> wallParts;
    public List<float> healthParts;

    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < wallParts.Count; i++)
        {
            healthParts[i] = base.GetMaxHealth() / (i + 1);
        }

    }
}
