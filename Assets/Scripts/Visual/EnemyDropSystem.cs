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

        enemyBehaviour = transform.parent.GetComponent<EnemyBehaviour>();

        enemyBehaviour.OnDropAnimNeeded += PlayDropAnim;
    }

    private void OnDisable()
    {
        enemyBehaviour.OnDropAnimNeeded -= PlayDropAnim;
    }

    public void PlayDropAnim(int dropTypeIndex)
    {
        if (dropTypeIndex == (int)DropTypes.coin)
        {

            GameObject spawnedObject = objectPooler.SpawnFromPool("Coin", enemyBehaviour.transform.position);

            CurrencyAnimationHandler currencyAnimationHandler = spawnedObject.GetComponent<CurrencyAnimationHandler>();
            currencyAnimationHandler.PlayDropAnim();
            gameManager.droppedCurrencies.Add(currencyAnimationHandler);

        }
        else if (dropTypeIndex == (int)DropTypes.meat)
        {

            GameObject spawnedObject = objectPooler.SpawnFromPool("Meat", enemyBehaviour.transform.position);

            CurrencyAnimationHandler currencyAnimationHandler = spawnedObject.GetComponent<CurrencyAnimationHandler>();
            currencyAnimationHandler.PlayDropAnim();
            gameManager.droppedCurrencies.Add(currencyAnimationHandler);


        }
        else if (dropTypeIndex == (int)DropTypes.experiencePoint)
        {

            GameObject spawnedObject = objectPooler.SpawnFromPool("XP", enemyBehaviour.transform.position);

            CurrencyAnimationHandler currencyAnimationHandler = spawnedObject.GetComponent<CurrencyAnimationHandler>();
            currencyAnimationHandler.PlayDropAnim();
            gameManager.droppedCurrencies.Add(currencyAnimationHandler);

        }
    }

}
