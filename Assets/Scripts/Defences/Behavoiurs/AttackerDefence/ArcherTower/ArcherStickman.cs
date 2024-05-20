using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStickman : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float animDur;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    public float GetAnimDur()
    {
        return animDur;
    }

}
