using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyAnimationHandler : MonoBehaviour, IPoolableObject
{
    Animator animator;

    private void OnEnable()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void OnObjectPooled()
    {
        // data resetlemek gerekirse
    }

    public void ResetObjectData()
    {
        // data resetlemek gerekirse
    }

    public void PlayDropAnim()
    {
        animator.SetTrigger("DropAnim");
    }

    public void PlayCollectionAnim()
    {
        animator.SetTrigger("CollectAnim");
    }

    public void PlaySpendAnim()
    {
        animator.SetTrigger("SpendAnim");

    }

}
