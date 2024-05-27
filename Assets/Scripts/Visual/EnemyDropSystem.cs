using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyDropSystem : MonoBehaviour
{
    ObjectPooler objectPooler;
    private EnemyBehaviour enemyBehaviour;

    private void OnEnable()
    {
        objectPooler = ObjectPooler.instance;
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
            objectPooler.SpawnDropTypeFromPool("Dropped Coin", enemyBehaviour.transform.position);
        }
        else if (dropTypeIndex == (int)DropTypes.meat)
        {
            objectPooler.SpawnDropTypeFromPool("Dropped Meat", enemyBehaviour.transform.position);
        }
        else if (dropTypeIndex == (int)DropTypes.experiencePoint)
        {
            objectPooler.SpawnDropTypeFromPool("Dropped XP", enemyBehaviour.transform.position);
        }
    }

}
