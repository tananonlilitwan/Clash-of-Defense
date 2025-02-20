using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//              class Tower
public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] public float range = 5f;
    [SerializeField] public float fireRate = 1f;
    public GameObject bulletPrefab;
    private float fireCountdown = 0f;

    [SerializeField] public int towerCost; // ราคาของ Tower
    private MoneyManager moneyManager;
    [SerializeField] public float damage;  // กำหนดดาเมจเริ่มต้นสำหรับ Tower นี้

    public GameObject sellButtonUI; // UI ของปุ่ม Sell
    private float mouseExitTime = 1f; // เวลา 1 วินาที
    private bool isMouseOver = false; // ติดตามว่าเมาส์อยู่บน Tower หรือไม่
    private bool isTowerPlaced = false; // ตรวจสอบว่า Tower ถูกวางหรือยัง
    public GameObject Ring; // วงแสดงระยะการยิง

    void Start()
    {
        moneyManager = FindObjectOfType<MoneyManager>(); // ค้นหา MoneyManager ใน Scene
        sellButtonUI.SetActive(false); // ตั้ง UI ให้ไม่แสดงตอนเริ่มต้น
        if (Ring != null)
        {
            Ring.SetActive(false); // ซ่อนวงแสดงระยะการยิงตอนเริ่มต้น
        }
    }

    private void Update()
    {
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
            StopCoroutine(HideSellButton());  // หยุดการทำงานซ่อน UI
        }
        else
        {
            // ถ้าเมาส์ออกจาก Tower จะตั้งเวลา 1 วินาที
            StartCoroutine(HideSellButton());
        }
    }

    // ฟังก์ชันซ่อน UI ปุ่ม Sell หลังจาก 1 วินาที
    private IEnumerator HideSellButton()
    {
        yield return new WaitForSeconds(mouseExitTime);
        sellButtonUI.SetActive(false);  // ซ่อน UI
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
        sellButtonUI.SetActive(false); // ตั้ง UI ของปุ่ม Sell ให้ไม่แสดงในตอนแรก
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
    }

    private void OnDrawGizmosSelected()
    {
        // ตั้งสีของ Gizmos (วงแสดงระยะการยิง)
        Gizmos.color = Color.red;

        // วาดวงกลมรอบ Tower ในระยะที่กำหนดไว้โดย range
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
