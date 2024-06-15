using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RagdollManager : MonoBehaviour
{

    private Collider[] ragdollColliders;
    private BoxCollider objectCollider;
    private Rigidbody objectRigidBody;
    private Animator animator;
    [SerializeField] private float upForce = 3;

    [Header("Actions")]
    public Action OnRagdollEnable;
    public Action OnRagdollDisable;

    private void Awake()
    {
        ragdollColliders = GetComponentsInChildren<Collider>();
        objectCollider = GetComponentInParent<BoxCollider>();
        animator = GetComponentInParent<Animator>();
        objectRigidBody = GetComponentInParent<Rigidbody>();
    }

    private void OnEnable()
    {
        OnRagdollEnable += EnableRagdoll;
        OnRagdollDisable += DisableRagdoll;
    }

    private void OnDisable()
    {
        OnRagdollEnable -= EnableRagdoll;
        OnRagdollDisable -= DisableRagdoll;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DisableRagdoll();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            EnableRagdoll();
        }
    }

    private void DisableRagdoll()
    {
        foreach (var collider in ragdollColliders)
        {
            collider.isTrigger = true;
        }

        objectCollider.enabled = true;
        animator.enabled = true;

    }

    private void EnableRagdoll()
    {
        objectRigidBody.AddForce((100f * Vector3.up) + (-100f * transform.forward));
        
        objectRigidBody.useGravity = false;
        objectRigidBody.velocity = Vector3.zero;


        animator.enabled = false;
        objectCollider.enabled = false;

        foreach (var collider in ragdollColliders)
        {
            collider.isTrigger = false;
            collider.attachedRigidbody.velocity = Vector3.zero;
        }

    }

}
