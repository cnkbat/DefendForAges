using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargeter : MonoBehaviour
{
    private Transform target;

    void Start()
    {

    }

    void Update()
    {

    }


    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public Transform GetTarget()
    {
        return target;
    }
}
