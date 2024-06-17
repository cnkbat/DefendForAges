using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ArcherTowerBehaviour : AttackerDefenceBehaviour
{

    NearestEnemyFinder nearestEnemyFinder;

    [Header("Aim")]
    [SerializeField] private float fireRange;
    [SerializeField] private float lookAtSense;

    [Header("! -- Archer Visuals -- !")]
    [SerializeField] private Animator animator;
    [SerializeField] private List<ArcherStickman> archerStickmans; // stickman on top

    [Header("Actions")]
    public Action<float> OnStickmanAnimationPlayNeeded;

    protected override void OnEnable()
    {
        base.OnEnable();

        for (int i = 0; i < attackerDefenceStat.GetWeapons().Count; i++)
        {
            OnRangedAttack += attackerDefenceStat.GetWeapons()[i].Attack;
        }

        for (int i = 0; i < archerStickmans.Count; i++)
        {
            OnStickmanAnimationPlayNeeded += archerStickmans[i].PlayAttackAnimation;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        for (int i = 0; i < attackerDefenceStat.GetWeapons().Count; i++)
        {
            OnRangedAttack -= attackerDefenceStat.GetWeapons()[i].Attack;
        }

        for (int i = 0; i < archerStickmans.Count; i++)
        {
            OnStickmanAnimationPlayNeeded -= archerStickmans[i].PlayAttackAnimation;
        }
    }

    protected override void Start()
    {
        base.Start();

        nearestEnemyFinder = GetComponent<NearestEnemyFinder>();
        nearestEnemyFinder.SetFireRange(fireRange);

        for (int i = 0; i < archerStickmans.Count; i++)
        {
            OnStickmanAnimationPlayNeeded += archerStickmans[i].PlayAttackAnimation;
        }
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DestroyDefence();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ReviveTarget();
        }
        
        if (isDestroyed) return;

        if (!nearestEnemyFinder.GetNearestEnemy()) return;
        base.Update();
        LookAtNearstEnemy(nearestEnemyFinder.GetNearestEnemy());

    }

    #region Attack

    protected override void Attack()
    {
        base.Attack();

        if (!nearestEnemyFinder.GetNearestEnemy()) return;

        // might need to add a check for newspeed == 0
        OnStickmanAnimationPlayNeeded?.Invoke(attackerDefenceStat.GetAttackSpeed());
    }

    public void ThrowArrow()
    {
        OnRangedAttack?.Invoke(nearestEnemyFinder.GetNearestEnemy(), attackerDefenceStat.GetDamage(), false);
    }

    private void LookAtNearstEnemy(EnemyDeathHandler closestEnemy)
    {
        if (archerStickmans.Count <= 0) return;

        if (closestEnemy != null)
        {
            //[SerializeField] Transform headAimObject;
            // ! anim rigging yapcaz ? belki
            //  headAimObject.transform.parent = closestEnemy;
            //headAimObject.transform.localPosition = Vector3.zero;


            for (int i = 0; i < archerStickmans.Count; i++)
            {
                Vector3 worldAimTarget = closestEnemy.transform.position;
                worldAimTarget.y = archerStickmans[i].transform.position.y;
                Vector3 aimDirection = (worldAimTarget - archerStickmans[i].transform.position).normalized;

                archerStickmans[i].transform.forward = Vector3.Lerp(archerStickmans[i].transform.forward, aimDirection, lookAtSense * Time.deltaTime);
            }
        }
    }

    #endregion

    protected override void DestroyDefence()
    {
        base.DestroyDefence();

        animator.SetTrigger("Destroy");

        for (int i = 0; i < archerStickmans.Count; i++)
        {
            archerStickmans[i].transform.DOScale(0, 0.5f).
                OnComplete(() => archerStickmans[i].gameObject.SetActive(false));
        }
    }

    public override void ReviveTarget()
    {
        base.ReviveTarget();

        animator.SetTrigger("Revive");

        for (int i = 0; i < archerStickmans.Count; i++)
        {
            archerStickmans[i].gameObject.SetActive(true);
            archerStickmans[i].transform.DOScale(new Vector3(0.3580822f, 0.1451476f, 0.3612171f), 1f).SetEase(Ease.OutElastic);
        }
    }
}
