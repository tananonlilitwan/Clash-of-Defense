using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string Level2Key = "Level2Unlocked";
    private const string Level3Key = "Level3Unlocked";
    
    public Button uiMapLevelButton; // ปุ่มสำหรับไปยัง UIMapLevel
    public Button Restart_again;
    
    
    
    public Button uiMapLevel; // ปุ่มสำหรับไปยัง UIMapLevel
    public Button Restart;
    public GameObject confirmationPanel; // Panel ยืนยัน
    private bool isConfirming = false;   // ตัวแปรสถานะว่ากำลังยืนยันอยู่หรือไม่
    
    public static GameManager Instance { get; private set; }

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // ถ้ามี Instance อยู่แล้ว ให้ทำลายตัวเอง
        }
    }

    
    private void Start()
    {
        uiMapLevel.onClick.AddListener(HandleSceneChange); // เพิ่มฟังก์ชัน HandleSceneChange ให้ปุ่ม
        confirmationPanel.SetActive(false); // ซ่อน Panel ตอนเริ่มเกม
        
        // เพิ่ม listener สำหรับปุ่ม UIMapLevel
        uiMapLevelButton.onClick.AddListener(() => GoToUIMapLevel());
        // เพิ่ม listener สำหรับปุ่ม Restart_again
        Restart_again.onClick.AddListener(RestartCurrentScene);
        
        // เพิ่ม listener สำหรับปุ่ม UIMapLevel
        //uiMapLevel.onClick.AddListener(() => GoToUIMapLevel());
        // เพิ่ม listener สำหรับปุ่ม Restart_again
        Restart.onClick.AddListener(RestartCurrentScene);
        
        
    }
    // ฟังก์ชันสำหรับไปยัง UIMapLevel
    void GoToUIMapLevel()
    {
        SceneManager.LoadScene("UIMapLevel"); // ไปที่ UIMapLevel Scene
    }
    
    public void GoUIMapLevel()
    {
        SceneManager.LoadScene("UIMapLevel"); // ไปที่ UIMapLevel Scene
    }
    
    // ฟังก์ชันสำหรับรีสตาร์ท Scene ปัจจุบัน
    void RestartCurrentScene()
    {
        // โหลด Scene ปัจจุบันอีกครั้ง
        Scene currentScene = SceneManager.GetActiveScene(); // รับ Scene ปัจจุบัน
        SceneManager.LoadScene(currentScene.name); // โหลด Scene นี้ใหม่
    }
    
    // ฟังก์ชันจัดการการเปลี่ยน Scene
    void HandleSceneChange()
    {
        if (!isConfirming)
        {
            ShowConfirmationPanel(); // แสดง Panel ถ้ายังไม่ได้กดเพื่อยืนยัน
        }
        else
        {
            GoToUIMapLevel(); // เปลี่ยน Scene ถ้ากำลังอยู่ในสถานะยืนยัน
        }
    }
    // ฟังก์ชันแสดง Panel ยืนยัน
    void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true); // แสดง Panel
        isConfirming = true; // เปลี่ยนสถานะเป็นกำลังยืนยัน
    }
    // ฟังก์ชันซ่อน Panel หากยกเลิกการเปลี่ยน Scene
    public void CancelSceneChange()
    {
        confirmationPanel.SetActive(false); // ซ่อน Panel
        isConfirming = false; // รีเซ็ตสถานะ
    }
    
    // ตรวจสอบสถานะการปลดล็อค Level
    public bool IsLevelUnlocked(string sceneName)
    {
        if (sceneName == "Level1")
        {
            // Level1 ควรปลดล็อคเสมอ
            return true;
        }
        if (sceneName == "Level2")
        {
            return PlayerPrefs.GetInt(Level2Key, 0) == 1; // ตรวจสอบว่า Level 2 ถูกปลดล็อคหรือยัง
        }
        if (sceneName == "Level3")
        {
            return PlayerPrefs.GetInt(Level3Key, 0) == 1; // ตรวจสอบว่า Level 3 ถูกปลดล็อคหรือยัง
        }
        return false;
    }


    // ฟังก์ชันปลดล็อค Level ถัดไป
    public void UnlockNextLevel(int level)
    {
        if (level == 1)
        {
            PlayerPrefs.SetInt(Level2Key, 1); // ปลดล็อค Level 2
            Debug.Log("Level 2 unlocked!");
        }
        else if (level == 2)
        {
            PlayerPrefs.SetInt(Level3Key, 1); // ปลดล็อค Level 3
            Debug.Log("Level 3 unlocked!");
        }
        PlayerPrefs.Save(); // บันทึกการเปลี่ยนแปลง
    }
    
    public void ExitGame()
    {
        PlayerPrefs.DeleteAll(); // ลบข้อมูลทั้งหมดใน PlayerPrefs
        PlayerPrefs.Save(); // บันทึกการเปลี่ยนแปลง
        
        Debug.Log("ออกจากเกม");
        Application.Quit(); // ออกจากเกม

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // หยุดเล่นใน Unity Editor
        #endif
    }

    public void GoTomenu()
    {
        SceneManager.LoadScene("menu"); // ไปที่ UIMapLevel Scene
    }

}