using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : MonoBehaviour
{
    public float speed = 10f;  // ความเร็วของกระสุน
    private Enemy target;  // ตัวแปรเก็บศัตรูที่กระสุนจะไปโจมตี
    private int damage;  // ดาเมจที่กระสุนจะทำ
    
    public float slowAmount = 2f;  // ความเร็วที่ลดลง (ลดจากความเร็วปกติ)
    public float slowDuration = 1.8f;  // ระยะเวลาในการช้าลง
    private bool isSlowed = false;  // ตัวแปรใหม่เพื่อตรวจสอบว่าศัตรูถูกชะลอแล้วหรือยัง

    public int upgradeLevel = 0; // ระดับการอัปเกรดเริ่มต้นที่ Lv1

    [SerializeField] private Sprite SlowEffectLv2Sprite;  // Sprite สำหรับ Level 2
    [SerializeField] private Sprite SlowEffectLv3Sprite;  // Sprite สำหรับ Level 3
    public SpriteRenderer SlowEffectRenderer;
    private void Start()
    {
        SlowEffectRenderer= GetComponent<SpriteRenderer>();
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
            SlowDown(target);
        }
    }

    void HitTarget()
    {
        if (!isSlowed)  // เช็คว่าถ้ายังไม่ได้ชะลอความเร็ว
        {
            StartCoroutine(SlowDown(target));  // เรียก Coroutine ที่จะลดความเร็วของศัตรู
            isSlowed = true;  // ตั้งค่าว่าได้ชะลอแล้ว
        }

        target.TakeDamage(damage);  // ใช้ damage ที่เป็น int
        Destroy(gameObject);
    }

      // Coroutine เพื่อลดความเร็วของศัตรูเป็นเวลา 1.8 วินาที
      private IEnumerator SlowDown(Enemy target)
      {
          // ลดความเร็วของศัตรูเมื่อกระสุนชน
          Enemy enemy = target.GetComponent<Enemy>();
          if (enemy != null)
          {
              enemy.SlowDown();
          }

          Destroy(gameObject); // ทำลายกระสุน
    
          yield break; // ออกจาก IEnumerator ทันที
      }
      
      // ฟังก์ชันอัปเกรด
      public void Upgrade()
      {
          if (upgradeLevel == 0)
          {
              damage = 3;
              slowAmount = 0.1f; // ลดความเร็วลง 10%
              slowDuration = 5f;
              SlowEffectRenderer.sprite = SlowEffectLv2Sprite;
          }
          else if (upgradeLevel == 1)
          {
              damage = 5;
              slowAmount = 0.2f; // ลดความเร็วลง 20%
              slowDuration = 5f;
              SlowEffectRenderer.sprite = SlowEffectLv3Sprite;
          }

          upgradeLevel++;
          ApplyUpgradeStats();
      }

      private void ApplyUpgradeStats()
      {
          if (upgradeLevel == 0)
          {
              damage = 3;
              slowAmount = 0.1f; // ลด 10%
              slowDuration = 5f;
          }
          else if (upgradeLevel == 1)
          {
              damage = 5;
              slowAmount = 0.2f; // ลด 20%
              slowDuration = 5f;
          }
      }
      
}
