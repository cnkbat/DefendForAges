using UnityEngine;
using DG.Tweening;

public class TrapBehaviour : AttackerDefenceBehaviour
{

    [Header("Spikes & Spike Poses")]
    [SerializeField] private Transform spikes;
    [SerializeField] private Transform originalSpikePos;
    [SerializeField] private Transform strikePos;

    [Header("Attacking")]
    [Tooltip("Yukari ve aşaği olan hareket attackspeede göre hareket ediyor.\n"
    + "Bundan dolayi buraya girilen değer attack speedi bölerek o sürede hareket ettiriyor.")]
    [SerializeField] private float attackSpeedDividor;
    private void Awake()
    {
        if (isTargetable)
        {
            isTargetable = false;
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        if (isTargetable)
        {
            isTargetable = false;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (isTargetable)
        {
            isTargetable = false;
        }
    }

    protected override void Start()
    {
        base.Start();
        isTargetable = false;
    }

    private void FixedUpdate()
    {
        if (isTargetable)
        {
            isTargetable = false;
        }
    }

    protected override void Attack()
    {

        base.Attack();

        spikes.DOMoveY(strikePos.position.y, attackerDefenceStat.GetAttackSpeed() / attackSpeedDividor).
            OnComplete(() =>
            {
                spikes.DOMoveY(originalSpikePos.position.y, attackerDefenceStat.GetAttackSpeed() / attackSpeedDividor);
            });

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(attackerDefenceStat.GetDamage());
        }
    }

    public override void ReviveTarget()
    {
        base.ReviveTarget();
        isTargetable = false;
    }
}
