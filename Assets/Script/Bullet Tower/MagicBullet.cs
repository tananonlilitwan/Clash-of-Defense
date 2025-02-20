using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : MonoBehaviour
{
    public float speed = 10f;
    private Enemy target;
    
    private int damage;  // damage เป็น int
    

    [SerializeField] private Sprite MagicBulletLv2Sprite;  // Sprite สำหรับ Level 2
    [SerializeField] private Sprite MagicBulletLv3Sprite;  // Sprite สำหรับ Level 3
    public SpriteRenderer MagicBulletRenderer;
    [SerializeField] public int upgradeLevel = 0; // เลเวลของการอัปเกรด (เริ่มต้นที่ 0)
    
    private void Start()
    {
        MagicBulletRenderer= GetComponent<SpriteRenderer>();
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

        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        target.TakeDamage(damage);  // ใช้ damage ที่เป็น int
        Destroy(gameObject);
    }
    
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
            MagicBulletRenderer.sprite = MagicBulletLv2Sprite;  // เปลี่ยน Sprite เป็น Lv2
        }
        else if (upgradeLevel == 1) // อัพเกรดจาก Lv2
        {
            MagicBulletRenderer.sprite = MagicBulletLv3Sprite; // เปลี่ยน Sprite เป็น Lv3
        }

        upgradeLevel++; // เพิ่มระดับอัปเกรด
        Debug.Log($"Bomb upgraded to level {upgradeLevel}.");
        
    }
}
