using System;
using UnityEngine;

[Serializable]
public class DamageEventData
{
    public float RawDamage { get; set; }
    public int DamageMultiplier { get; set; }
    public string DamageName { get; set; }
    public GameObject DamageSender { get; set; }

    public DamageEventData() { }
    public DamageEventData(float damage) {
        this.RawDamage = damage;
    }


}
