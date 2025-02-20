using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EnemySpawner1 : MonoBehaviour
{
    public GameObject[] wave1Enemies;  // ศัตรูสำหรับ Wave 1
    public GameObject[] wave2Enemies;  // ศัตรูสำหรับ Wave 2
    public GameObject[] wave3Enemies;  // ศัตรูสำหรับ Wave 3
    public GameObject[] wave4Enemies;  // ศัตรูสำหรับ Wave 4
    public GameObject[] wave5Enemies;  // ศัตรูสำหรับ Wave 5
    public GameObject[] wave6Enemies;  // ศัตรูสำหรับ Wave 6
    public GameObject[] wave7Enemies;  // ศัตรูสำหรับ Wave 7
    public GameObject[] wave8Enemies;  // ศัตรูสำหรับ Wave 8
    
    public TMP_Text waveText;              // ข้อความสำหรับแสดง Wave
    public TMP_Text liveCountdownText;     // ข้อความสำหรับแสดงจำนวนศัตรูที่เหลือ
    public TMP_Text countdownText;         // ข้อความสำหรับนับถอยหลัง
    public Button nextWaveButton;      // ปุ่มสำหรับเริ่ม Wave ถัดไป
    public Button startButton;             // ปุ่มเริ่มต้น

    public Transform spawnPoint;       // จุดที่ศัตรูจะถูกสปอน
    [SerializeField] float timeBetweenWaves;  // เวลาระหว่าง Wave
    [SerializeField] float timeBetweenEnemies; // เวลาระหว่างการสปอนแต่ละตัว
    private int currentWave = 0;          // เก็บหมายเลขของ Wave ปัจจุบัน
    
    private GameObject[] currentWaveEnemies; // ศัตรูสำหรับ Wave ปัจจุบัน
    private int remainingEnemies;        // จำนวนศัตรูที่เหลือใน Wave ปัจจุบัน
    private bool isSpawning = false;     // สถานะว่ากำลังปล่อยศัตรูอยู่หรือไม่

    public House house;  // อ้างอิงถึง House script
    public GameObject[] stars;  // รูปภาพดาวทั้ง 3 ดวง (ต้องเพิ่มใน Inspector)
    
    public GameObject Canvas_TowerIcon;
    public GameObject Canvas_Money;
    public GameObject Canvas_Startgame;
    
    public Canvas winCanvas; // Canvas สำหรับแสดงผลเมื่อชนะ
    
    private int totalEnemiesInCurrentWave; // จำนวนศัตรูทั้งหมดใน Wave ปัจจุบัน
    private int enemiesDefeated;

    public GameObject Effect;
    
    void Start()
    {
        nextWaveButton.onClick.AddListener(StartNextWave);  // ผูกปุ่ม Next Wave
        nextWaveButton.gameObject.SetActive(false);         // ซ่อนปุ่ม Next Wave เริ่มต้น
        startButton.gameObject.SetActive(true);              // แสดงปุ่มเริ่มต้น
        startButton.onClick.AddListener(StartSpawning);      // ผูกปุ่ม Start กับฟังก์ชัน StartSpawning
        winCanvas.gameObject.SetActive(false); // ซ่อน Canvas Win ตอนเริ่มเกม
        UpdateUI();   
    }
    
    void Update()
    {
        CheckIfAllEnemiesAreDestroyed();
    }
    
    public void StartSpawning()
    {
        Effect.SetActive(false);
        if (isSpawning || remainingEnemies > 0) return; // ตรวจสอบสถานะ
        Debug.Log("เริ่มการนับถอยหลังสำหรับ Wave แรก");
        StartCoroutine(CountdownCoroutine()); // เริ่มการนับถอยหลัง
    }

    void StartNextWave()
    {
        if (currentWave >= 8)
        {
            Debug.Log("ไม่มี Wave ถัดไปแล้ว!");
            return;
        }

        if (isSpawning || remainingEnemies > 0) // ถ้ากำลังสปอนหรือยังมีศัตรูอยู่ในเวฟก่อนหน้า
        {
            Debug.Log("ไม่สามารถเริ่ม Wave ถัดไปได้จนกว่า Wave ปัจจุบันจะเสร็จสิ้น!");
            return;
        }
        /*if (isSpawning)  // ถ้ากำลังสปอนศัตรูอยู่ ให้ไม่เริ่มเวฟใหม่
        {
            return;
        }*/
        StartWave();  // เริ่ม Wave ถัดไป
        //StartCoroutine(CountdownAndStartWave(3));  // เริ่มปล่อย Wave ถัดไปหลังจากนับถอยหลัง 3 วินาที
    }
    IEnumerator CountdownAndStartWave(int countdownTime)
    {
        nextWaveButton.gameObject.SetActive(false); // ซ่อนปุ่มระหว่างนับถอยหลัง
        countdownText.gameObject.SetActive(true);    // แสดงข้อความนับถอยหลัง
        for (int i = countdownTime; i > 0; i--)
        {
            waveText.text = "Next Wave in: " + i;
            yield return new WaitForSeconds(1f);
        }

        countdownText.gameObject.SetActive(false); // ซ่อนข้อความนับถอยหลัง
        StartWave();  // เริ่มสปอน
    }
    
    void StartWave()
    {
        nextWaveButton.gameObject.SetActive(false); // ซ่อนปุ่ม Next Wave
        currentWave++;
        currentWaveEnemies = GetEnemiesForWave(currentWave);  // ดึงข้อมูลศัตรูสำหรับ Wave นี้
        remainingEnemies = currentWaveEnemies.Length;         // ตั้งค่าจำนวนศัตรูเริ่มต้นใน Wave นี้
        UpdateUI();
        StartCoroutine(SpawnEnemies());
    }
    
    IEnumerator SpawnEnemies()
    {
        isSpawning = true;
        for (int i = 0; i < currentWaveEnemies.Length; i++)
        {
            SpawnEnemy(currentWaveEnemies[i]);  // สปอนศัตรู
            yield return new WaitForSeconds(timeBetweenEnemies);
        }

        isSpawning = false;
        CheckIfWaveCompleted();  // ตรวจสอบว่า Wave เสร็จแล้วหรือไม่
    }
    
    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position, Quaternion.identity);
        remainingEnemies--;  // ลดจำนวนศัตรูที่เหลือใน Wave
        UpdateUI();

        // ตรวจสอบว่า Wave สุดท้ายเสร็จสมบูรณ์หรือยัง
        if (remainingEnemies <= 0 && currentWave == 8)
        {
            Debug.Log("Wave สุดท้ายเสร็จสิ้น!");
            CheckIfAllEnemiesAreDestroyed(); // ตรวจสอบว่าไม่มีศัตรูเหลือใน Scene
        }
    }

    void CheckIfWaveCompleted()
    {
        if (!isSpawning && remainingEnemies <= 0)
        {
            Debug.Log("Wave " + currentWave + " เสร็จสิ้น!");
            if (currentWave < 8)
            {
                
                nextWaveButton.gameObject.SetActive(true); // แสดงปุ่ม Next Wave
            }
            else
            {
                Debug.Log("ถึง Wave สุดท้ายแล้ว!");
                nextWaveButton.gameObject.SetActive(false); // ซ่อนปุ่มเมื่อถึง Wave สุดท้าย
                
            }
            if (currentWave >= 8) // ถ้าเป็น Wave สุดท้าย
            {
                Debug.Log("เกมชนะ!");
               // ShowWinCanvas(); // แสดง Canvas Win
                return;
            }

            Debug.Log("Wave " + currentWave + " เสร็จสิ้น!");
            nextWaveButton.gameObject.SetActive(true);
        }
        else
        {
            nextWaveButton.gameObject.SetActive(false); // ซ่อนปุ่มถ้ายังมีศัตรูเหลือ
        }
    }
    GameObject[] GetEnemiesForWave(int waveNumber)
    {
        switch (waveNumber)
        {
            case 1: return wave1Enemies;
            case 2: return wave2Enemies;
            case 3: return wave3Enemies;
            case 4: return wave4Enemies;
            case 5: return wave5Enemies;
            case 6: return wave6Enemies;
            case 7: return wave7Enemies;
            case 8: return wave8Enemies;
            default: return new GameObject[0];
        }
    }

    void UpdateUI()
    {
        waveText.text = "Wave: " + currentWave;
        liveCountdownText.text = "Live: " + remainingEnemies;
    }
    
    IEnumerator SpawnWaves()
    {
        while (currentWave < 8)  // ทำให้มี 3 Wave
        {
            currentWave++;  // เพิ่มหมายเลขของ Wave
            Debug.Log("Wave " + currentWave + " เริ่มแล้ว!");

            GameObject[] currentWaveEnemies = GetEnemiesForWave(currentWave);  // เลือกศัตรูจาก Array สำหรับแต่ละ Wave
            
            for (int i = 0; i < currentWaveEnemies.Length; i++)
            {
                SpawnEnemy(currentWaveEnemies[i]);  // สปอนศัตรูที่เลือก
                yield return new WaitForSeconds(timeBetweenEnemies); // เวลาระหว่างการสปอนแต่ละตัว
            }

            yield return new WaitForSeconds(timeBetweenWaves);  // เวลาระหว่างการเริ่ม Wave ถัดไป
        }

        Debug.Log("สปอนเสร็จสิ้นแล้ว!");
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
        nextWaveButton.gameObject.SetActive(true);
        StartNextWave();                           // เริ่มต้น Wave แรก
        //StartCoroutine(SpawnWaves());               // เริ่มสปอนศัตรู
    }
    
    void ShowWinCanvas()
    {
        winCanvas.gameObject.SetActive(true); // แสดง Canvas Win
        Canvas_TowerIcon.SetActive(false);
        Canvas_Startgame.SetActive(false);
        Canvas_Money.SetActive(false);

        int starCount = 0; // จำนวนดาวเริ่มต้น
        if (house != null)
        {
            if (house.houseHP == 100)
            {
                starCount = 3; // แสดงดาว 3 ดวง
            }
            else if (house.houseHP >= 50)
            {
                starCount = 2; // แสดงดาว 2 ดวง
            }
            else if (house.houseHP > 0)
            {
                starCount = 1; // แสดงดาว 1 ดวง
            }
            else
            {
                starCount = 0; // แสดงไม่มีดาว
            }
        }

        ShowStars(starCount); // อัปเดตดาวใน UI
        Time.timeScale = 0; // หยุดเกม
    }

    
    // ฟังก์ชันสำหรับเปิดใช้งานรูปภาพดาว
    void ShowStars(int starCount)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(i < starCount); // เปิดใช้งานดาวตามจำนวนที่ต้องการ
        }
    }
    void CheckIfAllEnemiesAreDestroyed()
    {
        GameObject[] enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemiesInScene.Length == 0 && currentWave == 8)
        {
            Debug.Log("ไม่มีศัตรูใน Scene แล้ว! เกมชนะ!");
            ShowWinCanvas();

            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                // ตรวจสอบ Level ปัจจุบันและปลดล็อค Level ถัดไป
                Scene currentScene = SceneManager.GetActiveScene();
            
                if (currentScene.name == "Level1")
                {
                    gameManager.UnlockNextLevel(1); // ปลดล็อค Level2
                }
                else if (currentScene.name == "Level2")
                {
                    gameManager.UnlockNextLevel(2); // ปลดล็อค Level3
                }
            }
            else
            {
                Debug.LogError("GameManager reference not found!");
            }

            UIMapManager uiMapManager = FindObjectOfType<UIMapManager>();
            if (uiMapManager != null)
            {
                uiMapManager.UnlockLevelButton(); // รีเฟรชปุ่มใน UIMap
            }
            else
            {
                Debug.LogError("UIMapManager reference not found!");
            }
        }
    }
}
