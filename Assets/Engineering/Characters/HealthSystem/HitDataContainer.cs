using System;
using UnityEngine;

[Serializable]
public class HitDataContainer
{
    public float RawDamage { get; set; }
    public int DamageMultiplier { get; set; }
    public string DamageName { get; set; }
    public GameObject DamageSender { get; set; }

    public HitDataContainer() { }
    public HitDataContainer(float damage) {
        this.RawDamage = damage;
    }

    public int GetTotalDamage() {
        return (int)this.RawDamage * this.DamageMultiplier;
    }

}
