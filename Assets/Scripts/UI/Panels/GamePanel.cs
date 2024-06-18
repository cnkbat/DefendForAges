using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : PanelBase
{

    [Header("GameHud Texts")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text meatText;
    [SerializeField] private TMP_Text currentWaveIndexText;
    [SerializeField] private TMP_Text nextWaveIndexText;


    [Header("Wave Control")]
    [SerializeField] private List<GameObject> totalProgressBarImages;
    [SerializeField] private Slider totalWaveProgressBar;
    [SerializeField] private Slider inwaveProgressBar;

    [Header("ProgressBar Animation")]
    [SerializeField] private RectTransform progressBarHideTransform;
    [SerializeField] private RectTransform totalWaveProgressBarOriginalTransform;
    [SerializeField] private RectTransform inwaveProgressBarOriginalTransform;
    [SerializeField] private float progressBarAnimDur;
    [SerializeField] private Ease progressBarEaseType;

    [Header("Power Up Slider")]
    [SerializeField] private Image powerUpFill;
    [SerializeField] private ParticleSystem powerUpVFX;

    [Header("Levelling UI")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] Slider levelSlider;


    protected override void OnEnable()
    {
        base.OnEnable();

        // text Updates
        playerStats.OnExperienceGain += UpdateLevelBar;
        playerStats.OnMoneyChange += UpdateMoneyText;
        playerStats.OnMeatChange += UpdateMeatText;

        // Power UP
        playerStats.OnPowerUpValueChanged += UpdatePowerUpSliderValue;
        playerStats.OnPowerUpEnabled += EnablePowerupUIAnimation;
        playerStats.OnPowerUpDisabled += DisablePowerupUIAnimation;

    }

    private void OnDisable()
    {

        //text update cleanup
        playerStats.OnExperienceGain -= UpdateLevelBar;
        playerStats.OnMoneyChange -= UpdateMoneyText;
        playerStats.OnMeatChange -= UpdateMeatText;

        // Power UP cleanup
        playerStats.OnPowerUpValueChanged += UpdatePowerUpSliderValue;
        playerStats.OnPowerUpEnabled += EnablePowerupUIAnimation;
        playerStats.OnPowerUpDisabled += DisablePowerupUIAnimation;
    }

    void Start()
    {
        UpdateMoneyText();
        UpdateMeatText();
        UpdateAllWavesProgressBar();
        UpdatePowerUpSliderValue(0);
    }

    #region Wave Related

    public void OnWaveCallClicked()
    {

        totalWaveProgressBar.transform.parent.DOLocalMoveY(progressBarHideTransform.anchoredPosition.y, progressBarAnimDur).
            SetEase(progressBarEaseType).
                OnComplete(() => totalWaveProgressBar.transform.parent.gameObject.SetActive(false));


        inwaveProgressBar.transform.parent.gameObject.SetActive(true);

        inwaveProgressBar.transform.parent.DOLocalMoveY(inwaveProgressBarOriginalTransform.anchoredPosition.y, progressBarAnimDur)
            .SetEase(progressBarEaseType);

        UpdateInWaveProgressBarTexts();
    }

    public void UpdateAllWavesProgressBar()
    {
        inwaveProgressBar.transform.parent.DOLocalMoveY(progressBarHideTransform.anchoredPosition.y, progressBarAnimDur).
           SetEase(progressBarEaseType).
               OnComplete(() => inwaveProgressBar.transform.parent.gameObject.SetActive(false));

        totalWaveProgressBar.transform.parent.gameObject.SetActive(true);
        totalWaveProgressBar.transform.parent.DOLocalMoveY(totalWaveProgressBarOriginalTransform.anchoredPosition.y, progressBarAnimDur)
            .SetEase(progressBarEaseType);
        totalWaveProgressBar.value = (float)playerStats.GetWaveIndex() / (float)gameManager.totalWaveCount;

        for (int i = 0; i < totalProgressBarImages.Count; i++)
        {
            totalProgressBarImages[i].SetActive(false);
        }

        Debug.Log("city Index =" + playerStats.GetCityIndex());

        totalProgressBarImages[playerStats.GetCityIndex()].SetActive(true);
        UpdateInWaveProgressBarTexts();

    }
    public void UpdateInWaveProgressBarValue(float value)
    {
        inwaveProgressBar.value = value;
    }

    public void UpdateInWaveProgressBarTexts()
    {
        uiManager.UpdateText(currentWaveIndexText, playerStats.GetWaveIndex());
        uiManager.UpdateText(nextWaveIndexText, playerStats.GetWaveIndex() + 1);
    }

    #endregion

    #region Texts
    private void UpdateMoneyText()
    {
        uiManager.UpdateText(moneyText, playerStats.money);
    }
    private void UpdateMeatText()
    {
        uiManager.UpdateText(meatText, playerStats.meat);
    }

    #endregion

    #region LevelBar
    public void UpdateLevelBar(int playerLevel, float sliderLevel, int currentXP, int nextXP)
    {
        levelSlider.value = sliderLevel;
        uiManager.UpdateText(levelText, playerLevel);
        xpText.text = currentXP.ToString() + " / " + nextXP.ToString();
    }
    #endregion

    #region Power Up
    public void UpdatePowerUpSliderValue(float newValue)
    {
        powerUpFill.fillAmount = newValue;
    }

    public void EnablePowerupUIAnimation()
    {
        powerUpFill.fillAmount = 1;
        powerUpVFX.Play();
    }

    public void DisablePowerupUIAnimation()
    {
        powerUpFill.fillAmount = 0;
        powerUpVFX.Stop();
    }
    #endregion

}
