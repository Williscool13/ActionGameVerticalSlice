using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public abstract bool UsesBullets { get; }
    public abstract bool ImpactDamage { get; }


    public abstract string WeaponName { get; }
    public bool Aiming { get; protected set; }
    public void SetAiming(bool aiming) { Aiming = aiming; }

    public abstract float GetReloadSpeedMultiplier();

    public abstract bool CanFire(bool buttonHold);
    public abstract RecoilData Fire();

    public abstract bool CanReload();
    public abstract void ReloadStart();
    public abstract void ReloadEnd();

    public event EventHandler OnWeaponEquipped;
    public event EventHandler OnWeaponUnequipped;
    public event EventHandler OnWeaponFired;
    public event EventHandler OnWeaponFinishReloaded;
    public event EventHandler OnWeaponDropped;
    public event EventHandler OnWeaponPickedUp;

    public void OnWeaponEquip() {
        OnWeaponEquipped?.Invoke(this, null);
    }
    public void OnWeaponUnequip() {
        OnWeaponUnequipped?.Invoke(this, null);
    }
    public void OnWeaponDrop() {
        OnWeaponDropped?.Invoke(this, null);
    }
    public void OnWeaponPickUp() {
        OnWeaponPickedUp?.Invoke(this, null);
    }

    public abstract void AddCurrentAmmo(int count, bool exceedMax = false);
    public abstract void AddReserveAmmo(int count, bool exceedMax = false);

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 20.0f);
    }
}

