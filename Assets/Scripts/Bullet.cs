using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour, IPoolableObject
{
    
    private float damage;
    [SerializeField] float moveSpeed;
    GameObject target;

    [SerializeField] float fireRange = 150f;

    [Header("VFX")]
    [SerializeField] TrailRenderer trailFX;

    public void OnObjectPooled()
    {
        // targetPos = target.transform.position;
    }

    void Update()
    {
        if (target != null && target.activeSelf)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

            if (transform.position.z <= target.transform.position.z - 5)
            {
                gameObject.SetActive(false);
            }

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

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    public void SetTrailColor(Color startColor, Color endColor)
    {

        trailFX.startColor = startColor;
        trailFX.endColor = endColor;

    }
}
