using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour, IPoolableObject
{
    PlayerStats playerStats;
    GameManager gameManager;
    private float damage;
    [SerializeField] float moveSpeed;
    Transform target;
    Vector3 targetPos;
    Vector3 firedPoint;

    [Header("Life Steal")]
    bool isPlayersBullet;
    public Action<float> OnDamageDealt;

    [Header("VFX")]
    [SerializeField] TrailRenderer trailFX;

    bool targetReached = false;

    private void OnEnable()
    {
        playerStats = PlayerStats.instance;
        OnDamageDealt += playerStats.IncrementHealth;
        gameManager = GameManager.instance;
    }

    private void OnDisable()
    {
        OnDamageDealt -= playerStats.IncrementHealth;
    }

    #region IPoolableObject Functions

    public void OnObjectPooled()
    {
        ResetObjectData();
    }

    public void ResetObjectData()
    {
        targetReached = false;
        targetPos = target.transform.position;
        firedPoint = transform.position;
        Vector3 worldAimTarget = targetPos;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

        transform.forward = aimDirection;
    }

    #endregion

    void Update()
    {

        if (target != null && (!target.gameObject.activeSelf || targetReached))
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (transform.position == targetPos)
            {
                targetReached = true;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }

        if (gameManager.bulletFireRange < Vector3.Distance(transform.position, firedPoint))
        {
            gameObject.SetActive(false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(damage);

            if (isPlayersBullet)
            {
                OnDamageDealt.Invoke(damage);
            }

            gameObject.SetActive(false);
        }
        else if (other.TryGetComponent(out IEnvironment environment))
        {
            gameObject.SetActive(false);
        }
    }

    #region Getters & Setters
    public void SetDamage(float newDmg)
    {
        damage = newDmg;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }


    public void SetTrailColor(Color startColor, Color endColor)
    {

        trailFX.startColor = startColor;
        trailFX.endColor = endColor;

    }

    public void SetIsPlayerBullet(bool newBool)
    {
        isPlayersBullet = newBool;
    }

    #endregion
}
