using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacementManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public GameObject towerPrefab; // Prefab ของป้อม
    private GameObject currentTower; // เก็บป้อมที่กำลังถูกลาก
    private bool isTowerPlaced = false; // ตรวจสอบว่าป้อมถูกวางแล้วหรือยัง
    private bool isDragging = false; // ตรวจสอบว่ากำลังลากป้อมหรือไม่

    private Camera mainCamera;
    private MoneyManager moneyManager;
    
    public GameObject statusCanvas; // Canvas ที่จะแสดงสถานะเมื่อเมาส์ไปบนจุดที่สามารถวางป้อมได้

    void Start()
    {
        mainCamera = Camera.main;
        moneyManager = FindObjectOfType<MoneyManager>(); // ค้นหา MoneyManager
        
        // ซ่อน Canvas เริ่มต้นเมื่อเริ่มเกม
        if (statusCanvas != null)
        {
            statusCanvas.SetActive(false);
        }
    }

    public void PrepareTowerPlacement()
    {
        if (towerPrefab != null)
        {
            currentTower = Instantiate(towerPrefab); // สร้างป้อมที่ตำแหน่งเริ่มต้น
            isTowerPlaced = false; // ตั้งค่าเป็น false เมื่อสร้างป้อมใหม่
            isDragging = false; // กำหนดว่าไม่กำลังลาก
            Invoke("CancelTowerPlacement", 0.5f); // เรียกใช้ฟังก์ชัน CancelTowerPlacement หลัง 0.5 วินาที
        }
        else
        {
            Debug.LogWarning("ไม่สามารถสร้างป้อมได้ เพราะไม่มี Tower Prefab");
        }
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        
        // เมื่อเริ่มลาก เรากำหนดว่าเป็นการลากป้อม
        if (currentTower != null)
        {
            isDragging = true;
            CancelInvoke("CancelTowerPlacement"); // ยกเลิกการลบป้อม หลังจากเริ่มลาก
        }
        
        // แสดง statusCanvas เมื่อคลิกที่ obj และรอ 2 วินาทีเพื่อซ่อน
        if (statusCanvas != null)
        {
            statusCanvas.SetActive(true); // แสดง Canvas
            Invoke("HideStatusCanvas", 2f); // ซ่อน Canvas หลังจาก 2 วินาที
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentTower != null)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(eventData.position);
            mousePos.z = 0;
            currentTower.transform.position = mousePos;
        }
    }

    /*public void OnPointerUp(PointerEventData eventData)
    {
        if (currentTower != null)
        {
            Collider2D basePoint = Physics2D.OverlapPoint(currentTower.transform.position, LayerMask.GetMask("BasePoint"));
            if (basePoint != null)
            {
                currentTower.transform.position = basePoint.transform.position;
                isTowerPlaced = true; // ตั้งค่าว่าป้อมถูกวางแล้ว

                // หักเงินเมื่อวางป้อมสำเร็จ
                if (moneyManager.SpendMoney(50)) // ตรวจสอบว่ามีเงินเพียงพอ
                {
                    Debug.Log("ป้อมถูกวางในตำแหน่งฐานแล้ว!");
                }
                else
                {
                    Debug.LogWarning("ไม่สามารถซื้อป้อมได้ เงินไม่พอ");
                    Destroy(currentTower); // ลบป้อมหากเงินไม่พอ
                }
            }
            else
            {
                // ถ้าไม่ได้วางในจุดฐาน ให้ลบป้อมออก
                Destroy(currentTower);
                Debug.Log("ไม่สามารถวางป้อมที่ตำแหน่งนี้ได้!");
            }
            currentTower = null;
        }
    }*/
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentTower != null)
        {
            Collider2D basePoint = Physics2D.OverlapPoint(currentTower.transform.position, LayerMask.GetMask("BasePoint"));
            if (basePoint != null)
            {
                currentTower.transform.position = basePoint.transform.position;
                isTowerPlaced = true;

                // หักเงินเมื่อวางป้อมสำเร็จ
                if (moneyManager.SpendMoney(50))
                {
                    Debug.Log("ป้อมถูกวางในตำแหน่งฐานแล้ว!");
                    // เมื่อวางป้อมแล้ว ให้เปิดการยิงของ Tower
                    NormalTower normalTower = currentTower.GetComponent<NormalTower>();
                    if (normalTower != null)
                    {
                        normalTower.PlaceTower(); // เรียกฟังก์ชันที่ทำให้ Tower สามารถยิงได้
                    }
                    TowerAOE towerAoe = currentTower.GetComponent<TowerAOE>();
                    if (towerAoe != null)
                    {
                        towerAoe.PlaceTower(); // เรียกฟังก์ชันที่ทำให้ Tower สามารถยิงได้
                    }
                    SlowTower slowTower = currentTower.GetComponent<SlowTower>();
                    if (slowTower != null)
                    {
                        slowTower.PlaceTower(); // เรียกฟังก์ชันที่ทำให้ Tower สามารถยิงได้
                    }
                    LongRangeTower longRangeTower  = currentTower.GetComponent<LongRangeTower>();
                    if (longRangeTower != null)
                    {
                        longRangeTower.PlaceTower(); // เรียกฟังก์ชันที่ทำให้ Tower สามารถยิงได้
                    }
                    BarrackTower barrackTower = currentTower.GetComponent<BarrackTower>();
                    if (barrackTower != null)
                    {
                        barrackTower.PlaceTower(); // เรียกฟังก์ชันที่ทำให้ Tower สามารถยิงได้
                    }
                    
                }
                else
                {
                    Debug.LogWarning("ไม่สามารถซื้อป้อมได้ เงินไม่พอ");
                    Destroy(currentTower); // ลบป้อมหากเงินไม่พอ
                }
            }
            else
            {
                Destroy(currentTower);
                Debug.Log("ไม่สามารถวางป้อมที่ตำแหน่งนี้ได้!");
            }
            currentTower = null;
        }
    }

    private void CancelTowerPlacement()
    {
        // ถ้าผู้เล่นยังไม่ได้วางป้อมหลังจาก 0.5 วินาที และไม่ได้กำลังลาก ให้ลบป้อมออก
        if (currentTower != null && !isTowerPlaced && !isDragging)
        {
            Destroy(currentTower);
            Debug.Log("ไม่ได้ลากป้อมไปวางภายในเวลา 0.5 วินาที ป้อมถูกลบออก");
        }
    }
    // ฟังก์ชันที่ใช้ซ่อน Canvas
    private void HideStatusCanvas()
    {
        if (statusCanvas != null)
        {
            statusCanvas.SetActive(false);
        }
    }
}
