using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Components")]
    private GameManager gameManager;
    private PlayerStats playerStats;
    private PlayerDeathHandler playerDeathHandler;
    private NearestEnemyFinder nearestEnemyFinder;
    [SerializeField] private List<Weapon> weapons;


    [Header("Aim")]
    [SerializeField] private float lookAtSense = 20;
    private Transform playerAsset;
    private Vector3 playerAssetLocalPos;

    [Tooltip("Attack Speed")]
    float currentAttackSpeed;

    [Tooltip("Events")]
    public Action<EnemyDeathHandler, float, bool> OnAttack;
    public Action OnAttackAnimPlayNeeded;
    private void Awake()
    {
        playerDeathHandler = GetComponent<PlayerDeathHandler>();
        playerStats = GetComponent<PlayerStats>();
        nearestEnemyFinder = GetComponent<NearestEnemyFinder>();

        playerAsset = GameObject.Find("playerAsset").transform;
        playerAssetLocalPos = playerAsset.localPosition;
    }

    private void OnEnable()
    {
        playerStats = PlayerStats.instance;
        gameManager = GameManager.instance;

        playerStats.OnWeaponActivision += EnableWeapons;
        playerStats.OnRangeSet += UpdateFireRange;

        for (int i = 0; i < weapons.Count; i++)
        {
            OnAttack += weapons[i].Attack;
        }
    }

    private void OnDisable()
    {
        playerStats.OnWeaponActivision -= EnableWeapons;
        playerStats.OnRangeSet -= UpdateFireRange;

        for (int i = 0; i < weapons.Count; i++)
        {
            OnAttack -= weapons[i].Attack;
        }
    }

    private void Start()
    {

        ResetAttackSpeed();
        UpdateFireRange();
    }

    private void UpdateFireRange()
    {
        nearestEnemyFinder.SetFireRange(playerStats.GetRange());
    }

    private void Update()
    {
        if (gameManager.isGameFreezed) return;

        if (playerDeathHandler.GetIsDead()) return;

        playerAsset.localPosition = playerAssetLocalPos;

        if (nearestEnemyFinder.GetNearestEnemy())
        {
            LookAtNearstEnemy(nearestEnemyFinder.GetNearestEnemy());
            currentAttackSpeed -= Time.deltaTime;

            if (nearestEnemyFinder.GetIsEnemyInFiringRange())
            {
                if (currentAttackSpeed <= 0)
                {
                    OnAttackAnimPlayNeeded?.Invoke();
                    ResetAttackSpeed();
                }
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
    }

    #region  Finding Closest Enemy

    private void LookAtNearstEnemy(EnemyDeathHandler closestEnemy)
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
