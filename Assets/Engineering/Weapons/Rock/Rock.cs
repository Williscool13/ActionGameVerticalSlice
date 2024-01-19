using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Rock : WeaponBase
{
    public override string WeaponName => stats.rockName;

    public override bool UsesBullets => false; 
    public override bool ImpactDamage => true;

    [SerializeField] private RockStats stats;



    public override float GetReloadSpeedMultiplier() {
        return 1;
    }


    public override bool CanFire(bool buttonHold) {
        return false;
    }
    public override RecoilData Fire() {
        return new RecoilData(Vector2.zero, 0.01f);
    }


    public override bool CanReload() {
        return false;
    }

    public override void ReloadEnd() { }

    public override void ReloadStart() {
    }

    public override void AddCurrentAmmo(int count, bool exceedMax = false) { }

    public override void AddReserveAmmo(int count, bool exceedMax = false) { }

}
