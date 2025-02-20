using UnityEngine;
using UnityEngine.SceneManagement; // ต้องการใช้ SceneManager

public class GoToUIMapLevel : MonoBehaviour
{
    // ฟังก์ชันนี้จะทำงานเมื่อกดปุ่ม
    public void GoToUIMapLevelScene()
    {
        SceneManager.LoadScene("UIMapLevel"); // เปลี่ยนไปที่ Scene ที่ชื่อ UIMapLevel
    }
}