using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : PanelBase
{


    [Header("Upgrade Buttons")]
    [SerializeField] private Button upgradeAttackSpeedButton;
    [SerializeField] private Button upgradeDamageButton;
    [SerializeField] private Button upgradeRangeButton;
    [SerializeField] private Button upgradeLifeStealButton;
    [SerializeField] private Button upgradeMovementSpeedButton;
    [SerializeField] private Button upgradePowerupDurButton;
    [SerializeField] private Button upgradeMaxHealthButton;
    [SerializeField] private Button upgradeDualWeaponButton;

    [Header("Grayed Buttons")]
    [SerializeField] private List<GameObject> grayButtonList;
    [SerializeField] private GameObject asGrayButton;
    [SerializeField] private GameObject damageGrayButton;
    [SerializeField] private GameObject rangeGrayButton;
    [SerializeField] private GameObject lifeStealGrayButton;
    [SerializeField] private GameObject movementSpeedGrayButton;
    [SerializeField] private GameObject powerupDurGrayButton;
    [SerializeField] private GameObject maxHealthGrayButton;
    [SerializeField] private GameObject dualWeaponGrayButton;



    #region Upgrading Texts Variables

    [Header("!--Upgrade Texts--!")]

    [Header("Attack Speed Texts")]
    [SerializeField] private TMP_Text attackSpeedCurrentText;
    [SerializeField] private TMP_Text attackSpeedNextText;
    [SerializeField] private TMP_Text attackSpeedCostText;
    [SerializeField] private TMP_Text attackSpeedLevelText;

    [Header("Damage Texts")]
    [SerializeField] private TMP_Text damageCurrentText;
    [SerializeField] private TMP_Text damageNextText;
    [SerializeField] private TMP_Text damageCostText;
    [SerializeField] private TMP_Text damageLevelText;

    [Header("Range")]
    [SerializeField] private TMP_Text rangeCurrentText;
    [SerializeField] private TMP_Text rangeNextText;
    [SerializeField] private TMP_Text rangeCostText;
    [SerializeField] private TMP_Text rangeLevelText;

    [Header("Life Steal Texts")]
    [SerializeField] private TMP_Text lifeStealCurrentText;
    [SerializeField] private TMP_Text lifeStealNextText;
    [SerializeField] private TMP_Text lifeStealCostText;
    [SerializeField] private TMP_Text lifeStealLevelText;


    [Header("Movement Speed Texts")]
    [SerializeField] private TMP_Text movementSpeedCurrentText;
    [SerializeField] private TMP_Text movementSpeedNextText;
    [SerializeField] private TMP_Text movementSpeedCostText;
    [SerializeField] private TMP_Text movementSpeedLevelText;

    [Header("Powerup Dur Texts")]
    [SerializeField] private TMP_Text powerupDurCurrentText;
    [SerializeField] private TMP_Text powerupDurNextText;
    [SerializeField] private TMP_Text powerupDurCostText;
    [SerializeField] private TMP_Text powerupDurLevelText;


    [Header("Max Health Texts")]
    [SerializeField] private TMP_Text maxHealthCurrentText;
    [SerializeField] private TMP_Text maxHealthNextText;
    [SerializeField] private TMP_Text maxHealthCostText;
    [SerializeField] private TMP_Text maxHealthLevelText;

    [Header("Dual Weapon Texts")]
    [SerializeField] private TMP_Text dualWeaponCostText;

    [Header("!--- Upgrade Text Visuals ---!")]
    [SerializeField] private List<string> attackSpeedVisualValues;
    [SerializeField] private List<string> damageVisualValues;
    [SerializeField] private List<string> rangeVisualValues;
    [SerializeField] private List<string> lifeStealVisualValues;
    [SerializeField] private List<string> movementSpeedVisualValues;
    [SerializeField] private List<string> maxHealthVisualValues;
    [SerializeField] private List<string> powerupDurVisualValues;

    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();

        //upgrade button events

        upgradeAttackSpeedButton.onClick.AddListener(playerStats.AttemptUpgradeAttackSpeed);
        upgradeDamageButton.onClick.AddListener(playerStats.AttemptUpgradeDamage);
        upgradeRangeButton.onClick.AddListener(playerStats.AttemptUpgradeRange);
        upgradeLifeStealButton.onClick.AddListener(playerStats.AttemptUpgradeLifeSteal);
        upgradeMovementSpeedButton.onClick.AddListener(playerStats.AttemptUpgradeMovementSpeed);
        upgradePowerupDurButton.onClick.AddListener(playerStats.AttemptUpgradePowerupDuration);
        upgradeMaxHealthButton.onClick.AddListener(playerStats.AttemptUpgradeMaxHealth);
        upgradeDualWeaponButton.onClick.AddListener(playerStats.AttemptUpgradeDualWeapon);

        // Upgrade Done Events Assign
        playerStats.OnAttackSpeedUpgraded += UpdateUpgradeUI;
        playerStats.OnDamageUpgraded += UpdateUpgradeUI;
        playerStats.OnRangeUpgraded += UpdateUpgradeUI;
        playerStats.OnLifeStealUpgraded += UpdateUpgradeUI;
        playerStats.OnMovementSpeedUpgraded += UpdateUpgradeUI;
        playerStats.OnPowerupDurUpgraded += UpdateUpgradeUI;
        playerStats.OnMaxHealthUpgraded += UpdateUpgradeUI;
        playerStats.OnDualWeaponUpgraded += UpdateUpgradeUI;
    }

    private void OnDisable()
    {

        // Upgrading Button Events Clean Up
        upgradeAttackSpeedButton.onClick.RemoveAllListeners();
        upgradeDamageButton.onClick.RemoveAllListeners();
        upgradeRangeButton.onClick.RemoveAllListeners();
        upgradeLifeStealButton.onClick.RemoveAllListeners();
        upgradeMovementSpeedButton.onClick.RemoveAllListeners();
        upgradePowerupDurButton.onClick.RemoveAllListeners();
        upgradeMaxHealthButton.onClick.RemoveAllListeners();


        // Upgrade Done Events Clean Up
        playerStats.OnAttackSpeedUpgraded -= UpdateUpgradeUI;
        playerStats.OnDamageUpgraded -= UpdateUpgradeUI;
        playerStats.OnRangeUpgraded -= UpdateUpgradeUI;

        playerStats.OnLifeStealUpgraded -= UpdateUpgradeUI;
        playerStats.OnMovementSpeedUpgraded -= UpdateUpgradeUI;
        playerStats.OnPowerupDurUpgraded -= UpdateUpgradeUI;
        playerStats.OnMaxHealthUpgraded -= UpdateUpgradeUI;
        playerStats.OnDualWeaponUpgraded -= UpdateUpgradeUI;
    }

    void Start()
    {
        UpdateUpgradeUI();
    }

    private void DisableGrayedButtons()
    {
        for (int i = 0; i < grayButtonList.Count; i++)
        {
            grayButtonList[i].SetActive(false);
        }
    }

    #region Upgrade

    private void UpdateUpgradeUI()
    {
        DisableGrayedButtons();

        // ATTACK SPEED
        UpdateAttackSpeedTexts();

        // DAMAGE
        UpdateDamageTexts();

        //  LIFE STEAL
        UpdateLifeStealTexts();

        // MOVEMENT SPEED
        UpdateMovementSpeedTexts();

        //POWER UP DUR
        UpdatePowerupDurTexts();

        // MAX HEALTH
        UpdateMaxHealthTexts();

        // Dual Weapon 
        UpdateDualWeaponTexts();

        UpdateRangeTexts();

    }

    // Dual Weapon 
    private void UpdateDualWeaponTexts()
    {
        if (playerStats.GetPlayerSO().GetMaxHealthCosts()[playerStats.maxHealthIndex] > playerStats.money)
        {
            dualWeaponGrayButton.SetActive(true);
            dualWeaponCostText.color = Color.red;
        }
        else
        {
            dualWeaponGrayButton.SetActive(false);
            dualWeaponCostText.color = Color.black;
        }

        if (playerStats.dualWeaponIndex > 0)
        {
            dualWeaponCostText.text = "MAX";
        }
        else
        {
            dualWeaponCostText.text = playerStats.GetPlayerSO().GetDualWeaponCost().ToString();
        }
    }

    private void UpdateUpgradeText(TMP_Text textToUpdate, string value, string newString = null)
    {
        if (textToUpdate == null)
        {
            Debug.LogWarning("textToUpdate" + " is null");
            return;
        }

        if (newString != null)
        {
            textToUpdate.text = newString + " " + value.ToString();
        }
        else
        {
            textToUpdate.text = value.ToString();
        }

    }

    // MAX HEALTH
    private void UpdateMaxHealthTexts()
    {


        if (playerStats.GetPlayerSO().GetMaxHealthValues().Count - 1 > playerStats.maxHealthIndex)
        {
            UpdateUpgradeText(maxHealthCurrentText, maxHealthVisualValues[playerStats.maxHealthIndex]);
            UpdateUpgradeText(maxHealthNextText, maxHealthVisualValues[playerStats.maxHealthIndex + 1]);
            UpdateUpgradeText(maxHealthLevelText, (playerStats.maxHealthIndex + 1).ToString() + " / " + maxHealthVisualValues.Count.ToString());

            if (playerStats.GetPlayerSO().GetMaxHealthCosts()[playerStats.maxHealthIndex] > playerStats.money)
            {
                maxHealthGrayButton.SetActive(true);
                UpdateUpgradeText(maxHealthCostText, playerStats.GetPlayerSO().GetMaxHealthCosts()
                    [playerStats.maxHealthIndex].ToString());
                maxHealthCostText.color = Color.red;
            }
            else
            {
                maxHealthGrayButton.SetActive(false);
                maxHealthCostText.color = Color.black;
            }
        }
        else
        {
            UpdateUpgradeText(maxHealthCurrentText, maxHealthVisualValues[playerStats.maxHealthIndex]);
            UpdateUpgradeText(maxHealthNextText, "MAX");
            UpdateUpgradeText(maxHealthCostText, "MAX");
            UpdateUpgradeText(maxHealthLevelText, (playerStats.maxHealthIndex + 1).ToString() + " / " + maxHealthVisualValues.Count.ToString());
        }

    }

    //POWER UP DUR
    private void UpdatePowerupDurTexts()
    {

        if (playerStats.GetPlayerSO().GetPowerupDurValues().Count - 1 > playerStats.powerupDurIndex)
        {
            UpdateUpgradeText(powerupDurCurrentText, powerupDurVisualValues[playerStats.powerupDurIndex]);
            UpdateUpgradeText(powerupDurNextText, powerupDurVisualValues[playerStats.powerupDurIndex + 1]);

            UpdateUpgradeText(powerupDurLevelText, (playerStats.powerupDurIndex + 1).ToString() + " / " + powerupDurVisualValues.Count.ToString());

            if (playerStats.GetPlayerSO().GetPowerupDurCosts()[playerStats.powerupDurIndex] > playerStats.money)
            {
                powerupDurGrayButton.SetActive(true);
                UpdateUpgradeText(powerupDurCostText, playerStats.GetPlayerSO().GetPowerupDurCosts()
                    [playerStats.powerupDurIndex].ToString());
                powerupDurCostText.color = Color.red;
            }
            else
            {
                powerupDurGrayButton.SetActive(false);
                powerupDurCostText.color = Color.black;
            }
        }
        else
        {
            UpdateUpgradeText(powerupDurCurrentText, powerupDurVisualValues[playerStats.powerupDurIndex]);
            UpdateUpgradeText(powerupDurNextText, "MAX");
            UpdateUpgradeText(powerupDurCostText, "MAX");
            UpdateUpgradeText(powerupDurLevelText, (playerStats.powerupDurIndex + 1).ToString() + " / " + powerupDurVisualValues.Count.ToString());
        }
    }

    // MOVEMENT SPEED
    private void UpdateMovementSpeedTexts()
    {
        if (playerStats.GetPlayerSO().GetMovementSpeedValues().Count - 1 > playerStats.movementSpeedIndex)
        {
            UpdateUpgradeText(movementSpeedCurrentText, movementSpeedVisualValues[playerStats.movementSpeedIndex]);
            UpdateUpgradeText(movementSpeedNextText, movementSpeedVisualValues[playerStats.movementSpeedIndex + 1]);

            UpdateUpgradeText(movementSpeedLevelText, (playerStats.movementSpeedIndex + 1).ToString() + " / " + movementSpeedVisualValues.Count.ToString());

            if (playerStats.GetPlayerSO().GetMovementSpeedCosts()[playerStats.movementSpeedIndex] > playerStats.money)
            {
                movementSpeedGrayButton.SetActive(true);
                UpdateUpgradeText(movementSpeedCostText, playerStats.GetPlayerSO().GetMovementSpeedCosts()
                    [playerStats.movementSpeedIndex].ToString());
                movementSpeedCostText.color = Color.red;
            }
            else
            {
                movementSpeedGrayButton.SetActive(false);
                movementSpeedCostText.color = Color.black;
            }
        }
        else
        {
            UpdateUpgradeText(movementSpeedCurrentText, movementSpeedVisualValues[playerStats.movementSpeedIndex]);
            UpdateUpgradeText(movementSpeedNextText, "MAX");
            UpdateUpgradeText(movementSpeedCostText, "MAX");
            UpdateUpgradeText(movementSpeedLevelText, (playerStats.movementSpeedIndex + 1).ToString() + " / " + movementSpeedVisualValues.Count.ToString());
        }

    }

    //  LIFE STEAL
    private void UpdateLifeStealTexts()
    {

        if (playerStats.GetPlayerSO().GetLifeStealValues().Count - 1 > playerStats.lifeStealIndex)
        {
            UpdateUpgradeText(lifeStealCurrentText, lifeStealVisualValues[playerStats.lifeStealIndex]);
            UpdateUpgradeText(lifeStealNextText, lifeStealVisualValues[playerStats.lifeStealIndex + 1]);
            UpdateUpgradeText(lifeStealLevelText, (playerStats.lifeStealIndex + 1).ToString() + " / " + lifeStealVisualValues.Count.ToString());

            if (playerStats.GetPlayerSO().GetLifeStealCosts()[playerStats.lifeStealIndex] > playerStats.money)
            {
                lifeStealGrayButton.SetActive(true);
                UpdateUpgradeText(lifeStealCostText, playerStats.GetPlayerSO().GetLifeStealCosts()
                    [playerStats.lifeStealIndex].ToString());
                lifeStealCostText.color = Color.red;
            }
            else
            {
                lifeStealGrayButton.SetActive(false);
                lifeStealCostText.color = Color.black;
            }

        }
        else
        {
            UpdateUpgradeText(lifeStealCurrentText, lifeStealVisualValues[playerStats.lifeStealIndex]);
            UpdateUpgradeText(lifeStealNextText, "MAX");
            UpdateUpgradeText(lifeStealCostText, "MAX");
            UpdateUpgradeText(lifeStealLevelText, (playerStats.lifeStealIndex + 1).ToString() + " / " + lifeStealVisualValues.Count.ToString());
        }

    }

    // DAMAGE
    private void UpdateDamageTexts()
    {
        if (playerStats.GetPlayerSO().GetDamageValues().Count - 1 > playerStats.damageIndex)
        {
            UpdateUpgradeText(damageCurrentText, damageVisualValues[playerStats.damageIndex]);
            UpdateUpgradeText(damageNextText, damageVisualValues[playerStats.damageIndex + 1]);
            UpdateUpgradeText(damageLevelText, (playerStats.damageIndex + 1).ToString() + " / " + damageVisualValues.Count.ToString());

            if (playerStats.GetPlayerSO().GetDamageCosts()[playerStats.damageIndex] > playerStats.money)
            {
                damageGrayButton.SetActive(true);
                UpdateUpgradeText(damageCostText, playerStats.GetPlayerSO().GetDamageCosts()
                    [playerStats.damageIndex].ToString());
                damageCostText.color = Color.red;
            }
            else
            {
                damageGrayButton.SetActive(false);
                damageCostText.color = Color.black;
            }
        }
        else
        {
            UpdateUpgradeText(damageCurrentText, damageVisualValues[playerStats.damageIndex]);
            UpdateUpgradeText(damageNextText, "MAX");
            UpdateUpgradeText(damageCostText, "MAX");
            UpdateUpgradeText(damageLevelText, (playerStats.damageIndex + 1).ToString() + " / " + damageVisualValues.Count.ToString());
        }
    }

    private void UpdateRangeTexts()
    {

        if (playerStats.GetPlayerSO().GetRangeValues().Count - 1 > playerStats.rangeIndex)
        {
            UpdateUpgradeText(rangeCurrentText, rangeVisualValues[playerStats.rangeIndex]);
            UpdateUpgradeText(rangeNextText, rangeVisualValues[playerStats.rangeIndex + 1]);
            UpdateUpgradeText(rangeLevelText, (playerStats.rangeIndex + 1).ToString() + " / " + rangeVisualValues.Count.ToString());
            UpdateUpgradeText(rangeCostText, playerStats.GetPlayerSO().
                    GetRangeCosts()[playerStats.rangeIndex].ToString());

            if (playerStats.GetPlayerSO().GetRangeCosts()[playerStats.rangeIndex] > playerStats.money)
            {
                rangeGrayButton.SetActive(true);
                rangeCostText.color = Color.red;
            }
            else
            {
                rangeGrayButton.SetActive(false);
                rangeCostText.color = Color.black;
            }
        }
        else
        {
            UpdateUpgradeText(rangeCurrentText, rangeVisualValues[playerStats.rangeIndex]);
            UpdateUpgradeText(rangeNextText, "MAX");
            UpdateUpgradeText(rangeCostText, "MAX");
            UpdateUpgradeText(rangeLevelText, (playerStats.rangeIndex + 1).ToString() + " / " + rangeVisualValues.Count.ToString());
        }
    }

    // ATTACK SPEED
    private void UpdateAttackSpeedTexts()
    {

        if (playerStats.GetPlayerSO().GetAttackSpeedValues().Count - 1 > playerStats.attackSpeedIndex)
        {
            UpdateUpgradeText(attackSpeedCurrentText, attackSpeedVisualValues[playerStats.attackSpeedIndex]);
            UpdateUpgradeText(attackSpeedNextText, attackSpeedVisualValues[playerStats.attackSpeedIndex + 1]);
            UpdateUpgradeText(attackSpeedLevelText, (playerStats.attackSpeedIndex + 1).ToString() + "/" + attackSpeedVisualValues.Count.ToString());
            UpdateUpgradeText(attackSpeedCostText, playerStats.GetPlayerSO().
                           GetAttackSpeedCosts()[playerStats.attackSpeedIndex].ToString());

            if (playerStats.GetPlayerSO().GetAttackSpeedCosts()[playerStats.attackSpeedIndex] > playerStats.money)
            {
                asGrayButton.SetActive(true);
                attackSpeedCostText.color = Color.red;
            }
            else
            {
                asGrayButton.SetActive(false);
                attackSpeedCostText.color = Color.black;
            }
        }
        else
        {
            UpdateUpgradeText(attackSpeedCurrentText, attackSpeedVisualValues[playerStats.attackSpeedIndex]);
            UpdateUpgradeText(attackSpeedNextText, "MAX");
            UpdateUpgradeText(attackSpeedCostText, "MAX");
            UpdateUpgradeText(attackSpeedLevelText, (playerStats.attackSpeedIndex + 1).ToString() + "/" + attackSpeedVisualValues.Count.ToString());
        }

    }

    #endregion
}