using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class FloatingTextAnimation : MonoBehaviour, IPoolableObject
{
    [SerializeField] TMP_Text coinText;

    public void OnObjectPooled()
    {
        
        //transform.DORotate(new Vector3(30, 0, 0), 0.1f);
        // rotate açı için

        transform.DOMoveY(transform.position.y + 2, 1f).SetEase(Ease.OutBounce);

        StartCoroutine(Deactivate());

    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }

    public void SetText(string newText)
    {
        coinText.text = newText;
    }

    public void ResetObjectData()
    {
        throw new System.NotImplementedException();
    }
}
