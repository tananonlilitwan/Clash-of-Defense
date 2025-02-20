using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeManager : MonoBehaviour
{
    private TowerStats towerStats; // ตัวแปรที่จะเก็บข้อมูลจาก TowerStats
    
    public Button upgradeDamageButton;
    public Button upgradeRangeButton;
    public Button upgradeFireRateButton;
    public Button upgradeCostReductionButton;

    private void Start()
    {
        // โหลด TowerStats จาก Resources
        towerStats = Resources.Load<TowerStats>("TowerStats");

        // เช็คว่าโหลดได้สำเร็จไหม
        if (towerStats != null)
        {
            Debug.Log("TowerStats loaded successfully.");
        }
        else
        {
            Debug.LogError("Failed to load TowerStats!");
        }
        
        
        // Assigning Upgrade methods to the buttons
        upgradeDamageButton.onClick.AddListener(() => UpgradeDamage(0.07f));  // 7% damage upgrade
        upgradeRangeButton.onClick.AddListener(() => UpgradeRange(0.07f));  // 7% range upgrade
        upgradeFireRateButton.onClick.AddListener(() => UpgradeFireRate(0.07f));  // 7% fire rate upgrade
        upgradeCostReductionButton.onClick.AddListener(() => UpgradeCostReduction(0.07f));  // 7% cost reduction
    }

    // Method to handle damage upgrade
    private void UpgradeDamage(float percentage)
    {
        if (CheckMoney(towerStats.towerCost))
        {
            towerStats.UpgradeDamage(percentage);
        }
    }

    // Method to handle range upgrade
    private void UpgradeRange(float percentage)
    {
        if (CheckMoney(towerStats.towerCost))
        {
            towerStats.UpgradeRange(percentage);
        }
    }

    // Method to handle fire rate upgrade
    private void UpgradeFireRate(float percentage)
    {
        if (CheckMoney(towerStats.towerCost))
        {
            towerStats.UpgradeFireRate(percentage);
        }
    }

    // Method to handle cost reduction upgrade
    private void UpgradeCostReduction(float percentage)
    {
        if (CheckMoney(towerStats.towerCost))
        {
            towerStats.UpgradeCostReduction(percentage);
        }
    }

    // A method to check if the player has enough money (this is just a placeholder)
    private bool CheckMoney(int cost)
    {
        int playerMoney = 100; // Set the player's current money here
        if (playerMoney >= cost)
        {
            playerMoney -= cost;  // Deduct the money
            return true;
        }
        else
        {
            Debug.Log("Not enough money!");
            return false;
        }
    }
}
