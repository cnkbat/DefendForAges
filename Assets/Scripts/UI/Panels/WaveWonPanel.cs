using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    [Header("Earning Positions")]
    [SerializeField] private Transform coinEarning;
    [SerializeField] private Transform meatEarning;
    [Header("Earning Destinations")]
    [SerializeField] private Transform coinGameHud;
    [SerializeField] private Transform meatGameHud;


    [SerializeField] private float earningReachDuration;
    [SerializeField] Ease easeType;

    protected override void OnEnable()
    {
        base.OnEnable();
        earningsHolder = EarningsHolder.instance;
        // check this for error for instances

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
        yield return new WaitForSeconds(4);

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

        StartCoroutine(TransferCoinToPlayerAnimation(coinValue));
        StartCoroutine(TransferMeatToPlayerAnimation(meatValue));
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

        normalApplyEarningsButton.gameObject.SetActive(false);
        rewardedApplyEarningsButton.gameObject.SetActive(false);

        uiManager.OnApplyEarningToPlayerButtonClicked(2);

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

    IEnumerator TransferCoinToPlayerAnimation(int coinValue){
        yield return new WaitForSeconds(1.5f);
        // Here starts distribution of rewards
        // for meat and coin only for now.
        // we need a queue of rewards to distribute, as an object pool. pool in UIManager publicly
        // dotween durations will be set on inspector
        int startCoinVal = coinValue;
        for(int i = 0; i < startCoinVal; i++)
        {
            yield return new WaitForSeconds(1/startCoinVal);
            // dequeue coin
            GameObject coin = uiManager.coinPool.Dequeue();
            // remove 1 coin from earning
            currentCoinValue--;
            // move coin to coin earning position
            coin.transform.position = coinEarning.position;
            // coin.setactive(true)
            coin.SetActive(true);
            // dotween coin to coin gamehud
                // on complete add 1 coin to player and queue coin and coin.setactive(false)
            coin.transform.DOMove(coinGameHud.position, earningReachDuration)
            .SetEase(easeType)
            .OnComplete(() => {
                uiManager.coinPool.Enqueue(coin);
                coin.SetActive(false);
                playerStats.IncrementMoney(1);
            });
        }
    }


IEnumerator TransferMeatToPlayerAnimation(int meatValue){
        yield return new WaitForSeconds(1.5f);
        // Here starts distribution of rewards
        // we need a queue of rewards to distribute, as an object pool. pool in UIManager publicly
        // dotween durations will be set on inspector
        int startMeatVal = meatValue;
        for(int i = 0; i < startMeatVal; i++)
        {
            yield return new WaitForSeconds(1/startMeatVal);
            // dequeue meat
            GameObject meat = uiManager.meatPool.Dequeue();
            // remove 1 meat from earning
            currentMeatValue--;
            // move meat to meat earning position
            meat.transform.position = meatEarning.position;
            // meat.setactive(true)
            meat.SetActive(true);
            // dotween meat to meat gamehud
                // on complete add 1 meat to player and queue meat and meat.setactive(false)
            meat.transform.DOMove(meatGameHud.position, earningReachDuration)
            .SetEase(easeType)
            .OnComplete(() =>{
                uiManager.meatPool.Enqueue(meat);
                meat.SetActive(false);
                playerStats.IncrementMeat(1);
            });
        }
    }

    #endregion

}
