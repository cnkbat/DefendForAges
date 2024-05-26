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


    [Header("Aim")]
    [SerializeField] private float fireRange;
    [SerializeField] private float lookAtSense = 20;

    [Header("Anim")]
    [SerializeField] private float strikingAnimDur;

    Transform playerAsset;
    private Vector3 playerAssetLocalPos;

    [Tooltip("Attack Speed")]
    float currentAttackSpeed;

    [Tooltip("Events")]
    public Action<Transform, float, bool> OnAttack;
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

        playerAsset = GameObject.Find("playerAsset").transform;
        playerAssetLocalPos = playerAsset.localPosition;

        ResetAttackSpeed();
        nearestEnemyFinder.SetFireRange(fireRange);
    }

    private void Update()
    {
        if (playerStats.GetIsDead()) return;

        playerAsset.localPosition = playerAssetLocalPos;

        if (nearestEnemyFinder.GetNearestEnemy())
        {
            LookAtNearstEnemy(nearestEnemyFinder.GetNearestEnemy());

            currentAttackSpeed -= Time.deltaTime;
            if (currentAttackSpeed <= 0)
            {
                OnAttackAnimPlayNeeded?.Invoke();
            }
        }
        else
        {
            ResetAttackSpeed();
        }
    }

    public void Attack()
    {
        OnAttack?.Invoke(nearestEnemyFinder.GetNearestEnemy(), playerStats.GetDamage(), true);
        ResetAttackSpeed();
    }

    #region  Finding Closest Enemy

    private void LookAtNearstEnemy(Transform closestEnemy)
    {

        if (closestEnemy != null)
        {

            Vector3 worldAimTarget = closestEnemy.transform.position;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, lookAtSense * Time.deltaTime);
            playerAsset.forward = aimDirection;

        }

    }


    #endregion

    #region Firing

    private void ResetAttackSpeed()
    {
        currentAttackSpeed = playerStats.GetAttackSpeed();
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
