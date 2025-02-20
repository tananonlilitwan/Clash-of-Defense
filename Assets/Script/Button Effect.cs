using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // สำหรับการใช้ IPointerEnterHandler และ IPointerExitHandler
public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler // ต้อง implement อินเทอร์เฟซสองตัวนี้
{
    [SerializeField] private Sprite normalSprite;  // สปริงปกติเมื่อเมาส์ไม่ได้อยู่บนปุ่ม
    [SerializeField] private Sprite hoverSprite;   // สปริงเมื่อเมาส์อยู่บนปุ่ม

    private Image buttonImage; // อ้างอิงถึง Image component ของปุ่ม

    // Start is called before the first frame update
    void Start()
    {
        buttonImage = GetComponent<Image>(); // เข้าถึง Image component ของปุ่ม
        buttonImage.sprite = normalSprite;   // ตั้งค่า Sprite เริ่มต้นเป็นปกติ
    }

    // ฟังก์ชันเมื่อเมาส์เข้าไปบนปุ่ม
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = hoverSprite; // เปลี่ยน Sprite เป็น hoverSprite เมื่อเมาส์อยู่บนปุ่ม
    }

    // ฟังก์ชันเมื่อเมาส์ออกจากปุ่ม
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = normalSprite; // เปลี่ยน Sprite กลับเป็นปกติเมื่อเมาส์ออกจากปุ่ม
    }
}
