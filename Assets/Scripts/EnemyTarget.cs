using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour, ITargetable
{

    void Start()
    {

    }

    void Update()
    {

    }

    /// fiziksel hasar
    /// yıkım ölüm
    /// vs.
     
    public virtual void TakeDamage(float dmg)
    {
        throw new System.NotImplementedException();
    }

    public Transform GetTarget()
    {
        return transform;
    }
}
