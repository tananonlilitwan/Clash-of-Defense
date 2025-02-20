using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePoint : MonoBehaviour
{
    public GameObject[] towerPrefabs; // รายการของ Tower Prefab ที่รองรับ
    private GameObject currentTower; // ตัวแปรเก็บป้อมที่สร้างแล้ว ณ จุดนี้
    private int currentTowerIndex = 0; // ตัวแปรเก็บ index ของป้อมที่เลือก

    // ฟังก์ชันที่ถูกเรียกเมื่อคลิกที่ BasePoint
    /*void OnMouseDown()
    {
        if (currentTower == null)  // ถ้ายังไม่มีป้อมอยู่ที่จุดนี้
        {
            PlaceTower();
        }
        else  // ถ้ามีป้อมอยู่แล้ว
        {
            RemoveTower(); // ลบป้อมออก
        }
    }*/

    void PlaceTower()
    {
        if (towerPrefabs.Length > 0)
        {
            // เลือก towerPrefab ตาม index ที่ต้องการ
            currentTower = Instantiate(towerPrefabs[currentTowerIndex], transform.position, Quaternion.identity);
            Debug.Log("ป้อมถูกวางลงบนฐานแล้ว!");
        }
        else
        {
            Debug.LogWarning("ไม่มีป้อมให้วาง!");
        }
    }

    void RemoveTower()
    {
        if (currentTower != null)
        {
            Destroy(currentTower);
            currentTower = null;
            Debug.Log("ป้อมถูกลบออกจากฐานแล้ว!");
        }
    }

    // ฟังก์ชันสำหรับเปลี่ยน TowerPrefab ที่จะวาง
    public void SwitchTower(int index)
    {
        if (index >= 0 && index < towerPrefabs.Length)
        {
            currentTowerIndex = index;
            Debug.Log("เลือกป้อมใหม่ที่ index: " + index);
        }
        else
        {
            Debug.LogWarning("Index ไม่ถูกต้อง!");
        }
    }
}