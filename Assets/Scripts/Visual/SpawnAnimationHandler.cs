using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class SpawnAnimationHandler : MonoBehaviour
{
    private BuyableArea buyableArea;
    [Header("Animation Type")]
    [SerializeField] private AnimationTypes animationType = AnimationTypes.dropDown;

    [Header("Basics")]


    [Header("-----------------")]

    [Header("Drop Down Settings")]
    [SerializeField] private float flyUpValue;
    [SerializeField] private float dropDownAnimationTime;
    [SerializeField] private Ease dropDownEaseType = Ease.Linear;

    [Header("-----------------")]

    [Header("Small To Big Settings")]
    [SerializeField] private float smallToBigAnimationTime;
    [SerializeField] private Ease smallToBigEaseType = Ease.Linear;



    private void OnEnable()
    {
        buyableArea = transform.parent.GetComponent<BuyableArea>();
        buyableArea.OnAnimPlayNeeded += PlaySpawningAnimation;
    }

    private void OnDisable()
    {
        buyableArea.OnAnimPlayNeeded -= PlaySpawningAnimation;
    }

    public void PlaySpawningAnimation(List<Transform> objectsToEnable, List<Transform> objectsToDisable, List<Transform> spawnPosesToDisable)
    {
        for (int i = 0; i < spawnPosesToDisable.Count; i++)
        {
            spawnPosesToDisable[i].gameObject.SetActive(false);
        }

        if (animationType == AnimationTypes.dropDown)
        {
            for (int i = 0; i < objectsToEnable.Count; i++)
            {
                PlayDropDownAnimation(objectsToEnable[i]);
            }

            for (int i = 0; i < objectsToDisable.Count; i++)
            {
                PlayDropDownDespawnAnimation(objectsToDisable[i]);
            }

        }
        else if (animationType == AnimationTypes.smallToBig)
        {
            for (int i = 0; i < objectsToEnable.Count; i++)
            {
                PlaySmallToBigAnimation(objectsToEnable[i]);
            }

            for (int i = 0; i < objectsToDisable.Count; i++)
            {
                PlaySmallToBigDeSpawnAnimation(objectsToDisable[i]);
            }

        }
    }

    #region Small to Big
    private void PlaySmallToBigAnimation(Transform animatedObject)
    {
        Vector3 localScale = animatedObject.localScale;
        animatedObject.localScale = Vector3.zero;

        animatedObject.DOScale(localScale, smallToBigAnimationTime).SetEase(smallToBigEaseType);
    }

    private void PlaySmallToBigDeSpawnAnimation(Transform removedObject)
    {
        removedObject.DOScale(0, smallToBigAnimationTime).SetEase(smallToBigEaseType).OnComplete(() => removedObject.gameObject.SetActive(false));
    }
    #endregion

    #region Drop Down

    private void PlayDropDownAnimation(Transform animatedObject)
    {
        animatedObject.gameObject.SetActive(true);

        Vector3 localPos = animatedObject.localPosition;
        Vector3 localScale = animatedObject.localScale;

        animatedObject.localPosition += animatedObject.up * flyUpValue;
        animatedObject.localScale = Vector3.zero;

        animatedObject.DOLocalMove(localPos, dropDownAnimationTime).SetEase(dropDownEaseType).
            OnStart(() => animatedObject.DOScale(localScale, dropDownAnimationTime));
    }

    private void PlayDropDownDespawnAnimation(Transform removedObject)
    {
        removedObject.DOLocalMoveY(flyUpValue, dropDownAnimationTime).SetEase(dropDownEaseType).
         OnStart(() => removedObject.DOScale(Vector3.zero, dropDownAnimationTime)).OnComplete(() => removedObject.gameObject.SetActive(false));
    }

    #endregion
}
