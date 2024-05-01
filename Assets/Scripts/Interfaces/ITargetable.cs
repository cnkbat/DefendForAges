using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{
    void TakeDamage(float dmg);
    void ResetHealthValue();
}
