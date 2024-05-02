using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStats : DefencesStatsBase
{
    public List<GameObject> wallParts; // children barriers are set in the inspector, needs to be changed
    public List<float> healthParts;
}
