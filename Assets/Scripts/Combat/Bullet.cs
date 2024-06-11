using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour, IPoolableObject
{
    private PlayerStats playerStats;
    private GameManager gameManager;
    private ObjectPooler objectPooler;

    [Header("Travelling")]
    [SerializeField] private float travelSpeed;
    [Tooltip("Yere düşüş için")][SerializeField] private float gravity;

    [Header("Targeting")]
    private Transform target;
    private Vector3 targetPos;

    [Header("Damage")]
    private bool hasHit;
    private float damage;
    private Vector3 firedPoint;

    [Header("Life Steal")]
    private bool isPlayersBullet;
    public Action<float> OnDamageDealt;

    [Header("VFX")]
    [SerializeField] private ParticleSystem trailFX;
    [SerializeField] private ParticleSystem powerupedVFX;
    private bool targetReached = false;

    private void OnEnable()
    {
        playerStats = PlayerStats.instance;
        gameManager = GameManager.instance;
        objectPooler = ObjectPooler.instance;

        OnDamageDealt += playerStats.IncrementHealth;
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

        if (target != null)
        {
            targetPos = target.transform.position;
        }


        if (powerupedVFX != null)
        {
            powerupedVFX?.Stop();
        }

        if (trailFX != null)
        {
            trailFX?.Play();
        }

        if (isPlayersBullet)
        {
            if (playerStats.GetIsPowerupEnabled() && powerupedVFX != null)
            {
                powerupedVFX?.Play();

                if (trailFX != null)
                {
                    trailFX?.Stop();
                }
            }
            else
            {
                if (powerupedVFX != null)
                {
                    powerupedVFX?.Stop();

                    if (powerupedVFX.TryGetComponent(out Light light))
                    {
                        light.enabled = false;
                    }
                }
            }
        }


        firedPoint = transform.position;

        Vector3 worldAimTarget = targetPos;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
        transform.forward = aimDirection;

        hasHit = false;
    }

    #endregion

    void Update()
    {

        transform.position += Vector3.up * gravity;

        if (target != null && target.gameObject.activeSelf)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, travelSpeed * Time.deltaTime);

            if (transform.position == targetPos)
            {
                targetReached = true;
            }
        }
        else
        {
            DisableBullet();
        }

        if (targetReached && target.gameObject.activeSelf)
        {
            transform.position += transform.forward * travelSpeed * Time.deltaTime;
        }
        if (gameManager.bulletFireRange < Vector3.Distance(transform.position, firedPoint))
        {
            DisableBullet();
        }

    }

    private void DisableBullet()
    {
        if (trailFX != null)
        {
            trailFX.Stop();
        }

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (other.TryGetComponent(out IDamagable damagable))
        {
            hasHit = true;

            damagable.TakeDamage(damage);

            if (isPlayersBullet)
            {
                OnDamageDealt.Invoke(damage);
            }

            objectPooler.SpawnFloatingTextFromPool("FloatingText", targetPos, damage, damage * gameManager.fontSizeOnEnemyHit,
                playerStats.GetIsPowerupEnabled() ? Color.red : Color.white);

            DisableBullet();
        }
        else if (other.TryGetComponent(out IEnvironment environment))
        {
            DisableBullet();
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

    public void SetIsPlayerBullet(bool newBool)
    {
        isPlayersBullet = newBool;
    }

    #endregion
}
