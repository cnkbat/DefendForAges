using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class SpawnAnimationHandler : MonoBehaviour
{
    [Header("Animation Type")]
    [SerializeField] AnimationTypes animationType = AnimationTypes.dropDown;

    [Header("Basics")]
    [Tooltip("Eklenecek olan objeler")][SerializeField] List<Transform> objectsToAnimateOnSpawn;
    [Tooltip("Kaldırılacak olan objeler")][SerializeField] List<Transform> objectsToAnimateOnDespawn;

    [Header("-----------------")]

    [Header("Drop Down Settings")]
    [SerializeField] private float flyUpValue;
    [SerializeField] private float dropDownAnimationTime;
    [SerializeField] Ease dropDownEaseType = Ease.Linear;


    [Header("-----------------")]

    [Header("Small To Big Settings")]
    [SerializeField] private float smallToBigAnimationTime;
    [SerializeField] Ease smallToBigEaseType = Ease.Linear;

    [Header("Events")]
    public Action OnAnimPlay;

    private void OnEnable()
    {
        OnAnimPlay += PlaySpawningAnimation;
    }

    private void OnDisable()
    {
        OnAnimPlay -= PlaySpawningAnimation;
    }

    public void PlaySpawningAnimation()
    {
        Debug.Log("play anim");
        if (animationType == AnimationTypes.dropDown)
        {
            for (int i = 0; i < objectsToAnimateOnSpawn.Count; i++)
            {
                PlayDropDownAnimation(objectsToAnimateOnSpawn[i]);
            }

            for (int i = 0; i < objectsToAnimateOnDespawn.Count; i++)
            {
                PlayDropDownDespawnAnimation(objectsToAnimateOnDespawn[i]);
            }

        }
        else if (animationType == AnimationTypes.smallToBig)
        {
            for (int i = 0; i < objectsToAnimateOnSpawn.Count; i++)
            {
                PlaySmallToBigAnimation(objectsToAnimateOnSpawn[i]);
            }

            for (int i = 0; i < objectsToAnimateOnDespawn.Count; i++)
            {
                PlaySmallToBigDeSpawnAnimation(objectsToAnimateOnDespawn[i]);
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
