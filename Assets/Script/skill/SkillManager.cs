using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;

public class SkillManager : MonoBehaviour
{
    public Button skillButton1;   // ปุ่มสกิล 1
    public float skillCooldown = 20f;  // คูลดาวน์ของสกิล
    private bool isSkillReady = true; // เช็คว่าสกิลพร้อมใช้งานไหม
    public GameObject effectPrefab;  // เอฟเฟกต์เมื่อใช้สกิล
    public float skillRadius = 5f;   // รัศมีของสกิล
    public List<Enemy> allEnemies = new List<Enemy>(); // รายชื่อศัตรูทั้งหมดในเกม
    public List<Enemy1> allEnemies1 = new List<Enemy1>(); // รายชื่อศัตรูทั้งหมดในเกม
    // ตัวแปรสำหรับดาเมจที่สามารถตั้งค่าได้ใน Unity Inspector
    [SerializeField] public int skillDamage;  // ดาเมจที่ใช้กับสกิล
    public TextMeshProUGUI cooldownText; // ช่องข้อความสำหรับแสดงคูลดาวน์
    
    
    
    public Button skillButton2; // ปุ่มสกิลที่ 2
    public float skill2Cooldown = 50f;
    private bool isSkill2Ready = true;
    public float multiplierDuration = 5f;  // ระยะเวลาของการคูณดาเมจ
    public float damageMultiplier = 2f;  // ค่าคูณดาเมจ
    public TextMeshProUGUI cooldownText2;
    
    public GameObject highlightEffect1; // แสงสำหรับปุ่มสกิล 1
    public GameObject highlightEffect2; // แสงสำหรับปุ่มสกิล 2
    
    
    public Button skillButton3; // ปุ่มสกิลที่ 3
    public float skill3Cooldown = 8f; // คูลดาวน์ของสกิลที่ 3
    private bool isSkill3Ready = true; // เช็คว่าสกิลที่ 3 พร้อมใช้งานไหม
    public TextMeshProUGUI cooldownText3; // ช่องข้อความสำหรับแสดงคูลดาวน์สกิลที่ 3
    
    void Start()
    {
        skillButton1.onClick.AddListener(UseSkill); // ผูกปุ่มกับสกิล 1
        skillButton2.onClick.AddListener(UseSkill2); // ผูกปุ่มกับสกิล 2
        skillButton3.onClick.AddListener(UseSkill3);  // ผูกปุ่มกับสกิล 3
        
        cooldownText.gameObject.SetActive(false); // ซ่อนข้อความเริ่มต้น
        cooldownText2.gameObject.SetActive(false); // ซ่อนข้อความเริ่มต้น
        cooldownText3.gameObject.SetActive(false); // ซ่อนข้อความคูลดาวน์สกิล 3
        
        highlightEffect1.GameObject().SetActive(false);
        highlightEffect2.GameObject().SetActive(false);
        
        // เพิ่ม EventTrigger ให้กับปุ่ม
        AddPointerEnterExit(skillButton1, highlightEffect1);
        AddPointerEnterExit(skillButton2, highlightEffect2);
    }
    // ฟังก์ชันเพิ่ม EventTrigger
    private void AddPointerEnterExit(Button button, GameObject effect)
    {
        EventTrigger eventTrigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((eventData) => OnPointerEnter(effect));

        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((eventData) => OnPointerExit(effect));

        eventTrigger.triggers.Add(pointerEnter);
        eventTrigger.triggers.Add(pointerExit);
    }
    // เมื่อเมาส์ไปโดนปุ่ม
    private void OnPointerEnter(GameObject effect)
    {
        effect.SetActive(true); // แสดงแสง
    }

    // เมื่อเมาส์ออกจากปุ่ม
    private void OnPointerExit(GameObject effect)
    {
        effect.SetActive(false); // ซ่อนแสง
    }
    
    void UseSkill()
    {
        if (isSkillReady)
        {
            isSkillReady = false;
            Debug.Log("ใช้สกิล!");

            // เรียกใช้เอฟเฟกต์
            if (effectPrefab != null)
            {
                Instantiate(effectPrefab, transform.position, Quaternion.identity);
            }

            // ค้นหาและทำความเสียหายกับศัตรูในระยะ
            for (int i = 0; i < allEnemies.Count; i++)
            {
                Enemy enemy = allEnemies[i];
                if (enemy == null || !enemy.gameObject.scene.isLoaded) continue; // ข้ามศัตรูที่ถูกทำลายไปแล้ว หรือไม่ใช่อ็อบเจ็กต์ใน Scene

                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= skillRadius)
                {
                    // ใช้ตัวแปร skillDamage ที่ตั้งค่าใน Inspector
                    enemy.TakeDamage(skillDamage); // ทำความเสียหายตามค่าที่ตั้งใน Inspector
                }
            }
            for (int i = 0; i < allEnemies1.Count; i++)
            {
                Enemy1 enemy = allEnemies1[i];
                if (enemy == null || !enemy.gameObject.scene.isLoaded) continue; // ข้ามศัตรูที่ถูกทำลายไปแล้ว หรือไม่ใช่อ็อบเจ็กต์ใน Scene

                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= skillRadius)
                {
                    // ใช้ตัวแปร skillDamage ที่ตั้งค่าใน Inspector
                    enemy.TakeDamage(skillDamage); // ทำความเสียหายตามค่าที่ตั้งใน Inspector
                }
            }

            // เริ่มคูลดาวน์
            StartCoroutine(SkillCooldown());
        }
        else
        {
            Debug.Log("สกิลยังไม่พร้อมใช้งาน!");
        }
    }


    private IEnumerator SkillCooldown()
    {
        /*skillButton1.interactable = false; // ปิดปุ่มระหว่างคูลดาวน์
        yield return new WaitForSeconds(skillCooldown);
        isSkillReady = true;
        skillButton1.interactable = true; // เปิดปุ่มเมื่อพร้อม*/
        
        skillButton1.interactable = false; // ปิดปุ่มระหว่างคูลดาวน์
        cooldownText.gameObject.SetActive(true); // เปิดข้อความเมื่อเริ่มคูลดาวน์

        float remainingCooldown = skillCooldown;

        while (remainingCooldown > 0)
        {
            cooldownText.text = remainingCooldown.ToString("F1"); // แสดงเวลาที่เหลือแบบทศนิยม 1 ตำแหน่ง
            remainingCooldown -= Time.deltaTime; // ลดเวลาคูลดาวน์
            yield return null; // รอให้ผ่านไป 1 เฟรม
        }

        cooldownText.gameObject.SetActive(false); // ปิดข้อความเมื่อคูลดาวน์เสร็จ
        isSkillReady = true;
        skillButton1.interactable = true; // เปิดปุ่มเมื่อพร้อม
    }
    
    void UseSkill2()
    {
        /*if (isSkill2Ready)
        {
            isSkill2Ready = false;
            Debug.Log("ใช้สกิลที่ 2! คูณดาเมจของศัตรู");

            // เรียกใช้ระบบคูณดาเมจให้ศัตรูทั้งหมดในระยะ
            //StartCoroutine(StopEnemyMovement());
            StartCoroutine(Skill2Cooldown());
        }
        else
        {
            Debug.Log("สกิลที่ 2 ยังไม่พร้อมใช้งาน!");
        }*/
        
        if (isSkill2Ready)
        {
            isSkill2Ready = false;
            Debug.Log("ใช้สกิลที่ 2! คูณดาเมจของศัตรู");

            // เรียกใช้ระบบคูณดาเมจให้ศัตรูทั้งหมดในระยะ
            StartCoroutine(Skill2Cooldown());

            // หยุดการเคลื่อนไหวของศัตรูทั้งหมดในระยะ
            StartCoroutine(StopEnemyMovement());
        }
        else
        {
            Debug.Log("สกิลที่ 2 ยังไม่พร้อมใช้งาน!");
        }

    }
    private IEnumerator StopEnemyMovement()
    {
        /*// หยุดการเคลื่อนไหวของศัตรูทั้งหมดในระยะ
        foreach (Enemy enemy in allEnemies)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= skillRadius)
                {
                    enemy.StopMoving(false); // หยุดการเคลื่อนไหว
                    Debug.Log("Enemy stopped moving!");
                }
            }
        }

        yield return new WaitForSeconds(5f); // รอ 5 วินาที

        // ให้ศัตรูกลับมาเคลื่อนไหวได้
        foreach (Enemy enemy in allEnemies)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= skillRadius)
                {
                    enemy.StopMoving(false); // ให้กลับมาเคลื่อนไหวได้
                    Debug.Log("Enemy can move again!");
                }
            }
        }
        
        foreach (Enemy enemy in allEnemies)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= skillRadius)
                {
                    // บันทึกความเร็วเดิมของศัตรูไว้
                    float originalSpeed = enemy.speed;
                
                    // คูณความเร็ว
                    enemy.speed *= damageMultiplier;

                    // รอจนกว่าจะหมดระยะเวลา
                    yield return new WaitForSeconds(multiplierDuration);

                    // คืนค่าความเร็วเดิมให้ศัตรู
                    enemy.speed = originalSpeed;

                    Debug.Log("Applied speed multiplier to enemy!");
                }
            }
        }*/
        
        /*// หยุดการเคลื่อนไหวของศัตรูทั้งหมดในระยะ
        foreach (Enemy enemy in allEnemies)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= skillRadius)
                {
                    enemy.StopMoving(true); // หยุดการเคลื่อนไหว
                    Debug.Log("Enemy stopped moving!");
                }
            }
        }

        yield return new WaitForSeconds(5f); // รอ 5 วินาที

        // ให้ศัตรูกลับมาเคลื่อนไหวได้
        foreach (Enemy enemy in allEnemies)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= skillRadius)
                {
                    enemy.StopMoving(false); // ให้กลับมาเคลื่อนไหวได้
                    Debug.Log("Enemy can move again!");
                }
            }
        }

        // เพิ่มส่วนที่คูณความเร็วให้กับศัตรูภายหลังการหยุด
        foreach (Enemy enemy in allEnemies)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= skillRadius)
                {
                    // บันทึกความเร็วเดิมของศัตรูไว้
                    float originalSpeed = enemy.speed;

                    // คูณความเร็ว
                    enemy.speed *= damageMultiplier;

                    // รอจนกว่าจะหมดระยะเวลา
                    yield return new WaitForSeconds(multiplierDuration);

                    // คืนค่าความเร็วเดิมให้ศัตรู
                    enemy.speed = originalSpeed;

                    Debug.Log("Applied speed multiplier to enemy!");
                }
            }
        }*/
        
        // หยุดการเคลื่อนไหวของศัตรูทั้งหมดในระยะที่อยู่ใน Scene
        foreach (Enemy enemy in allEnemies)
        {
            if (enemy != null && enemy.gameObject.scene.isLoaded) // ตรวจสอบว่าศัตรูอยู่ใน Scene
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= skillRadius)
                {
                    enemy.StopMoving(true); // หยุดการเคลื่อนไหว
                    Debug.Log("Enemy stopped moving!");
                }
            }
        }

        yield return new WaitForSeconds(5f); // รอ 5 วินาที

        // ให้ศัตรูกลับมาเคลื่อนไหวได้
        foreach (Enemy enemy in allEnemies)
        {
            if (enemy != null && enemy.gameObject.scene.isLoaded) // ตรวจสอบว่าศัตรูอยู่ใน Scene
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= skillRadius)
                {
                    enemy.StopMoving(false); // ให้กลับมาเคลื่อนไหวได้
                    Debug.Log("Enemy can move again!");
                }
            }
        }
        
        // หยุดการเคลื่อนไหวของศัตรูทั้งหมดในระยะที่อยู่ใน Scene
        foreach (Enemy1 enemy in allEnemies1)
        {
            if (enemy != null && enemy.gameObject.scene.isLoaded) // ตรวจสอบว่าศัตรูอยู่ใน Scene
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= skillRadius)
                {
                    enemy.StopMoving(true); // หยุดการเคลื่อนไหว
                    Debug.Log("Enemy stopped moving!");
                }
            }
        }

        yield return new WaitForSeconds(5f); // รอ 5 วินาที

        // ให้ศัตรูกลับมาเคลื่อนไหวได้
        foreach (Enemy1 enemy in allEnemies1)
        {
            if (enemy != null && enemy.gameObject.scene.isLoaded) // ตรวจสอบว่าศัตรูอยู่ใน Scene
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= skillRadius)
                {
                    enemy.StopMoving(false); // ให้กลับมาเคลื่อนไหวได้
                    Debug.Log("Enemy can move again!");
                }
            }
        }
    }

   
    private IEnumerator Skill2Cooldown()
    {
        skillButton2.interactable = false;
        cooldownText2.gameObject.SetActive(true);

        float remainingCooldown = skill2Cooldown;

        while (remainingCooldown > 0)
        {
            cooldownText2.text = remainingCooldown.ToString("F1"); // แสดงเวลาที่เหลือแบบทศนิยม 1 ตำแหน่ง
            remainingCooldown -= Time.deltaTime; // ลดเวลาคูลดาวน์
            yield return null; // รอให้ผ่านไป 1 เฟรม
        }

        cooldownText2.gameObject.SetActive(false);
        isSkill2Ready = true;
        skillButton2.interactable = true;
    }



    private void OnDrawGizmosSelected()
    {
        // แสดงรัศมีของสกิลใน Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, skillRadius);
    }
    
    
    [SerializeField] private NormalTower normalTower; // อ้างอิงถึง NormalTower ใน Inspector
    void UseSkill3()
    {
        if (isSkill3Ready)
        {
            isSkill3Ready = false;
            Debug.Log("ใช้สกิลที่ 3!");

            // เรียกใช้ Coroutine ของ NormalTower เมื่อใช้สกิลที่ 3
            StartCoroutine(normalTower.SkillCoroutine());

            StartCoroutine(Skill3Cooldown());
        }
        else
        {
            Debug.Log("สกิลที่ 3 ยังไม่พร้อมใช้งาน!");
        }
    }

    private IEnumerator Skill3Cooldown()
    {
        skillButton3.interactable = false;          // ปิดปุ่มระหว่างคูลดาวน์
        cooldownText3.gameObject.SetActive(true);   // เปิดข้อความคูลดาวน์

        float remainingCooldown = skill3Cooldown;

        while (remainingCooldown > 0)
        {
            cooldownText3.text = remainingCooldown.ToString("F1"); // แสดงเวลาที่เหลือแบบทศนิยม 1 ตำแหน่ง
            remainingCooldown -= Time.deltaTime;                   // ลดเวลาคูลดาวน์
            yield return null;                                     // รอให้ผ่านไป 1 เฟรม
        }

        cooldownText3.gameObject.SetActive(false); // ซ่อนข้อความเมื่อคูลดาวน์เสร็จ
        isSkill3Ready = true;
        skillButton3.interactable = true;          // เปิดปุ่มเมื่อคูลดาวน์เสร็จ
    }
    
}

