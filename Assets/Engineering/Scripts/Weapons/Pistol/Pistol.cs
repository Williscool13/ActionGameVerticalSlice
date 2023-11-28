using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponBase
{
    public override float GetReloadSpeedMultiplier() {
        return 1.0f;
    }
    public override bool CanFire(bool buttonHold) {
        return true;
    }

    public override bool CanReload() {
        return true;
    }

    public override RecoilData Fire() {
        return new RecoilData(Vector2.zero, 0.1f);
    }

    public override void ReloadEnd() {
    }

    public override void ReloadStart() {
    }

}
