using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveWonPanel : PanelBase
{
    EarningsHolder earningsHolder;

    bool collected;

    [Header("Earnings")]
    [SerializeField] private Button normalApplyEarningsButton;
    [SerializeField] private Button rewardedApplyEarningsButton;
    [SerializeField] private TMP_Text waveWoncollectedXPText;
    [SerializeField] private TMP_Text waveWoncollectedCoinText;
    [SerializeField] private TMP_Text waveWoncollectedMeatText;

    [Header("Current values")]
    int currentCoinValue;
    int currentXPValue;
    int currentMeatValue;


    protected override void OnEnable()
    {
        base.OnEnable();
        earningsHolder = EarningsHolder.instance;

        collected = false;

        rewardedApplyEarningsButton.gameObject.SetActive(true);
        normalApplyEarningsButton.gameObject.SetActive(false);

        StartCoroutine(EnableNormalButton());

        normalApplyEarningsButton.onClick.AddListener(RegularDisableWaveWonUI);
        rewardedApplyEarningsButton.onClick.AddListener(RewardedDisableWaveWonUI);

        // Text Updates
        earningsHolder.OnEarningsApply += ActivateAndUpdateWaveWonEarningsTexts;
    }

    private void OnDisable()
    {
        playerStats.OnWaveWon -= EnableWaveWonUI;

        normalApplyEarningsButton.onClick.RemoveAllListeners();
        rewardedApplyEarningsButton.onClick.RemoveAllListeners();

        // Text Updates
        earningsHolder.OnEarningsApply -= ActivateAndUpdateWaveWonEarningsTexts;
    }

    IEnumerator EnableNormalButton()
    {
        yield return new WaitForSeconds(1.5f);

        if (collected)
        {
            yield return null;
        }
        else
        {
            normalApplyEarningsButton.gameObject.SetActive(true);
        }
    }

    #region Wave Won Management

    public void EnableWaveWonUI()
    {
        waveWoncollectedXPText.transform.parent.gameObject.SetActive(false);
        waveWoncollectedCoinText.transform.parent.gameObject.SetActive(false);
        waveWoncollectedMeatText.transform.parent.gameObject.SetActive(false);
    }
    IEnumerator DisableWaveWonUI()
    {
        yield return new WaitForSeconds(2);

        uiManager.GetBackToGamePanel();
        gameManager.OnApplyEarnings?.Invoke();
    }

    private void ActivateAndUpdateWaveWonEarningsTexts(int meatValue, int coinValue, int xpValue)
    {
        waveWoncollectedXPText.transform.parent.gameObject.SetActive(true);
        waveWoncollectedCoinText.transform.parent.gameObject.SetActive(true);
        waveWoncollectedMeatText.transform.parent.gameObject.SetActive(true);


        StartCoroutine(UpdateTextOverTime(waveWoncollectedXPText, currentXPValue, xpValue));
        StartCoroutine(UpdateTextOverTime(waveWoncollectedCoinText, currentCoinValue, coinValue));
        StartCoroutine(UpdateTextOverTime(waveWoncollectedMeatText, currentMeatValue, meatValue));

    }

    IEnumerator UpdateTextOverTime(TMP_Text text, int currentValue, int targetValue)
    {
        while (currentValue < targetValue)
        {
            currentValue++;
            text.text = currentValue.ToString();
            yield return new WaitForSeconds(1 / (targetValue + 1));
        }
    }

    public void RewardedDisableWaveWonUI()
    {
        collected = true;

        uiManager.OnApplyEarningToPlayerButtonClicked(2);

        normalApplyEarningsButton.gameObject.SetActive(false);
        rewardedApplyEarningsButton.gameObject.SetActive(false);

        StartCoroutine(DisableWaveWonUI());
    }
    public void RegularDisableWaveWonUI()
    {
        collected = true;

        normalApplyEarningsButton.gameObject.SetActive(false);
        rewardedApplyEarningsButton.gameObject.SetActive(false);

        uiManager.OnApplyEarningToPlayerButtonClicked(1);

        StartCoroutine(DisableWaveWonUI());
    }


    #endregion

}
