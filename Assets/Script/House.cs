using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;  // สำหรับรีเซ็ต Scene



public class House : MonoBehaviour
{
    public int houseHP = 100;  // กำหนด HP เริ่มต้นของบ้าน
    public TextMeshProUGUI hpText;  // ตัวแปร TextMeshProUGUI สำหรับแสดงค่า HP บน UI
    public GameObject loseCanvas;  // ตัวแปร Canvas ที่จะแสดงเมื่อแพ้
    public GameObject Canvas_TowerIcon;
    public GameObject Canvas_Money;
    public GameObject Canvas_Startgame;
    
    // ฟังก์ชันที่ใช้เพื่อรับความเสียหาย
    public void SetDamage(int damageAmount)
    {
        houseHP -= damageAmount;  // ลด HP บ้านตามค่าดาเมจ
        UpdateHealthText();  // เรียกฟังก์ชันเพื่ออัพเดตข้อความ HP บน UI
        Debug.Log("บ้านถูกโจมตี! HP เหลือ: " + houseHP);
        if (houseHP <= 0)
        {
            Debug.Log("แพ้");  // แสดงข้อความเมื่อ HP ของบ้านหมด
            GameOver();
            // ทำการดำเนินการเพิ่มเติมที่ต้องการเมื่อบ้าน HP หมด (เช่น จบเกม)
        }
    }

    // ฟังก์ชันนี้ใช้ในการอัพเดตข้อความ HP บน UI
    private void UpdateHealthText()
    {
        if (hpText != null)
        {
            hpText.text = "HP: " + houseHP.ToString();  // แสดงค่า HP ของบ้าน
        }
    }
    
    // ฟังก์ชันนี้จะหยุดเกมและแสดง Canvas "Lose"
    private void GameOver()
    {
        // เล่นเสียงแพ้จาก SoundManager
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayLoseSound();
        }
        
        if (loseCanvas != null)
        {
            loseCanvas.SetActive(true);  // แสดง Canvas "Lose"
            Canvas_TowerIcon.SetActive(false);
            Canvas_Startgame.SetActive(false);
            Canvas_Money.SetActive(false);
        }
        Time.timeScale = 0;  // หยุดเกม
        
        //PlayerPrefs.DeleteKey("MoneyDoubled"); // ลบสถานะเงินสองเท่าเมื่อเกมจบ
        
        Debug.Log("เกมจบ! คุณแพ้");
    }
    
    public MoneyManager moneyManager; // อ้างอิงไปยัง MoneyManager
    // ฟังก์ชันรีเซ็ตเกม
    public void ResetGame()
    {
        // เพิ่มเงินเป็นสองเท่า
        if (moneyManager != null)
        {
            moneyManager.DoubleStartingMoney();  // เพิ่มเงินเป็นสองเท่า
            PlayerPrefs.SetInt("MoneyDoubled", 1); // บันทึกสถานะว่าเงินถูกเพิ่มสองเท่า
        }

        Time.timeScale = 1; // รีเซ็ตเวลาให้กลับมาเดินปกติ
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // โหลดซีนใหม่
    }
    
}