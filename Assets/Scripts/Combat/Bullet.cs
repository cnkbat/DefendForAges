using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour, IPoolableObject
{
    private float damage;
    [SerializeField] float moveSpeed;
    Transform target;
    Vector3 targetPos;
    Transform firedPoint;

    [SerializeField] float fireRange = 150f;

    [Header("VFX")]
    [SerializeField] TrailRenderer trailFX;

    bool targetReached = false;

    #region IPoolableObject Functions
    public void OnObjectPooled()
    {
        ResetObjectData();
    }
    public void ResetObjectData()
    {
        targetReached = false;
        targetPos = target.transform.position;

        Vector3 worldAimTarget = targetPos;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

        transform.forward = aimDirection;
    }
    #endregion

    void Update()
    {

        if (target != null && (!target.gameObject.activeSelf || targetReached))
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (transform.position == targetPos)
            {
                targetReached = true;
            }
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

    #region Getters & Setters
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

    #endregion
}
