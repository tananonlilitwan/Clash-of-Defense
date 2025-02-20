using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MoneyManager : MonoBehaviour
{
    [SerializeField] public int startingMoney; // เงินเริ่มต้น
    public TextMeshProUGUI moneyText; // ตัวแปร UI Text ที่แสดงจำนวนเงิน
    public int currentMoney { get; private set; } // เงินปัจจุบัน
    
    public GameObject warningPanel; // GameObject สำหรับแสดงข้อความเตือน
    

    void Start()
    {
        if (PlayerPrefs.GetInt("MoneyDoubled", 0) == 1)
        {
            currentMoney = startingMoney * 2;  // หากกด ResetGame() ให้เงินเพิ่มสองเท่า
        }
        else
        {
            currentMoney = startingMoney;  // หากไม่ได้กด ResetGame() ให้ใช้ค่าเงินเริ่มต้น
        }
        // รีเซ็ตสถานะเงินคูณสองทุกครั้งที่เริ่ม Scene ใหม่
        PlayerPrefs.DeleteKey("MoneyDoubled");
        
        //currentMoney = startingMoney;
        UpdateMoneyUI(); // อัปเดต UI ครั้งแรกเมื่อเริ่มเกม
        warningPanel.SetActive(false); // ซ่อน Panel ตอนเริ่มเกม
    }

    // ฟังก์ชันเพิ่มเงิน
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI(); // อัปเดต UI ทุกครั้งที่มีการเปลี่ยนแปลงเงิน
        Debug.Log("เพิ่มเงิน: " + amount + " เงินปัจจุบัน: " + currentMoney);
    }
    
    // ฟังก์ชันเพื่อดึงยอดเงินปัจจุบัน
    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    // ฟังก์ชันหักเงิน
    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateMoneyUI(); // อัปเดต UI ทุกครั้งที่มีการเปลี่ยนแปลงเงิน
            Debug.Log("ใช้เงิน: " + amount + " เงินที่เหลือ: " + currentMoney);
            return true;
        }
        else
        {
            Debug.LogWarning("เงินไม่พอ!");
            StartCoroutine(ShowWarningPanel()); // เรียกฟังก์ชันแสดง Panel
            return false;
        }
    }
    
    // ฟังก์ชันเพิ่มเงินเริ่มต้นเป็นสองเท่า
    public void DoubleStartingMoney()
    {
        startingMoney *= 2; // เพิ่มเงินเริ่มต้นเป็นสองเท่า
        currentMoney = startingMoney; // อัปเดตเงินปัจจุบัน
        UpdateMoneyUI();
        Debug.Log("เงินเริ่มต้นเพิ่มเป็นสองเท่า: " + startingMoney);
    }
    
    
    // ฟังก์ชันอัปเดต UI แสดงจำนวนเงิน
    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "" + currentMoney.ToString();
        }
    }
    
    private IEnumerator ShowWarningPanel()
    {
        warningPanel.SetActive(true); // แสดง Panel
        yield return new WaitForSeconds(2f); // รอ 2 วินาที
        warningPanel.SetActive(false); // ซ่อน Panel
    }
    
}
