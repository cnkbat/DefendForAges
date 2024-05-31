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
    private float yValue;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        archerTowerBehaviour = transform.parent.parent.GetComponent<ArcherTowerBehaviour>();

        yValue = transform.localPosition.y;
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
        transform.localPosition = new Vector3(transform.localPosition.x, yValue, transform.localPosition.z);
    }
    public void PlayAttackAnimation(float animSpeed)
    {
        animator.SetTrigger("Attack");
        //     animator.SetFloat("AttackAnimSpeed", animSpeed);
    }

    public void AttackAtEnemy()
    {
        OnThrowArrow?.Invoke();
    }

}
