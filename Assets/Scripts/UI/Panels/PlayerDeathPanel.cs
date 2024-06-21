using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeathPanel : PanelBase
{

    [Header("Revive")]
    [SerializeField] TMP_Text playerKilledCountDownText;
    [SerializeField] private Button lateReviveButton;
    [SerializeField] private Button rewardedReviveButton;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetButtonActivity(true);

        // Revive & Wave Control
        lateReviveButton.onClick.AddListener(RevivePlayer);
        rewardedReviveButton.onClick.AddListener(RewardedReviveButtonPressed);
    }

    private void OnDisable()
    {
        lateReviveButton.onClick.RemoveAllListeners();
        rewardedReviveButton.onClick.RemoveAllListeners();
    }


    #region Revive

    public void RewardedReviveButtonPressed()
    {
        if (playerStats.DecrementGem(gameManager.reviveTowerCost))
        {
            gameManager.isGameFreezed = false;

            for (int i = 0; i < Mathf.RoundToInt(gameManager.allSpawnedEnemies.Count / 2); i++)
            {
                gameManager.allSpawnedEnemies[i].Kill();
            }

            SetButtonActivity(false);
            RevivePlayer();
        }
        else
        {
            // popup offer
        }

    }


    private void RevivePlayer()
    {
        playerStats.OnRevivePlayer?.Invoke();
        SetButtonActivity(false);
    }


    private void SetButtonActivity(bool value)
    {
        lateReviveButton.gameObject.SetActive(value);
        rewardedReviveButton.gameObject.SetActive(value);
    }

    #endregion

    #region  Death Panel Management Helper

    public void DeathReviveSequence()
    {
        lateReviveButton.gameObject.SetActive(false);

        StartCoroutine(PlayerDeathCountDown());
    }

    IEnumerator PlayerDeathCountDown()
    {
        if (!gameObject.activeSelf) yield return null;

        playerKilledCountDownText.text = "5";
        yield return new WaitForSeconds(1);

        if (!gameObject.activeSelf) yield return null;

        playerKilledCountDownText.text = "4";
        yield return new WaitForSeconds(1);

        if (!gameObject.activeSelf) yield return null;

        playerKilledCountDownText.text = "3";
        yield return new WaitForSeconds(1);

        if (!gameObject.activeSelf) yield return null;

        playerKilledCountDownText.text = "2";
        yield return new WaitForSeconds(1);

        if (!gameObject.activeSelf) yield return null;

        playerKilledCountDownText.text = "1";
        yield return new WaitForSeconds(1);

        if (!gameObject.activeSelf) yield return null;

        lateReviveButton.gameObject.SetActive(true);
        playerKilledCountDownText.gameObject.SetActive(false);
    }


    #endregion

}
