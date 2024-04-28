using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Vector2 result;
    private Transform joystick;

    private RectTransform rect;
    private Canvas canvas;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        joystick = transform.GetChild(0);
    }

    public void OnPointerDown(PointerEventData ped)
    {
        ChangeJoy(ped.position);
    }
    public void OnDrag(PointerEventData ped)
    {
        ChangeJoy(ped.position);
    }
    public void OnPointerUp(PointerEventData ped)
    {
        ResetJoy();
    }
    public void ChangeJoy(Vector2 pedPos)
    {
        Vector2 diff = pedPos - (Vector2)rect.position;
        Vector2 modDiff = diff * (1f / canvas.scaleFactor);
        modDiff /= rect.sizeDelta * 0.5f;
        result = Vector2.ClampMagnitude(modDiff, 1f);
        modDiff = result * rect.sizeDelta * 0.3f;
        joystick.localPosition = modDiff;
    }
    public void ResetJoy()
    {
        result = Vector2.zero;
        joystick.localPosition = Vector2.zero;
    }
}
