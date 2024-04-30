using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyStats : MonoBehaviour, IDamagable, IPoolableObject
{

    GameManager gameManager;

    public EnemySO EnemySO;
    public bool canMove = true;
    public float currentHealth;
    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] float maxHealth;
    [HideInInspector] public bool isBoss;

    public float currentDamage;
    private int moneyValue;
    private float powerUpAddOnValue;
    public bool isLockedToPlayer;

    [Header("Hit FX")]

    [SerializeField] ParticleSystem hitFX;

    [Header("Animation")]
    [SerializeField] float deathAnimDur;
    public bool isDead;
    Animator animator;
    float knockbackDur;

    [Header("Floating Text")]
    [SerializeField] Transform floatingTextTransform;

    [Header("Health Bar")]
    [SerializeField] Slider healthBar;
    [SerializeField] List<Image> healthBarImages;

    private void Start()
    {
        gameManager = GameManager.instance;

        // testing
        OnObjectPooled();
    }

    public void OnObjectPooled()
    {



        //    GetComponent<EnemyMovement>().StartMovement();
        //   animator = GetComponent<Animator>();

        GetEnemySOValues();
        // UpdateHealthBar();
        //  ResetEnemy();

    }

    private void GetEnemySOValues()
    {
        powerUpAddOnValue = EnemySO.GetPowerUpAddOnValue();
        knockbackDur = EnemySO.GetKnockbackDur();
        currentMoveSpeed = EnemySO.GetMoveSpeed();
        currentDamage = EnemySO.GetDamage();
        moneyValue = EnemySO.GetMoneyValue();
        maxHealth = EnemySO.GetMaxHealth();

        currentHealth = maxHealth;
    }

    private void ResetEnemy()
    {
        // kesin değişecek
        animator.SetBool("isMoving", true);
        animator.SetBool("isDead", false);
        animator.SetBool("isDead", false);

        animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 1);

        canMove = true;
        isDead = false;
        isLockedToPlayer = false;
        transform.forward = Vector3.forward;

        gameObject.layer = LayerMask.NameToLayer("Zombie");
    }


    #region  Damage & Death
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;


        UpdateHealthBar();

        canMove = false;

        //   if (!isBoss)
        // {
        //   animator.SetBool("isMoving", false);
        // animator.SetBool("hitTaken", true);
        // animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 0);
        // }


        StartCoroutine(EnableMovement());

        if (hitFX)
        {
            if (hitFX.isPlaying)
            {
                hitFX.Stop();
                hitFX.Play();
            }
            else
            {
                hitFX.Play();
            }
        }

        if (currentHealth <= 0)
        {
            Kill();
        }


    }
    IEnumerator EnableMovement()
    {

        yield return new WaitForSeconds(knockbackDur);

        if (isDead) yield return null;

        /* if (!isBoss)
         {
             animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 1);
             animator.SetBool("isMoving", true);
             animator.SetBool("hitTaken", false);
         } */

        canMove = true;

    }

    private void Kill()
    {
        isDead = true;
        EnemySpawner EnemySpawner = FindObjectOfType<EnemySpawner>();
        EnemySpawner.OnEnemyKilled();

        healthBar.gameObject.SetActive(false);

        // para kazanma
        //player.IncrementMoney(moneyValue);

        //    if (!gameManager.isPowerEnabled)
        //  {
        //    player.IncrementPowerUpValue(powerUpAddOnValue);
        // }
        // power up sistemi böyle olmayacak tabi

        GameObject spawnedFloatingText = ObjectPooler.instance.SpawnFromPool("Floating Text", floatingTextTransform.position);

        spawnedFloatingText.GetComponent<FloatingTextAnimation>().SetText("$" + moneyValue.ToString());

        gameObject.layer = LayerMask.NameToLayer("DeadZombie");

        PlayDeathAnimation();

        gameManager.allSpawnedEnemies.Remove(gameObject);
    }

    private void UpdateHealthBar()
    {

        if (!healthBar) return;
        healthBar.gameObject.SetActive(true);

        if (currentHealth < maxHealth)
        {
            for (int i = 0; i < healthBarImages.Count; i++)
            {
                healthBarImages[i].DOFade(255f, 0);
            }

            healthBar.value = currentHealth / maxHealth;

            if (isBoss) return;

            for (int i = 0; i < healthBarImages.Count; i++)
            {
                healthBarImages[i].DOFade(0, 1);
            }
        }


    }

    #endregion

    #region  Collisions

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out PlayerStats playerStats))
        {
            // player.KillPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerStats playerStats))
        {
            //   player.KillPlayer();
        }
    }

    #endregion

    #region  Animations
    public void PlayDeathAnimation()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isDead", true);
        animator.SetBool("hitTaken", false);
        animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 0);

        canMove = false;
        StartCoroutine(DestroyObject());
    }

    public void PlayLevelEndAnimation()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isDead", false);
        animator.SetBool("hitTaken", false);
        animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 0);
        canMove = false;
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(deathAnimDur);
        gameObject.SetActive(false);
    }

    #endregion

    #region Getters


    #endregion

}
