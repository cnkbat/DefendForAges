using UnityEngine;
using DG.Tweening;

public class TrapBehaviour : AttackerDefenceBehaviour
{

    [SerializeField] BoxCollider spikesCollider;

    [SerializeField] Transform spikes;
    [SerializeField] Transform originalSpikePos;
    [SerializeField] Transform strikePos;

    bool isStriked;

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
    }

    protected override void Attack()
    {

        base.Attack();


        spikes.DOMoveY(strikePos.position.y, attackerDefenceStat.GetAttackSpeed() / 4).
            OnComplete(() =>
            {
                spikes.DOMoveY(originalSpikePos.position.y, attackerDefenceStat.GetAttackSpeed() / 4);
            });

    }

}
