using System;
using UnityEngine;

[Serializable]
public class DamageData
{
    public float RawDamage { get; set; }
    public int DamageMultiplier { get; set; }
    public string DamageName { get; set; }
    public GameObject DamageSender { get; set; }

    public DamageData() { }
    public DamageData(float damage) {
        this.RawDamage = damage;
    }


}
