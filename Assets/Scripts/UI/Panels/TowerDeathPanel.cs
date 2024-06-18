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


    protected override void OnEnable()
    {
        base.OnEnable();
        earningsHolder = EarningsHolder.instance;

        loseGameButton.onClick.AddListener(GameLostPanelSequenceEnd);
        earningsHolder.OnEarningsApply += ActivateAndUpdateTowerDeathEarningsTexts;
    }

    private void OnDisable()
    {
        loseGameButton.onClick.RemoveAllListeners();

        earningsHolder.OnEarningsApply -= ActivateAndUpdateTowerDeathEarningsTexts;
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

        uiManager.GetBackToGamePanel();
        gameManager.LevelLost();
    }

    private void ActivateAndUpdateTowerDeathEarningsTexts(int meatValue, int coinValue, int xpValue)
    {

        gameLostOpenedChestIcon.SetActive(true);
        gameLostClosedChestIcon.SetActive(false);


        gameLostCollectedXPText.transform.parent.gameObject.SetActive(true);
        gameLostCollectedCoinText.transform.parent.gameObject.SetActive(true);
        gameLostCollectedMeatText.transform.parent.gameObject.SetActive(true);

        gameLostCollectedXPText.text = xpValue.ToString();
        gameLostCollectedCoinText.text = coinValue.ToString();
        gameLostCollectedMeatText.text = meatValue.ToString();

    }

    #endregion
}
