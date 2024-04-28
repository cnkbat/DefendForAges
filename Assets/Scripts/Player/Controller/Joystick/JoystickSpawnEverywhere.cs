using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickSpawnEverywhere : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Joystick joystick;
    private Transform joyBG;
    private Vector3 joyBGFirst;

    private RectTransform rect;
    private Canvas canvas;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        joyBG = joystick.transform;

        joyBGFirst = joyBG.localPosition;
    }
    public void OnPointerDown(PointerEventData ped)
    {
        Vector2 diff = ped.position - (Vector2)rect.position;
        Vector2 modDiff = diff * (1f / canvas.scaleFactor);
        joyBG.localPosition = modDiff;
    }
    public void OnDrag(PointerEventData ped)
    {
        joystick.ChangeJoy(ped.position);
    }
    public void OnPointerUp(PointerEventData ped)
    {
        joyBG.localPosition = joyBGFirst;
        joystick.ResetJoy();
    }
}
