using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStickman : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Transform tipOfWeapon;

    public Transform GetTipOfWeapon()
    {
        return tipOfWeapon;
    }
}
