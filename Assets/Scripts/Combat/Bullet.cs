using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour, IPoolableObject
{
    private float damage;
    [SerializeField] float moveSpeed;
    Transform target;
    Transform targetPos;
    Transform firedPoint;

    [SerializeField] float fireRange = 150f;

    [Header("VFX")]
    [SerializeField] TrailRenderer trailFX;

    public void OnObjectPooled()
    {
        targetPos = target.transform;
    }

    void Update()
    {
        if (target != null && !target.gameObject.activeSelf)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos.transform.position, moveSpeed * Time.deltaTime);
        }
        else if (fireRange < Vector3.Distance(transform.position, firedPoint.position))
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }

    public void SetDamage(float newDmg)
    {
        damage = newDmg;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetFiredPoint(Transform newTransform)
    {
        firedPoint = newTransform;
    }

    public void SetTrailColor(Color startColor, Color endColor)
    {

        trailFX.startColor = startColor;
        trailFX.endColor = endColor;

    }
}
