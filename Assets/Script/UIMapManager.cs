using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMapManager : MonoBehaviour
{
    public Button level1Button; // ปุ่มสำหรับ Level 1
    public Button level2Button; // ปุ่มสำหรับ Level 2
    public Button level3Button; // ปุ่มสำหรับ Level 3

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // รับตัวแปร GameManager
        RefreshLevelButtons();  // รีเฟรชปุ่มเพื่อให้สถานะถูกต้องเมื่อเริ่มต้น
        // ตรวจสอบว่าปุ่มมีการกำหนดไว้หรือไม่ก่อนใช้งาน
        if (level2Button != null)
        {
            level2Button.interactable = gameManager.IsLevelUnlocked("Level2");
            level2Button.onClick.AddListener(() => LoadLevel("Level2"));
        }

        if (level3Button != null)
        {
            level3Button.interactable = gameManager.IsLevelUnlocked("Level3");
            level3Button.onClick.AddListener(() => LoadLevel("Level3"));
        }

        if (level1Button != null)
        {
            level1Button.onClick.AddListener(() => LoadLevel("Level1"));
        }
    }

    // โหลดฉากที่เลือกเมื่อคลิกปุ่ม
    void LoadLevel(string levelName)
    {
        Debug.Log($"Checking if {levelName} is unlocked: {gameManager.IsLevelUnlocked(levelName)}");

        if (gameManager.IsLevelUnlocked(levelName)) // ตรวจสอบว่า Level นั้นถูกปลดล็อคแล้วหรือไม่
        {
            Debug.Log($"Loading scene: {levelName}");
            SceneManager.LoadScene(levelName); // โหลดฉากที่มีชื่อที่เลือก
        }
    }

    // ฟังก์ชันนี้จะปลดล็อคปุ่ม Level ตามเงื่อนไข
    public void UnlockLevelButton()
    {
        if (level2Button != null)
        {
            level2Button.interactable = true; // ปลดล็อคปุ่ม Level 2
            Debug.Log("Level 2 Button unlocked!");
        }
        if (level3Button != null)
        {
            level3Button.interactable = true; // ปลดล็อคปุ่ม Level 3
            Debug.Log("Level 3 Button unlocked!");
        }
    }
    
    public void RefreshLevelButtons()
    {
        // ตัวอย่างสมมุติว่ามีปุ่มที่ชื่อว่า level2Button
        if (GameManager.Instance.IsLevelUnlocked("Level2"))
        {
            level2Button.interactable = true; // เปิดใช้งานปุ่ม
        }
        else
        {
            level2Button.interactable = false; // ปิดปุ่ม
        }

        if (GameManager.Instance.IsLevelUnlocked("Level3"))
        {
            level3Button.interactable = true;
        }
        else
        {
            level3Button.interactable = false;
        }
    }


    
    // เรียกใช้เมื่อผู้เล่นชนะ Level และต้องการปลดล็อค Level ถัดไป
    public void UnlockNextLevel(int level)
    {
        gameManager.UnlockNextLevel(level); // เรียกใช้ฟังก์ชันปลดล็อคใน GameManager
    }
}
