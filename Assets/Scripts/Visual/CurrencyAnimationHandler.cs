using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyAnimationHandler : MonoBehaviour, IPoolableObject
{
    Animator animator;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }

    public void OnObjectPooled()
    {
        // data resetlemek gerekirse
        throw new System.NotImplementedException();
    }

    public void ResetObjectData()
    {
        // data resetlemek gerekirse
        throw new System.NotImplementedException();
    }

    public void PlayDropAnim()
    {
        animator.SetTrigger("DropAnim");
    }

    public void PlayCollectionAnim()
    {
        animator.SetTrigger("DropAnim");

    }

    public void PlaySpendAnim()
    {
        animator.SetTrigger("SpendAnim");

    }

}
