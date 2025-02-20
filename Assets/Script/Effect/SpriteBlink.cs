using UnityEngine;
using System.Collections;

public class SpriteBlink : MonoBehaviour
{
    [SerializeField] public float blinkSpeed = 2f; // ความเร็วในการเปลี่ยน Alpha

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(BlinkSmooth());
    }

    private IEnumerator BlinkSmooth()
    {
        while (true)
        {
            // กระพริบแบบ Smooth ด้วยการปรับค่า Alpha ตาม Sine Wave
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;

            yield return null; // อัพเดตทุกเฟรมเพื่อความสมูท
        }
    }
}