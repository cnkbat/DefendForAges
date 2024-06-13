using UnityEngine;

public class PanelBase : MonoBehaviour
{
    protected PlayerStats playerStats;
    protected GameManager gameManager;
    protected UIManager uiManager;

    protected virtual void OnEnable()
    {
        playerStats = PlayerStats.instance;
        gameManager = GameManager.instance;
        uiManager = UIManager.instance;
    }

}
