using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float speed;          // ความเร็วของกระสุน
    private Enemy target;             // เป้าหมาย
    private int damage;               // ความเสียหายแบบ int
    [SerializeField] float explosionRadius; // รัศมีการระเบิด

    [SerializeField] private Sprite bombLv2Sprite;  // Sprite สำหรับ Level 2
    [SerializeField] private Sprite bombLv3Sprite;  // Sprite สำหรับ Level 3
    public SpriteRenderer bombRenderer;

    private void Start()
    {
        bombRenderer= GetComponent<SpriteRenderer>();
        // ตรวจสอบให้แน่ใจว่า bombRenderer และ sprites ถูกกำหนด
        if (bombRenderer == null)
        {
            Debug.LogError("bombRenderer is not assigned!");
        }

        if (bombLv2Sprite == null || bombLv3Sprite == null)
        {
            Debug.LogError("Bomb level sprites are not assigned!");
        }
    }

    // ฟังก์ชันนี้จะถูกเรียกจาก Tower เพื่อกำหนดดาเมจให้กับกระสุน
    public void SetDamage(float damageAmount)
    {
        damage = Mathf.RoundToInt(damageAmount);
        Debug.Log("Bullet damage set to: " + damage);
    }

    public void Seek(Enemy _target)
    {
        target = _target;
        Debug.Log("Bullet seeking target: " + target.name);
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

        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
        {
            Explode(); // เรียกฟังก์ชันการระเบิด
        }
    }

    void Explode()
    {
        // ค้นหา Enemy ทุกตัวในเกม (เช่น จาก Tag หรือการจัดเก็บใน List)
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            // คำนวณระยะห่างระหว่างระเบิดและ Enemy
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            // ตรวจสอบว่าศัตรูอยู่ในระยะการระเบิดหรือไม่
            if (distance <= explosionRadius)
            {
                enemy.TakeDamage(damage); // สร้างความเสียหายให้ศัตรูในระยะ
                Debug.Log($"Enemy {enemy.name} took {damage} damage!");
            }
        }

        // แสดงเอฟเฟกต์การระเบิด (ถ้ามี)
        Debug.Log("Explosion at position: " + transform.position);
        Destroy(gameObject); // ทำลายกระสุน
    }

    private void OnDrawGizmosSelected()
    {
        Debug.Log($"Drawing Gizmos at position: {transform.position}");
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    
    [SerializeField] public int upgradeLevel = 0; // เลเวลของการอัปเกรด (เริ่มต้นที่ 0)
    public void Upgrade()
    {
        if (upgradeLevel >= 2) // ตรวจสอบว่า Bomb อัปเกรดได้สูงสุดแค่ 2 ครั้ง
        {
            Debug.Log("Bomb has reached max upgrade level.");
            return;
        }

        // เปลี่ยน Sprite ตามเลเวลการอัปเกรด
        if (upgradeLevel == 0) // อัพเกรดจาก Lv1
        {
            bombRenderer.sprite = bombLv2Sprite;  // เปลี่ยน Sprite เป็น Lv2
        }
        else if (upgradeLevel == 1) // อัพเกรดจาก Lv2
        {
            bombRenderer.sprite = bombLv3Sprite;  // เปลี่ยน Sprite เป็น Lv3
        }

        upgradeLevel++; // เพิ่มระดับอัปเกรด
        Debug.Log($"Bomb upgraded to level {upgradeLevel}.");
        
    }



}