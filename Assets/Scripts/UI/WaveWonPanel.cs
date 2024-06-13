using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveWonPanel : PanelBase
{
    EarningsHolder earningsHolder;

    [Header("Earnings")]
    [SerializeField] private Button normalApplyEarningsButton;
    [SerializeField] private Button rewardedApplyEarningsButton;
    [SerializeField] private GameObject waveWonOpenedChestIcon;
    [SerializeField] private GameObject waveWonClosedChestIcon;
    [SerializeField] private TMP_Text waveWoncollectedXPText;
    [SerializeField] private TMP_Text waveWoncollectedCoinText;
    [SerializeField] private TMP_Text waveWoncollectedMeatText;

    protected override void OnEnable()
    {
        base.OnEnable();
        earningsHolder = EarningsHolder.instance;

       

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

    #region Wave Won Management

    public void EnableWaveWonUI()
    {
        waveWonOpenedChestIcon.SetActive(false);
        waveWonClosedChestIcon.SetActive(true);

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
        waveWonOpenedChestIcon.SetActive(true);
        waveWonClosedChestIcon.SetActive(false);

        waveWoncollectedXPText.transform.parent.gameObject.SetActive(true);
        waveWoncollectedCoinText.transform.parent.gameObject.SetActive(true);
        waveWoncollectedMeatText.transform.parent.gameObject.SetActive(true);


        StartCoroutine(UpdateTextOverTime(waveWoncollectedXPText, xpValue));
        StartCoroutine(UpdateTextOverTime(waveWoncollectedCoinText, coinValue));
        StartCoroutine(UpdateTextOverTime(waveWoncollectedMeatText, meatValue));


        //waveWoncollectedXPText.text = xpValue.ToString();
        //waveWoncollectedCoinText.text = coinValue.ToString();
        //waveWoncollectedMeatText.text = meatValue.ToString();

    }

    IEnumerator UpdateTextOverTime(TMP_Text text, int val)
    {
        for (int i = 0; i < val; i++)
        {
            text.text = i + "";
            yield return new WaitForSeconds(2 / (val + 1));
        }
    }

    public void RewardedDisableWaveWonUI()
    {
        uiManager.OnApplyEarningToPlayerButtonClicked(2);

        StartCoroutine(DisableWaveWonUI());
    }
    public void RegularDisableWaveWonUI()
    {
        uiManager.OnApplyEarningToPlayerButtonClicked(1);

        StartCoroutine(DisableWaveWonUI());
    }



    #endregion

}
