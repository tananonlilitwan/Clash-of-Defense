using UnityEngine;

[CreateAssetMenu(fileName = "TowerStats", menuName = "TowerStats")]
public class TowerStats : ScriptableObject
{
    public int damage = 10;
    public float range = 5f;
    public float fireRate = 1f;
    public int towerCost = 50;

    public void UpgradeDamage(float percentage)
    {
        damage += Mathf.CeilToInt(damage * percentage);
        Debug.Log($"Damage upgraded to: {damage}");
    }

    public void UpgradeRange(float percentage)
    {
        range += range * percentage;
        Debug.Log($"Range upgraded to: {range}");
    }

    public void UpgradeFireRate(float percentage)
    {
        fireRate -= fireRate * percentage;
        if (fireRate < 0.1f) fireRate = 0.1f;
        Debug.Log($"FireRate upgraded to: {fireRate}");
    }

    public void UpgradeCostReduction(float percentage)
    {
        towerCost -= Mathf.CeilToInt(towerCost * percentage);
        if (towerCost < 10) towerCost = 10;
        Debug.Log($"Tower Cost reduced to: {towerCost}");
    }
}