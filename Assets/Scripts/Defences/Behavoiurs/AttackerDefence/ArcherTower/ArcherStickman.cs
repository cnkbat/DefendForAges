using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStickman : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Transform tipOfWeapon;
    [SerializeField] private float animDur;

    public Transform GetTipOfWeapon()
    {
        return tipOfWeapon;
    }

    public void PlayAttackAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    public float GetAnimDur()
    {
        return animDur;
    }
}
