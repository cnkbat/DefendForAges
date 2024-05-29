using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CityChangingAnimationHandler : MonoBehaviour
{
    CityChangeSequenceTrigger cityChangeSequenceTrigger;

    [Header("Animation Type")]
    [SerializeField] private AnimationTypes animationType = AnimationTypes.dropDown;

    [Header("Basics")]
    [SerializeField] private List<Transform> objectsToEnable;
    [SerializeField] private List<Transform> objectsToDisable;

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
        cityChangeSequenceTrigger = transform.GetComponent<CityChangeSequenceTrigger>();
        cityChangeSequenceTrigger.OnSequencetTriggered += PlaySpawningAnimation;
    }

    private void OnDisable()
    {
        cityChangeSequenceTrigger.OnSequencetTriggered -= PlaySpawningAnimation;
    }

    public void PlaySpawningAnimation()
    {
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
