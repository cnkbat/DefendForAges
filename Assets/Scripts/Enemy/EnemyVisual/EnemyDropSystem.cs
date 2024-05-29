using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyDropSystem : MonoBehaviour
{
    ObjectPooler objectPooler;
    GameManager gameManager;
    private EnemyBehaviour enemyBehaviour;

    private void OnEnable()
    {
        objectPooler = ObjectPooler.instance;
        gameManager = GameManager.instance;

        enemyBehaviour = transform.GetComponent<EnemyBehaviour>();

        enemyBehaviour.OnDropAnimNeeded += PlayDropAnim;
    }

    private void OnDisable()
    {
        enemyBehaviour.OnDropAnimNeeded -= PlayDropAnim;
    }


    // REFACTOR
    public void PlayDropAnim(int dropTypeIndex)
    {
        string dropTag = GetDropTag(dropTypeIndex);
        if(dropTag.Equals(null))
        {
            Debug.Log("No Tag");
            return;
        }
        GameObject spawnedObject = objectPooler.SpawnFromPool(dropTag, enemyBehaviour.transform.position);
        CurrencyAnimationHandler currencyAnimationHandler = spawnedObject.GetComponent<CurrencyAnimationHandler>();
        currencyAnimationHandler.PlayDropAnim();
        gameManager.droppedCurrencies.Add(currencyAnimationHandler);
    }
    private string GetDropTag(int dropTypeIndex)
    {
        switch (dropTypeIndex)
        {
            case (int)DropTypes.coin:
                return "Coin";
            case (int)DropTypes.meat:
                return "Meat";
            case (int)DropTypes.experiencePoint:
                return "XP";
            default:
                return null;
        }
    }

}
