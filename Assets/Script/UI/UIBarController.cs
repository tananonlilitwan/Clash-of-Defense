using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBarController : MonoBehaviour
{
    public RectTransform uiBar; // อ้างอิงถึง UI Bar ที่เราต้องการเลื่อน
    public float slideSpeed = 5f; // ความเร็วในการเลื่อน
    private bool isUIBarOpen = false; // สถานะของ UI Bar (เปิด/ปิด)

    [SerializeField] public float minPositionX; // ตำแหน่งขั้นต่ำที่ UI Bar จะเลื่อนไป
    [SerializeField] public float maxPositionX;    // ตำแหน่งสูงสุดที่ UI Bar จะเลื่อนไป

    private Vector2 closedPosition; // ตำแหน่งที่ UI Bar จะเลื่อนไปเมื่อปิด
    private Vector2 openPosition;   // ตำแหน่งที่ UI Bar จะเลื่อนไปเมื่อเปิด
    
    public GameObject statusCanvas; // Canvas ที่จะแสดงสถานะเมื่อเมาส์ไปบนจุดที่สามารถวางป้อมได้

    void Start()
    {
        // เก็บตำแหน่งเปิดและปิดจากระยะที่กำหนด
        openPosition = new Vector2(maxPositionX, uiBar.anchoredPosition.y); // เลื่อน UI Bar ไปทางขวา
        closedPosition = new Vector2(minPositionX, uiBar.anchoredPosition.y); // เลื่อน UI Bar ไปทางซ้าย

        // เริ่มต้นที่ตำแหน่งที่ปิด (ตำแหน่ง minPositionX)
        uiBar.anchoredPosition = closedPosition;
    }

    // ฟังก์ชันในการกดปุ่มเพื่อเปิด/ปิด UI Bar
    public void ToggleUIBar()
    {
        if (isUIBarOpen)
        {
            // เลื่อน UI Bar ไปตำแหน่งที่ปิด
            StartCoroutine(SlideUIBar(closedPosition));
            statusCanvas.SetActive(true);
        }
        else
        {
            // เลื่อน UI Bar กลับไปตำแหน่งที่เปิด
            StartCoroutine(SlideUIBar(openPosition));
            statusCanvas.SetActive(false);
        }

        // เปลี่ยนสถานะของ UI Bar
        isUIBarOpen = !isUIBarOpen;
    }

    // Coroutine เพื่อเลื่อน UI Bar
    private IEnumerator SlideUIBar(Vector2 targetPosition)
    {
        float timeElapsed = 0f;
        Vector2 startingPos = uiBar.anchoredPosition;

        // เลื่อน UI Bar ไปยังตำแหน่งที่กำหนด
        while (timeElapsed < 1f)
        {
            // ใช้ Mathf.Lerp เพื่อทำให้ UI Bar เลื่อนไปอย่างเรียบเนียน
            uiBar.anchoredPosition = Vector2.Lerp(startingPos, targetPosition, timeElapsed);
            timeElapsed += Time.deltaTime * slideSpeed;
            yield return null;
        }

        // เลื่อนถึงตำแหน่งสุดท้าย
        uiBar.anchoredPosition = targetPosition;
    }
}
