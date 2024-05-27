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
    [Tooltip("Boşlukta kalınca buga giriyor süresi bundan dolayı var.")][SerializeField] private float disappearTime;
    private float currentDisappearTime;
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
    [SerializeField] private TrailRenderer trailFX;

    private bool targetReached = false;

    private void OnEnable()
    {
        playerStats = PlayerStats.instance;
        OnDamageDealt += playerStats.IncrementHealth;
        gameManager = GameManager.instance;
        objectPooler = ObjectPooler.instance;
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

        firedPoint = transform.position;

        Vector3 worldAimTarget = targetPos;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
        transform.forward = aimDirection;

        currentDisappearTime = disappearTime;

        hasHit = false;
    }

    #endregion

    void Update()
    {
        currentDisappearTime -= Time.deltaTime;
        if (currentDisappearTime < 0)
        {
            gameObject.SetActive(false);
        }

        transform.position += Vector3.up * gravity;

        if (target != null && (!target.gameObject.activeSelf || targetReached))
        {
            transform.position += transform.forward * travelSpeed * Time.deltaTime;
        }
        else if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, travelSpeed * Time.deltaTime);

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
        if (hasHit) return;

        if (other.TryGetComponent(out IDamagable damagable))
        {
            hasHit = true;

            damagable.TakeDamage(damage);

            if (isPlayersBullet)
            {
                OnDamageDealt.Invoke(damage);
            }

            objectPooler.SpawnFloatingTextFromPool("FloatingText", targetPos, damage, damage * gameManager.fontSizeOnEnemyHit, Color.red);

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
