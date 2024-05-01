using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    //  saldırı, yürüme , ölme
    EnemyStats enemyStats;
    GameManager gameManager;
    EnemyTargeter enemyTargeter;

    public float attackDur;
    public bool isDead;
    public bool canMove;
    Animator animator;
    [SerializeField] float deathAnimDur;
    public void Start()
    {
        attackDur = 1;
        canMove = true;
        isDead = false;
        // sonra silinecekler ^^

        enemyTargeter = this.GetComponent<EnemyTargeter>();
        enemyStats = this.GetComponent<EnemyStats>();
        gameManager = GameManager.instance;
    }
    public void Update()
    {
        if (!isDead)
        {
            if (canMove && enemyTargeter.GetTarget()!= null) Move();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.Equals(enemyTargeter.GetTarget()))
        {
            Attack();
        }   
    }
    private void ResetEnemy()
    {
        // kesin değişecek
        /*  animator.SetBool("isMoving", true);
          animator.SetBool("isDead", false);
          animator.SetBool("isDead", false);

          animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 1);

          canMove = true;
          isDead = false;
          transform.forward = Vector3.forward;

          gameObject.layer = LayerMask.NameToLayer("Zombie"); */
    }
    public void TakeDamage()
    {
        canMove = false;
        StartCoroutine(EnableMovement());
    }

    IEnumerator EnableMovement()
    {

        yield return new WaitForSeconds(attackDur);

        if (isDead) yield return null;

        /* if (!isBoss)
         {
             animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 1);
             animator.SetBool("isMoving", true);
             animator.SetBool("hitTaken", false);
         } */

        canMove = true;

    }

    #region States
    public void Attack()
    {
        canMove = false;
        // play animation
        // deal damage

        StartCoroutine(EnableMovement());
    }
    public void Move()
    {
        Vector3 targetPosition = enemyTargeter.GetTarget().position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, enemyStats.currentMoveSpeed * Time.deltaTime);
    }
    public void Kill()
    {
        isDead = true;
        EnemySpawner EnemySpawner = FindObjectOfType<EnemySpawner>();
        EnemySpawner.OnEnemyKilled();

        enemyStats.getHealthBar().gameObject.SetActive(false);

        // para kazanma
        //player.IncrementMoney(moneyValue);
        //    if (!gameManager.isPowerEnabled)
        //  {
        //    player.IncrementPowerUpValue(powerUpAddOnValue);
        // }
        // power up sistemi böyle olmayacak tabi
        // GameObject spawnedFloatingText = ObjectPooler.instance.SpawnFromPool("Floating Text", floatingTextTransform.position);
        // spawnedFloatingText.GetComponent<FloatingTextAnimation>().SetText("$" + moneyValue.ToString());

        gameObject.layer = LayerMask.NameToLayer("DeadZombie");
        PlayDeathAnimation();

        gameManager.allSpawnedEnemies.Remove(gameObject);
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

}
