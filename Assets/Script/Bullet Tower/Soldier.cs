using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public float speed = 10f;
    private Enemy target;
    
    private int damage;  // damage เป็น int
    public int bulletHP = 3; // HP ของกระสุน เริ่มต้นที่ 3 หน่วย
    private bool isDamaging = false; // ใช้เพื่อควบคุมการลด HP แบบต่อเนื่อง
    private float damageCooldown = 0.1f; // เวลาคูลดาวน์ระหว่างชน
    private float lastDamageTime = 0f;
    
    
    private int level = 1; // ระดับของ Soldier
    private int maxLevel = 2; // จำนวนระดับสูงสุดที่สามารถอัปเกรดได้

    // ค่าพื้นฐานสำหรับ Level 1
    public int baseHP = 100;
    public int baseDamage = 2;

    // ค่าของ Level 2
    public int upgradedHP = 200;
    public int upgradedDamage = 10;


    private void Start()
    {
        //SoldierRenderer= GetComponent<SpriteRenderer>();
        if (SoldierRenderer == null)
        {
            SoldierRenderer = GetComponent<SpriteRenderer>();
            if (SoldierRenderer == null)
            {
                Debug.LogError("SpriteRenderer is not assigned to Soldier.");
            }
        }
        UpdateSoldierStats();
    }

    // ฟังก์ชันนี้จะถูกเรียกจาก Tower เพื่อกำหนดดาเมจให้กับกระสุน
    public void SetDamage(float damageAmount)
    {
        damage = Mathf.RoundToInt(damageAmount);  // แปลงจาก float เป็น int
    }
    
    public void Seek(Enemy _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f&& !isDamaging)
        {
            //HitTarget();
            StartCoroutine(DamageOverTime()); // เริ่ม Coroutine เมื่อชนกับศัตรู
        }
    }

    /*void HitTarget()
    {
        /*target.TakeDamage(damage);  // ใช้ damage ที่เป็น int
        Destroy(gameObject);#1#
        
        if (target != null)
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                target.TakeDamage(damage);
                bulletHP--;
                lastDamageTime = Time.time;

                Debug.Log("Bullet HP: " + bulletHP);

                if (bulletHP <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }*/
    
    private IEnumerator DamageOverTime()
    {
        isDamaging = true; // ตั้ง flag เพื่อไม่ให้เรียกหลายครั้ง

        // ลด HP ของศัตรูและกระสุนทุก 1.5 วินาทีจนกว่ากระสุนหรือศัตรูจะหมด HP
        while (bulletHP > 0 && target != null)
        {
            target.TakeDamage(damage); // ลด HP ของศัตรู
            bulletHP--; // ลด HP ของกระสุน

            Debug.Log("Bullet HP: " + bulletHP); // แสดงค่า HP ของกระสุน

            yield return new WaitForSeconds(2f); // รอ 1.5 วินาที ก่อนลด HP ครั้งถัดไป
        }

        Destroy(gameObject); // ลบกระสุนถ้า HP หมด
    }
    
    // ฟังก์ชันอัปเกรด Soldier
    public void Upgrade()
    {
        if (level < maxLevel) // ตรวจสอบว่า Soldier ยังสามารถอัปเกรดได้
        {
            level++; // เพิ่มระดับ Soldier

            // อัปเกรดค่าพื้นฐานของ Soldier ตามระดับ
            UpdateSoldierStats();

            Debug.Log("Soldier upgraded to level " + level);
        }
        else
        {
            Debug.Log("Soldier is already at max level.");
        }
    }

    
    [SerializeField] private Sprite SoldierLv2Sprite;  // Sprite สำหรับ Level 2
    [SerializeField] private Sprite SoldierLv3Sprite;  // Sprite สำหรับ Level 3
    public SpriteRenderer SoldierRenderer;
    
    // ฟังก์ชันอัปเดตค่าของ Soldier ตามระดับ
    public void UpdateSoldierStats()
    {
        if (SoldierRenderer == null)
        {
            Debug.LogError("SoldierRenderer is still not assigned.");
            return;
        }
        
        if (level == 1)
        {
            bulletHP = baseHP;  // กำหนด HP สำหรับ Lv1
            damage = baseDamage;  // กำหนด Damage สำหรับ Lv1
            SoldierRenderer.sprite = SoldierLv2Sprite;
        }
        else if (level == 2)
        {
            bulletHP = upgradedHP;  // กำหนด HP สำหรับ Lv2
            damage = upgradedDamage;  // กำหนด Damage สำหรับ Lv2
            SoldierRenderer.sprite = SoldierLv3Sprite;
        }
        Debug.Log("Soldier stats updated. Level: " + level);
    }
}
