using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; // ใช้สำหรับ IPointerClickHandler

public class Enemy1 : MonoBehaviour
{
    [SerializeField] public float speed;
    private int waypointIndex = 0;
    private Path1 path;

    [SerializeField] int health;
    
    public int rewardMoney; // จำนวนเงินที่จะได้รับเมื่อกำจัดศัตรู
    private MoneyManager moneyManager;
    
    public TextMeshProUGUI hpText;  // ตัวแปรใหม่เพื่อแสดงค่า HP บน UI
    private House targetHouse;  // เป้าหมายที่ต้องการทำให้เสียหาย (บ้าน)
    [SerializeField] public int damage_house;  // กำหนดความเสียหายที่ศัตรูจะทำให้บ้าน
    
    private float originalSpeed;  // ความเร็วเดิมของศัตรู
    
    private void Start()
    {
        originalSpeed = speed;
        
        path = FindObjectOfType<Path1>();
        transform.position = path.GetWaypoint(waypointIndex).position;
        moneyManager = FindObjectOfType<MoneyManager>(); // ค้นหา MoneyManager ใน Scene
        targetHouse = path.GetTargetHouse(); // ตั้งเป้าหมายเป็นบ้านจาก Path
        // ตรวจสอบว่า hpText ถูกตั้งค่าใน Inspector หรือไม่
        if (hpText != null)
        {
            UpdateHealthText();  // อัพเดตข้อความ HP ตอนเริ่มต้น
        }
    }

    public void SlowDown()
    {
        // ลดความเร็วของศัตรูลง 50%
        speed = originalSpeed * 0.5f;  // ลดความเร็วเหลือ 50%   peed = originalSpeed * 0.25f;  // ลดความเร็วเหลือ 25% ของความเร็วเดิม
        StartCoroutine(ResetSpeed());
    }

    IEnumerator ResetSpeed()
    {
        // รอ 1 วินาที แล้วรีเซ็ทความเร็ว
        yield return new WaitForSeconds(1f);
        speed = originalSpeed;
    }
    
    private void Update()
    {
        //MoveAlongPath();
        // ตรวจสอบว่าไปถึง Waypoint สุดท้ายแล้วหรือยัง
        if (waypointIndex < path.WaypointCount)
        {
            MoveAlongPath(); // เคลื่อนที่ตามทาง Waypoints
        }
        else
        {
            MoveToHouse(); // เมื่อถึง Waypoint สุดท้าย ให้เคลื่อนที่ไปบ้าน
        }
    }

    void MoveAlongPath()
    {
        if (waypointIndex < path.WaypointCount)
        {
            // ค้นหา Waypoint เป้าหมายถัดไป
            Transform targetWaypoint = path.GetWaypoint(waypointIndex);
            // คำนวณทิศทางที่ศัตรูควรเคลื่อนที่ไป
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;
            // เคลื่อนที่ไปในทิศทางของ Waypoint เป้าหมาย
            transform.position += direction * speed * Time.deltaTime;

            // ถ้า Enemy มาถึง Waypoint เป้าหมายแล้ว ให้ไปยัง Waypoint ถัดไป
            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.2f)
            {
                waypointIndex++;
            }
        }
        /*else
        {
            ReachEnd();
        }*/
    }
    void MoveToHouse()
    {
        if (targetHouse != null) // ตรวจสอบว่า targetHouse ถูกกำหนดหรือยัง
        {
            // คำนวณทิศทางไปยังบ้าน
            Vector3 direction = (targetHouse.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // ถ้า Enemy ถึงบ้านแล้ว ให้โจมตี
            if (Vector3.Distance(transform.position, targetHouse.transform.position) < 0.2f)
            {
                AttackHouse();
            }
        }
    }


    /*void ReachEnd()
    {
        Destroy(gameObject);
        // Add code here to reduce player health or take other actions when enemy reaches the end
    }*/
    
    public void TakeDamage(int amount)
    {
       
        health -= amount;
       //int damageInt = (int)damage;
       //health -= damageInt;
        UpdateHealthText(); // เพิ่มการอัพเดตข้อความหลังจากเลือดลด
        if (health <= 0)
        {
            Die();
            
        }
    }

    /*void Die()
    {
        Destroy(gameObject);
    }*/
    
    // ฟังก์ชันเรียกเมื่อตาย
    public void Die()
    {
        if (moneyManager != null)
        {
            moneyManager.AddMoney(rewardMoney); // เพิ่มเงิน
            Debug.Log("ศัตรูตาย! ได้รับเงิน: " + rewardMoney);
        }
        Destroy(gameObject); // ทำลาย Enemy
    }

    // ฟังก์ชันนี้จะอัพเดตข้อความ HP บน UI
    private void UpdateHealthText()
    {
        if (hpText != null)
        {
            hpText.text = "HP: " + health.ToString();  // แสดงค่า HP ของศัตรู
        }
    }
    
    // ทดสอบจำลองการโดนโจมตี
    /*void OnMouseDown() // เมื่อคลิกที่ Enemy ให้เรียกฟังก์ชัน Die
    {
        Die();
    }*/
    
    // สกิล1
    private void OnEnable()
    {
        SkillManager skillManager = FindObjectOfType<SkillManager>();
        if (skillManager != null)
        {
            skillManager.allEnemies1.Add(this); // เพิ่มตัวเองในลิสต์เมื่อเปิดใช้งาน
        }
    }

    private void OnDisable()
    {
        SkillManager skillManager = FindObjectOfType<SkillManager>();
        if (skillManager != null)
        {
            skillManager.allEnemies1.Remove(this); // ลบตัวเองออกจากลิสต์เมื่อปิดใช้งาน
        }
    }
    // สกิล1
    
    // สกิล2
    public void StopMoving(bool stop)
    {
        if (stop)
        {
            // หยุดการเคลื่อนไหว (ตั้งค่าความเร็วเป็น 0)
            speed = 0;
        }
        else
        {
            // คืนค่าความเร็วการเคลื่อนไหวตามปกติ
            speed = originalSpeed;
        }
        Debug.Log("Speed: " + speed); // ดีบักเพื่อดูค่าความเร็ว
    }
    //สกิล2
    
    // ฟังก์ชันนี้จะถูกใช้เพื่อกำหนดบ้านที่ศัตรูต้องการโจมตี
    public void SetTargetHouse(House house)
    {
        targetHouse = house;  // กำหนดเป้าหมายเป็นบ้านที่ได้รับการโจมตี
    }

    // ฟังก์ชันนี้จะถูกใช้เพื่อลด HP ของบ้าน
    public void AttackHouse()
    {
        if (targetHouse != null)
        {
            targetHouse.SetDamage(damage_house);  // เรียกฟังก์ชัน SetDamage จากบ้านเพื่อลด HP
            Destroy(gameObject);  // ทำลายศัตรูเมื่อโจมตีบ้านเสร็จ
        }
    }

    // ตัวอย่างการใช้ฟังก์ชัน Seek ที่ถูกปรับ
    public void Seek(House house)
    {
        SetTargetHouse(house);  // กำหนดบ้านเป็นเป้าหมาย
    }
    
    
    

}
