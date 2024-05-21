using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class FloatingTextAnimation : MonoBehaviour, IPoolableObject
{
    [SerializeField] private TMP_Text floatingText;

    [Header("Anim Related")]
    [Tooltip("Anim timedan her zaman fazla olmalı.")][SerializeField] private float stayAliveTime;
    [SerializeField] private float animationTime;
    [SerializeField] private Ease easeType;

    [Header("Movement")]
    [SerializeField] private float clampValue;
    [SerializeField] private float height = 5;

    public void OnObjectPooled()
    {
        transform.position += Vector3.up * height;

        Vector3 randVector = Vector3.one * Random.Range(-clampValue, clampValue);

        transform.DOMoveX(transform.position.x + randVector.x, animationTime).SetEase(easeType);
        transform.DOMoveY(transform.position.y + Mathf.Abs(randVector.y), animationTime).SetEase(easeType);

        StartCoroutine(Deactivate());

    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(stayAliveTime);
        gameObject.SetActive(false);
    }

    public void SetText(string newText)
    {
        floatingText.text = newText;
    }
    
    public void SetFontSize(float value)
    {
        floatingText.fontSize = value;
    }

    public void SetTextColor(Color color)
    {
        floatingText.color = color;
    }

    public void ResetObjectData()
    {
        // no code
    }
}
