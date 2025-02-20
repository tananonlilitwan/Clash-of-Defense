using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBarController : MonoBehaviour
{
    public Camera mainCamera; // กล้องหลักที่เราต้องการเลื่อน
    public RectTransform dragBar; // บาร์ที่ใช้สำหรับลาก
    public float slideSpeed = 5f; // ความเร็วในการเลื่อนกล้อง

    [SerializeField] private float minPositionX = -10f; // ตำแหน่งซ้ายสุดของกล้อง
    [SerializeField] private float maxPositionX = 10f; // ตำแหน่งขวาสุดของกล้อง

    private bool isDragging = false; // การลากเปิด/ปิด
    private Vector2 previousMousePosition; // ตำแหน่งเมาส์ก่อนหน้าสำหรับการลาก

    void Update()
    {
        // เริ่มลากเมื่อคลิกบนบาร์
        if (Input.GetMouseButtonDown(0) && RectTransformUtility.RectangleContainsScreenPoint(dragBar, Input.mousePosition))
        {
            isDragging = true;
            previousMousePosition = Input.mousePosition;
        }

        // หยุดการลากเมื่อปล่อยปุ่ม
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // ถ้ากำลังลาก
        if (isDragging)
        {
            // คำนวณการเลื่อนของบาร์
            Vector2 delta = (Vector2)Input.mousePosition - previousMousePosition;

            // คำนวณตำแหน่งใหม่ของบาร์
            float newPositionX = Mathf.Clamp(dragBar.anchoredPosition.x + delta.x, minPositionX, maxPositionX);

            // ตั้งค่าตำแหน่งใหม่ของบาร์
            dragBar.anchoredPosition = new Vector2(newPositionX, dragBar.anchoredPosition.y);

            // เลื่อนกล้องไปตามตำแหน่งบาร์
            float cameraNewX = Mathf.Lerp(minPositionX, maxPositionX, Mathf.InverseLerp(minPositionX, maxPositionX, newPositionX));
            mainCamera.transform.position = new Vector3(cameraNewX, mainCamera.transform.position.y, mainCamera.transform.position.z);

            // อัปเดตตำแหน่งเมาส์
            previousMousePosition = Input.mousePosition;
        }
    }
}