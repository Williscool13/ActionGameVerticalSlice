using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public bool Aiming { get; protected set; }
    public void SetAiming(bool aiming) { Aiming = aiming; }

    public abstract float GetReloadSpeedMultiplier();

    public abstract bool CanFire(bool buttonHold);
    public abstract RecoilData Fire();

    public abstract bool CanReload();
    public abstract void ReloadStart();
    public abstract void ReloadEnd();


    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 20.0f);
    }
}

