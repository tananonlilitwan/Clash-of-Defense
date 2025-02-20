using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GamePauseButton : MonoBehaviour
{
    private bool isGamePaused = false;
    public Button pauseButton;     // ปุ่มสำหรับหยุดเกม
    public Button backButton;      // ปุ่มสำหรับกลับไปที่เกม
    public Button restartButton;  // ปุ่มสำหรับเริ่มใหม่
    public Button Level_Page;   
    public Canvas settingsCanvas;  // Canvas สำหรับ Setting
    private CanvasGroup settingsCanvasGroup;
    void Start()
    {
        // รับ CanvasGroup ของ Settings Canvas
        settingsCanvasGroup = settingsCanvas.GetComponent<CanvasGroup>();

        // ตรวจสอบว่ามี CanvasGroup อยู่ใน Settings Canvas หรือไม่
        if (settingsCanvasGroup == null)
        {
            settingsCanvasGroup = settingsCanvas.gameObject.AddComponent<CanvasGroup>();
        }
        
        // ตั้งค่าปุ่ม Pause
        pauseButton.onClick.AddListener(OpenSettings);
        
        // ตั้งค่าปุ่ม Back
        backButton.onClick.AddListener(CloseSettings);
        
        restartButton.onClick.AddListener(RestartGame);

        // ซ่อน Canvas Setting เมื่อเริ่มเกม
        settingsCanvas.gameObject.SetActive(false);
        
        // ซ่อน Canvas Setting และตั้งค่าบล็อก Raycasts
        CloseSettings();
    }

    void OpenSettings()
    {
        Time.timeScale = 0;  // หยุดเวลาเกม
        isGamePaused = true;
        settingsCanvas.gameObject.SetActive(true);  // แสดง Canvas Setting
        settingsCanvasGroup.blocksRaycasts = true; // เปิดการบล็อกคลิกทะลุ
        Debug.Log("Game Paused and Settings Opened");
    }

    void CloseSettings()
    {
        Time.timeScale = 1;  // กลับมาเล่นเกม
        isGamePaused = false;
        settingsCanvas.gameObject.SetActive(false);  // ซ่อน Canvas Setting
        settingsCanvasGroup.blocksRaycasts = false; // ปิดการบล็อกคลิกทะลุ
        Debug.Log("Game Resumed and Settings Closed");
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isGamePaused = false;
        settingsCanvas.gameObject.SetActive(false);  // ซ่อน Canvas Setting
        settingsCanvasGroup.blocksRaycasts = false; // ปิดการบล็อกคลิกทะลุ
        Time.timeScale = 1;  // กลับมาเล่นเกม
        Debug.Log("เกมกำลังรีเซ็ต...");
    }
}
