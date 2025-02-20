using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private Enemy target;
    public float explosionRadius = 5f; // รัศมีระเบิด
    public int damage = 50; // ความเสียหาย

    public void Seek(Enemy _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // ทำลายสกิลถ้าไม่มีเป้าหมาย
            return;
        }

        // ระเบิดเป้าหมาย
        Explode();
    }

    private void Explode()
    {
        // ค้นหาศัตรูทั้งหมดในรัศมีระเบิด
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= explosionRadius)
            {
                enemy.TakeDamage(damage); // ให้ศัตรูรับความเสียหาย
            }
        }

        // ทำลายตัวสกิล
        Destroy(gameObject);
    }
}
