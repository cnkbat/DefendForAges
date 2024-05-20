using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Components")]
    PlayerStats playerStats;
    NearestEnemyFinder nearestEnemyFinder;
    [SerializeField] List<Weapon> weapons;


    [Header("Fire Range")]
    [SerializeField] private float fireRange;

    [Header("Anim")]
    [SerializeField] private float strikingAnimDur;
    private float currentStrikingAnimDur;

    [Tooltip("Attack Speed")]
    float currentAttackSpeed;

    [Tooltip("Events")]
    public Action<Transform, float, bool, float> OnAttack;
    public Action OnAttackAnimPlayNeeded;

    private void OnEnable()
    {
        playerStats = PlayerStats.instance;
        playerStats.OnWeaponActivision += EnableWeapons;

        for (int i = 0; i < weapons.Count; i++)
        {
            OnAttack += weapons[i].Attack;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            OnAttack -= weapons[i].Attack;
        }
    }

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        nearestEnemyFinder = GetComponent<NearestEnemyFinder>();

        ResetAttackSpeed();
        nearestEnemyFinder.SetFireRange(fireRange);
    }

    private void Update()
    {
        if (playerStats.GetIsDead()) return;

        if (nearestEnemyFinder.GetNearestEnemy())
        {
            LookAtNearstEnemy(nearestEnemyFinder.GetNearestEnemy());

            currentAttackSpeed -= Time.deltaTime;
            if (currentAttackSpeed <= 0)
            {
                OnAttack?.Invoke(nearestEnemyFinder.GetNearestEnemy(), playerStats.GetDamage(), true, currentStrikingAnimDur);
                OnAttackAnimPlayNeeded?.Invoke();
                ResetAttackSpeed();
            }
        }
        else
        {
            ResetAttackSpeed();
        }
    }

    #region  Finding Closest Enemy

    private void LookAtNearstEnemy(Transform closestEnemy)
    {

        if (closestEnemy != null)
        {
            //[SerializeField] Transform headAimObject;
            // ! anim rigging yapcaz ? belki
            //  headAimObject.transform.parent = closestEnemy;
            //headAimObject.transform.localPosition = Vector3.zero;


            Vector3 worldAimTarget = closestEnemy.transform.position;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = aimDirection;

        }

    }


    #endregion

    #region Firing

    private void ResetAttackSpeed()
    {
        currentAttackSpeed = playerStats.GetAttackSpeed();
    }

    public void SetAttackingDelay(float delayMultiplier)
    {
        currentStrikingAnimDur = strikingAnimDur * delayMultiplier;
    }

    #endregion

    #region Dual Weaponing
    public void EnableWeapons(int index)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < index; i++)
        {
            weapons[i].gameObject.SetActive(true);
        }
    }
    #endregion
}
