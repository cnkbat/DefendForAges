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
        // Revive & Wave Control
        lateReviveButton.onClick.AddListener(OnLateReviveButtonClicked);
        rewardedReviveButton.onClick.AddListener(OnReviveButtonClicked);
    }

    private void OnDisable()
    {
        lateReviveButton.onClick.RemoveAllListeners();
        rewardedReviveButton.onClick.RemoveAllListeners();
    }


    #region Revive

    private void OnLateReviveButtonClicked()
    {
        playerStats.OnLateReviveButtonClicked?.Invoke();
        playerStats.OnPlayerRevived?.Invoke();
    }
    private void OnReviveButtonClicked()
    {
        playerStats.OnReviveButtonClicked?.Invoke();
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
