using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerDeathPanel : PanelBase
{
    EarningsHolder earningsHolder;
    [Header("Game Lost Sequence")]
    [SerializeField] private GameObject gameLostOpenedChestIcon;
    [SerializeField] private GameObject gameLostClosedChestIcon;
    [SerializeField] private TMP_Text gameLostCollectedXPText;
    [SerializeField] private TMP_Text gameLostCollectedMeatText;
    [SerializeField] private TMP_Text gameLostCollectedCoinText;
    [SerializeField] private TMP_Text towerDestroyedCountDownText;
    [SerializeField] private Button loseGameButton;
    [SerializeField] private Button useGemToReviveTowerButton;


    int currentCoinValue;
    int currentXPValue;
    int currentMeatValue;


    protected override void OnEnable()
    {
        base.OnEnable();
        earningsHolder = EarningsHolder.instance;

        useGemToReviveTowerButton.onClick.AddListener(ReviveTowerButtonPressed);
        loseGameButton.onClick.AddListener(GameLostPanelSequenceEnd);
        earningsHolder.OnEarningsApply += ActivateAndUpdateTowerDeathEarningsTexts;

    }

    private void OnDisable()
    {
        loseGameButton.onClick.RemoveAllListeners();
        useGemToReviveTowerButton.onClick.RemoveAllListeners();

        earningsHolder.OnEarningsApply -= ActivateAndUpdateTowerDeathEarningsTexts;
    }

    public void ReviveTowerButtonPressed()
    {

        if (playerStats.DecrementGem(gameManager.reviveTowerCost))
        {
            loseGameButton.gameObject.SetActive(false);
            useGemToReviveTowerButton.gameObject.SetActive(false);
            gameManager.isGameFreezed = false;
            gameManager.allCities[playerStats.GetCityIndex()].GetTower().ReviveTarget();
        }
        else
        {
            // popup offer
        }

    }


    #region  Lose Panel Management
    public void GameLostPanelSequence()
    {

        gameManager.isGameFreezed = true;

        loseGameButton.gameObject.SetActive(false);

        gameLostOpenedChestIcon.SetActive(false);
        gameLostClosedChestIcon.SetActive(true);

        gameLostCollectedXPText.transform.parent.gameObject.SetActive(false);
        gameLostCollectedCoinText.transform.parent.gameObject.SetActive(false);
        gameLostCollectedMeatText.transform.parent.gameObject.SetActive(false);


        StartCoroutine(TowerDeathCountDown());
    }

    IEnumerator TowerDeathCountDown()
    {
        // game lose screen yok     if (!.activeSelf) yield return null;

        towerDestroyedCountDownText.text = "3";
        yield return new WaitForSeconds(1);

        // game lose screen yok     if (!.activeSelf) yield return null;

        towerDestroyedCountDownText.text = "2";
        yield return new WaitForSeconds(1);

        // game lose screen yok     if (!.activeSelf) yield return null;

        towerDestroyedCountDownText.text = "1";
        yield return new WaitForSeconds(1);

        // game lose screen yok    if (!.activeSelf) yield return null;

        loseGameButton.gameObject.SetActive(true);
        towerDestroyedCountDownText.gameObject.SetActive(false);
    }

    private void GameLostPanelSequenceEnd()
    {
        StartCoroutine(DisableGameLostUI());

        uiManager.OnApplyEarningToPlayerButtonClicked(1);

        loseGameButton.gameObject.SetActive(false);
        useGemToReviveTowerButton.gameObject.SetActive(false);

        gameLostOpenedChestIcon.SetActive(true);
        gameLostClosedChestIcon.SetActive(false);

        gameLostCollectedXPText.transform.parent.gameObject.SetActive(true);
        gameLostCollectedCoinText.transform.parent.gameObject.SetActive(true);
        gameLostCollectedMeatText.transform.parent.gameObject.SetActive(true);
    }

    IEnumerator DisableGameLostUI()
    {
        yield return new WaitForSeconds(2);

        // uiManager.GetBackToGamePanel();
        gameManager.LevelLost();
    }

    private void ActivateAndUpdateTowerDeathEarningsTexts(int meatValue, int coinValue, int xpValue)
    {

        gameLostOpenedChestIcon.SetActive(true);
        gameLostClosedChestIcon.SetActive(false);


        gameLostCollectedXPText.transform.parent.gameObject.SetActive(true);
        gameLostCollectedCoinText.transform.parent.gameObject.SetActive(true);
        gameLostCollectedMeatText.transform.parent.gameObject.SetActive(true);

        StartCoroutine(UpdateTextOverTime(gameLostCollectedXPText, currentXPValue, xpValue));
        StartCoroutine(UpdateTextOverTime(gameLostCollectedCoinText, currentCoinValue, coinValue));
        StartCoroutine(UpdateTextOverTime(gameLostCollectedMeatText, currentMeatValue, meatValue));

    }


    IEnumerator UpdateTextOverTime(TMP_Text text, int currentValue, int targetValue)
    {
        Debug.Log("target value = " + targetValue);
        while (currentValue < targetValue)
        {
            currentValue++;
            text.text = currentValue.ToString();
            yield return new WaitForSeconds(1 / (targetValue + 1));
        }
    }

    #endregion
}
