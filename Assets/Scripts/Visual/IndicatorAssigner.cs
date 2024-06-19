using System;
using System.Collections;
using System.Collections.Generic;
using HUDIndicator;
using UnityEngine;

public class IndicatorAssigner : MonoBehaviour
{
    IndicatorOffScreen indicator;

    [Header("Textures")]
    [SerializeField] private Texture homeTexture;
    [SerializeField] private Texture repairTexture;
    [SerializeField] private Texture enemyTexture;
    [SerializeField] private Texture bossTexture;

    [Header("Actions")]
    public Action<PointableTypes, Color> OnEnableIndicator;
    public Action OnDisableIndicator;

    private void Awake()
    {
        indicator = GetComponent<IndicatorOffScreen>();
    }

    private void OnEnable()
    {
        OnEnableIndicator += EnableIndicator;
        OnDisableIndicator += DisableIndicator;
    }

    private void OnDisable()
    {
        OnEnableIndicator += EnableIndicator;
        OnDisableIndicator += DisableIndicator;
    }

    private void EnableIndicator(PointableTypes pointableType, Color arrowColor)
    {

        if (pointableType == PointableTypes.enemy)
        {
            indicator.style.texture = enemyTexture;
        }
        else if (pointableType == PointableTypes.repair)
        {
            indicator.style.texture = repairTexture;
        }
        else if (pointableType == PointableTypes.boss)
        {
            indicator.style.texture = bossTexture;
        }
        else if (pointableType == PointableTypes.home)
        {
            indicator.style.texture = homeTexture;
        }

        indicator.arrowStyle.color = arrowColor;
        indicator.visible = true;
    }

    private void DisableIndicator()
    {
        indicator.visible = false;
    }
}