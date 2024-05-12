using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTowerBehaviour : AttackerDefenceBehaviour
{
    ObjectPooler objectPooler;
    NearestEnemyFinder nearestEnemyFinder;

    [Header("Combat")]
    [SerializeField] private float fireRange;
    [SerializeField] private string bulletTag;

    [Header("! -- Archer Visuals -- !")]
    [SerializeField] private ArcherStickman archerStickman; // stickman on top

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Start()
    {
        base.Start();
        objectPooler = ObjectPooler.instance;
        nearestEnemyFinder = GetComponent<NearestEnemyFinder>();
        nearestEnemyFinder.SetFireRange(fireRange);
    }

    protected override void Update()
    {
        if (!nearestEnemyFinder.GetNearestEnemy()) return;
        base.Update();
        LookAtNearstEnemy(nearestEnemyFinder.GetNearestEnemy());
    }

    protected override void Attack()
    {
        base.Attack();

        objectPooler.SpawnBulletFromPool(bulletTag, archerStickman.GetTipOfWeapon().position,
            nearestEnemyFinder.GetNearestEnemy(), attackerDefenceStat.GetDamage());
    }

    private void LookAtNearstEnemy(Transform closestEnemy)
    {
        if (!archerStickman) return;

        if (closestEnemy != null)
        {
            //[SerializeField] Transform headAimObject;
            // ! anim rigging yapcaz ? belki
            //  headAimObject.transform.parent = closestEnemy;
            //headAimObject.transform.localPosition = Vector3.zero;


            Vector3 worldAimTarget = closestEnemy.transform.position;
            worldAimTarget.y = archerStickman.transform.position.y;
            Vector3 aimDirection = (worldAimTarget - archerStickman.transform.position).normalized;

            archerStickman.transform.forward = aimDirection;

        }

    }
}
