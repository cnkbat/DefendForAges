using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArcherStickman : MonoBehaviour
{
    private Animator animator;
    private ArcherTowerBehaviour archerTowerBehaviour;
    public Action OnThrowArrow;
    private Vector3 startPos;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        startPos = transform.localPosition;
        archerTowerBehaviour = transform.parent.parent.GetComponent<ArcherTowerBehaviour>();

    }

    private void OnEnable()
    {
        OnThrowArrow += archerTowerBehaviour.ThrowArrow;
    }

    private void OnDisable()
    {
        OnThrowArrow -= archerTowerBehaviour.ThrowArrow;
    }

    private void Update()
    {
        transform.localPosition = startPos;
    }
    public void PlayAttackAnimation(float animSpeed)
    {
        animator.SetTrigger("Attack");
        animator.SetFloat("AttackAnimSpeed", animSpeed);
    }

    public void AttackAtEnemy()
    {
        OnThrowArrow?.Invoke();
    }

}
