using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Vector2 result;
    private Transform joystick;
    const float JOYSTICKALPHA = 0.48f;
    const float JOYSTICKALPHABACKGROUND = 0.12f;
    [SerializeField] private Image joystickImage;
    [SerializeField] private Image joystickImageBackground;
    [SerializeField] private float joystickAlphaMultiplier = 2f;

    private RectTransform rect;
    private Canvas canvas;
    public bool hasJoystickInput;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        joystick = transform.GetChild(0);
    }
    private void Update(){
        // Changes joystick transparency based on input
        if(!hasJoystickInput){
            // create new alpha values
            float newAlpha = joystickImage.color.a - Time.deltaTime * joystickAlphaMultiplier * JOYSTICKALPHA;
            float newAlphaBackground = joystickImageBackground.color.a - Time.deltaTime * joystickAlphaMultiplier * JOYSTICKALPHABACKGROUND;
            // if its higher than 0 set that as the new alpha value
            if(newAlpha > 0){
                joystickImage.color = new Color(joystickImage.color.r, joystickImage.color.g, joystickImage.color.b, newAlpha);
                joystickImageBackground.color = new Color(joystickImageBackground.color.r, joystickImageBackground.color.g, joystickImageBackground.color.b, newAlphaBackground);
            }
            // if alpha is 0, dont set it again.
            else if(joystickImage.color.a == 0){}
            // if its lower than 0, set it to 0. needs to run once
            else if(newAlpha < 0){
                joystickImage.color = new Color(joystickImage.color.r, joystickImage.color.g, joystickImage.color.b, 0);
                joystickImageBackground.color = new Color(joystickImageBackground.color.r, joystickImageBackground.color.g, joystickImageBackground.color.b, 0);
            }
        }
        else{
             // create new alpha values
            float newAlpha = joystickImage.color.a + Time.deltaTime * joystickAlphaMultiplier * JOYSTICKALPHA;
            float newAlphaBackground = joystickImageBackground.color.a + Time.deltaTime * joystickAlphaMultiplier * JOYSTICKALPHABACKGROUND;
            // if new alpha is lower than limit, set new alpha as current alpha value
            if(newAlpha < JOYSTICKALPHA){
                joystickImage.color = new Color(joystickImage.color.r, joystickImage.color.g, joystickImage.color.b, newAlpha);
                joystickImageBackground.color = new Color(joystickImageBackground.color.r, joystickImageBackground.color.g, joystickImageBackground.color.b, newAlphaBackground);
            }
            // if alpha is at limit, dont set it again
            else if(joystickImage.color.a == JOYSTICKALPHA){}
            // if new alpha is higher than limit, set it to limit. needs to run once 
            else if(newAlpha > JOYSTICKALPHA){
                joystickImage.color = new Color(joystickImage.color.r, joystickImage.color.g, joystickImage.color.b, JOYSTICKALPHA);
                joystickImageBackground.color = new Color(joystickImageBackground.color.r, joystickImageBackground.color.g, joystickImageBackground.color.b, JOYSTICKALPHABACKGROUND);
            }
        }
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
        hasJoystickInput = true;
    }
    public void ResetJoy()
    {
        result = Vector2.zero;
        joystick.localPosition = Vector2.zero;
        hasJoystickInput = false;
    }
}
