using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NormalTower : MonoBehaviour
{
    [SerializeField] public float range = 5f; // ระยะการโจมตีของ Tower (ระยะที่สามารถโจมตีศัตรูได้)
    [SerializeField] public float fireRate = 1f; // อัตราการยิงของ Tower (ยิงกระสุนได้ทุกๆ กี่วินาที)
    public GameObject bulletPrefab; // Prefab ของกระสุนที่ Tower จะยิง
    [SerializeField] float fireCountdown;  // ตัวแปรนับถอยหลังเวลาสำหรับการยิงครั้งถัดไป (ใช้ร่วมกับ fireRate)

    [SerializeField] public int towerCost; // ราคาของ Tower
    //private MoneyManager moneyManager;
    [SerializeField] public float damage;  // กำหนดดาเมจเริ่มต้นสำหรับ Tower นี้
    [SerializeField] public int upgradeLevel = 0; // เลเวลของการอัปเกรด (เริ่มต้นที่ 0)

    public GameObject sellButtonUI; // UI ของปุ่ม Sell
    private float mouseExitTime = 1f; // เวลา 1 วินาที
    private bool isMouseOver = false; // ติดตามว่าเมาส์อยู่บน Tower หรือไม่
    private bool isTowerPlaced = false; // ตรวจสอบว่า Tower ถูกวางหรือยัง
    public GameObject Ring; // วงแสดงระยะการยิง

    public GameObject upgradeButton; // ปุ่มอัพเกรด UI
    private MoneyManager moneyManager; 
    
    //-------------------------------------------------------------
    // ช่อง Text สำหรับแสดงข้อมูลต่างๆ
    [SerializeField] private TextMeshProUGUI levelText;  // เปลี่ยนเป็น TextMeshProUGUI
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI fireRateText;
    [SerializeField] private TextMeshProUGUI towerCostText;
    
    public GameObject closeButton; // ปุ่มปิด UI
    public GameObject towerUI; // Canvas UI ของ Tower
    //-------------------------------------------------------------
    
    
    //[SerializeField] private Sprite towerLv1Sprite;  // Sprite สำหรับ Level 1
    [SerializeField] private Sprite towerLv2Sprite;  // Sprite สำหรับ Level 2
    [SerializeField] private Sprite towerLv3Sprite;  // Sprite สำหรับ Level 3
    private SpriteRenderer towerRenderer;
    
    [SerializeField] public AudioClip shootSound;  // เสียงยิงกระสุน
    private AudioSource audioSource;  // AudioSource สำหรับเล่นเสียง
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();  // หา AudioSource ที่ติดตั้งกับ Tower
        
        // หา SpriteRenderer ที่ติดตั้งกับ Tower
        towerRenderer = GetComponent<SpriteRenderer>();
        moneyManager = FindObjectOfType<MoneyManager>(); // ค้นหา MoneyManager ใน Scene
        sellButtonUI.SetActive(false); // ตั้ง UI ให้ไม่แสดงตอนเริ่มต้น
        if (Ring != null)
        {
            Ring.SetActive(false); // ซ่อนวงแสดงระยะการยิงตอนเริ่มต้น
        }
        
        // แสดงข้อมูล Tower ใน UI ตอนเริ่มต้น
        UpdateTowerInfoUI();
        towerUI.SetActive(false); // ซ่อน UI ตอนเริ่มต้น
    }

    private void Update()
    {
        
        if (!isTowerPlaced)
        {
            return; // ถ้า Tower ยังไม่ถูกวาง จะหยุดการทำงานของ Update
        }
        
        Enemy target = GetNearestEnemy();
        if (target != null && fireCountdown <= 0f)
        {
            Shoot(target);
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;

        if (isMouseOver) // ถ้าเมาส์อยู่บน Tower
        {
            // เมื่อเมาส์อยู่บน Tower จะให้แสดงปุ่ม Sell
            sellButtonUI.SetActive(true);
            upgradeButton.SetActive(true);
            //StopCoroutine(HideSellButton());  // หยุดการทำงานซ่อน UI
        }
        else
        {
            // ถ้าเมาส์ออกจาก Tower จะตั้งเวลา 1 วินาที
            //StartCoroutine(HideSellButton());
        }
    }

    /*// ฟังก์ชันซ่อน UI ปุ่ม Sell หลังจาก 1 วินาที
    private IEnumerator HideSellButton()
    {
        yield return new WaitForSeconds(mouseExitTime);
        sellButtonUI.SetActive(false);  // ซ่อน UI
        upgradeButton.SetActive(false);
    }*/

    // ฟังก์ชันเมื่อคลิกที่ Tower
    private void OnMouseDown()
    {
        if (!isTowerPlaced)
            return;

        // เรียกใช้ฟังก์ชัน ShowTowerUI เมื่อคลิกที่ Tower
        ShowTowerUI();
    }
    // ฟังก์ชันที่เรียกใช้เมื่อคลิกที่ Tower เพื่อแสดง UI
    public void ShowTowerUI()
    {
        towerUI.SetActive(true); // แสดง Canvas UI
        sellButtonUI.SetActive(true); // แสดงปุ่ม Sell
        upgradeButton.SetActive(true); // แสดงปุ่ม Upgrade
        closeButton.SetActive(true); // แสดงปุ่มปิด
    }

    public void CloseTowerUI()
    {
        towerUI.SetActive(false); // ซ่อน Canvas UI
        sellButtonUI.SetActive(false); // ซ่อนปุ่ม Sell
        upgradeButton.SetActive(false); // ซ่อนปุ่ม Upgrade
        closeButton.SetActive(false); // ซ่อนปุ่มปิด
        
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound();
        }
    }
    
    // ฟังก์ชันการคลิก Tower เพื่อให้แสดง UI
    private void OnMouseOver()
    {
        isMouseOver = true;
        if (Ring != null)
        {
            Ring.SetActive(true); // แสดงวงแสดงระยะการยิง
        }
        Debug.Log("Mouse is over the tower");
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
        if (Ring != null)
        {
            Ring.SetActive(false); // ซ่อนวงแสดงระยะการยิง
        }
    }

    // ฟังก์ชันในการวาง Tower
    public void PlaceTower()
    {
        isTowerPlaced = true; // ตั้งค่าให้ Tower ถูกวางแล้ว
        //sellButtonUI.SetActive(false); // ตั้ง UI ของปุ่ม Sell ให้ไม่แสดงในตอนแรก
        //upgradeButton.SetActive(false);
        
        // กำหนด Offset เพื่อปรับตำแหน่งการวาง Tower
        Vector3 offset = new Vector3(0, -0f, 0); // เลื่อนลง 0.5 หน่วย (ปรับค่าตาม Sprite)
        transform.position += offset;
        
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayPlaceTowerSound();
        }
        
        Debug.Log("Tower has been placed.");
    }
    
    Enemy GetNearestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Enemy nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    void Shoot(Enemy target)
    {
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Seek(target);
            bullet.SetDamage(damage);  // เรียก SetDamage เพื่อกำหนดค่าดาเมจให้กระสุน
        }
        // เล่นเสียงยิงกระสุนจาก SoundManager
        if (SoundManager.instance != null && SoundManager.instance.towerShootSound != null)
        {
            SoundManager.instance.sfxSource.PlayOneShot(SoundManager.instance.towerShootSound);
        }
    }

    public void SellTower()
    {
        if (moneyManager != null)
        {
            int refundAmount = towerCost / 2;
            moneyManager.AddMoney(refundAmount);
            Destroy(gameObject); 
            Debug.Log("ขายป้อมสำเร็จ! ได้เงินคืน: " + refundAmount);
        }
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySellTowerSound();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // ตั้งสีของ Gizmos (วงแสดงระยะการยิง)
        Gizmos.color = Color.red;

        // วาดวงกลมรอบ Tower ในระยะที่กำหนดไว้โดย range
        Gizmos.DrawWireSphere(transform.position, range);
    }
    
    
    private void UpdateTowerInfoUI()
    {
        // อัปเดตข้อมูลที่ UI
        levelText.text = "Level: " + (upgradeLevel + 1).ToString();
        damageText.text = "Damage: " + damage.ToString();
        rangeText.text = "Range: " + range.ToString();
        fireRateText.text = "FireRate: " + fireRate.ToString("F1");
        towerCostText.text = "Cost: " + towerCost.ToString();
    }
    
    
    public void UpgradeTower()
    {
        if (upgradeLevel >= 2) // ตรวจสอบว่า Tower อัปเกรดได้สูงสุดแค่ 2 ครั้ง
        {
            Debug.Log("Tower has reached max upgrade level.");
            return;
        }

        // เช็คยอดเงินของผู้เล่นก่อนทำการอัพเกรด
        int upgradeCost = upgradeLevel == 0 ? 100 : 150; // เลือกราคาอัพเกรด
        if (moneyManager.GetCurrentMoney() >= upgradeCost)
        {
            moneyManager.AddMoney(-upgradeCost); // ลดเงินของผู้เล่น

            // เพิ่มค่าคุณสมบัติต่างๆ ตามเลเวลอัปเกรด
            if (upgradeLevel == 0) // อัพเกรดจาก Lv1
            {
                damage += 7f;
                range += 2f;
                fireRate += 2f;  // เพิ่ม fireRate สำหรับการอัปเกรดครั้งที่ 1
                towerCost += 100; // เพิ่มราคา Tower หลังอัปเกรด
                // เปลี่ยน Sprite เป็น Lv2
                towerRenderer.sprite = towerLv2Sprite;
            }
            else if (upgradeLevel == 1) // อัพเกรดจาก Lv2
            {
                damage += 10f;
                range += 4f;
                fireRate += 4f;  // เพิ่ม fireRate สำหรับการอัปเกรดครั้งที่ 2
                towerCost += 150;
                // เปลี่ยน Sprite เป็น Lv3
                towerRenderer.sprite = towerLv3Sprite;
            }

            // ปรับ fireCountdown ตามค่า fireRate ใหม่
            fireCountdown = 1f / fireRate;  // ตั้งค่า fireCountdown ให้สมดุลกับ fireRate

            upgradeLevel++; // เพิ่มระดับอัปเกรด
            Debug.Log($"Tower upgraded to level {upgradeLevel}. New damage: {damage}, new range: {range}, new fireRate: {fireRate}");

            // อัปเดตข้อมูล UI หลังการอัปเกรด
            UpdateTowerInfoUI();
            
            // ปิดปุ่มอัพเกรดหลังจากการอัปเกรด
            //upgradeButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Not enough money to upgrade the tower!");
        }
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayPlaceTowerSound();
        }
    }
    
    
    public void ActivateSkill()
    {
        StartCoroutine(SkillCoroutine());
    }

    public IEnumerator SkillCoroutine()
    {
        // เก็บค่าดาเมจปัจจุบันไว้
        float originalDamage = damage;

        // คูณค่าดาเมจปัจจุบัน 2 เท่า
        damage *= 2;
        Debug.Log("Skill activated! Damage doubled. Current damage: " + damage);

        float skillDuration = 10f; // ระยะเวลาที่สกิลมีผล (10 วินาที)
        yield return new WaitForSeconds(skillDuration);

        // คืนค่าดาเมจกลับเป็นปกติ (ใช้ค่าที่เก็บไว้)
        damage = originalDamage;
        Debug.Log("Skill effect ended. Damage reset. Current damage: " + damage);
    }

}

