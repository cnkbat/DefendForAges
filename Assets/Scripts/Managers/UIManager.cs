using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [Header("UI Elements")]
    public GameObject reviveUI;
    [SerializeField] Button reviveButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        reviveButton.onClick.AddListener(OnReviveButtonPressed);
    }

    public void HandleReviveUI()
    {
        if (reviveUI.activeSelf)
        {
            reviveUI.SetActive(false);
        }
        else
        {
            reviveUI.SetActive(true);
        }
    }

    private void OnReviveButtonPressed()
    {
        Debug.Log("revived");
    }
}
