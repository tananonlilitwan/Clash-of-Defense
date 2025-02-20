using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // เพิ่มการใช้งาน TextMeshPro

public class NewBehaviourScript2 : MonoBehaviour
{
    public TextMeshProUGUI countdownText;  // เปลี่ยนเป็น TextMeshProUGUI
    public EnemySpawner enemySpawner;  // สคริปต์ที่ใช้ในการปล่อยศัตรู
    public Button startButton;  // ปุ่มเริ่มเกม
    public Button NextWave;  // ปุ่มเริ่มเกม
    public Button nextWaveButton;  // ปุ่มสำหรับเริ่ม wave ถัดไป
    
    private void Start()
    {
        startButton.gameObject.SetActive(true);  // ซ่อนปุ่มเมื่อเริ่มเกม
        startButton.onClick.AddListener(StartGame);  // เชื่อมโยงฟังก์ชันกับปุ่มเริ่มเกม
        countdownText.gameObject.SetActive(true);  // ซ่อนข้อความนับเลขเริ่มต้น
        NextWave.gameObject.SetActive(false);
        
    }

    // ฟังก์ชันเริ่มเกมเมื่อกดปุ่มเริ่มเกม
    private void StartGame()
    {
        countdownText.gameObject.SetActive(true);  // แสดงข้อความนับเลข
        StartCoroutine(CountdownCoroutine());  // เริ่ม Coroutine สำหรับการนับเลข
        startButton.gameObject.SetActive(false);  // ซ่อนปุ่มเมื่อเริ่มเกม
    }

    // Coroutine สำหรับการนับเลข
    private IEnumerator CountdownCoroutine()
    {
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();  // แสดงตัวเลขใน UI
            yield return new WaitForSeconds(1f);  // รอ 1 วินาที
        }

        countdownText.gameObject.SetActive(false);  // ซ่อนข้อความนับเลข
        startButton.gameObject.SetActive(false);
        NextWave.gameObject.SetActive(true);
        enemySpawner.StartSpawning(); // เริ่มปล่อยศัตรู
    }
    // ฟังก์ชันที่ถูกเรียกเมื่อกดปุ่ม "Next Wave"
    private void StartNextWave()
    {
        enemySpawner.StartSpawning();  // เริ่มปล่อยศัตรูจาก EnemySpawner
        nextWaveButton.gameObject.SetActive(false);  // ซ่อนปุ่ม "Next Wave" หลังเริ่มการปล่อยศัตรู
    }
}