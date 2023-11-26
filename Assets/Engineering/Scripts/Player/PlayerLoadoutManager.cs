using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadoutManager : MonoBehaviour
{
    WeaponSlot activeSlot = WeaponSlot.Primary;
    [SerializeField] Transform activeWeaponPivot;
    [SerializeField] Transform reserveWeaponPivot;
    [SerializeField] Transform primaryWeapon;
    [SerializeField] Transform secondaryWeapon;
    [SerializeField] GunRigController gunRigController;

    Dictionary<int, WeaponBase> cachedWeapons = new();
    Dictionary<int, WeaponIkTarget> cachedWeaponIkTargets = new();
    private void Start() {
        SetWeaponParent(primaryWeapon, activeWeaponPivot);
        SetWeaponParent(secondaryWeapon, reserveWeaponPivot);
        gunRigController.ChangeHandIKTargets(GetCachedWeaponIkTargets(primaryWeapon).Front, GetCachedWeaponIkTargets(primaryWeapon).Handle);
    }

    public void SwapWeapon() {
        Debug.Log("Swapping Weapons");
        WeaponSlot targetSlot = activeSlot == WeaponSlot.Primary ? WeaponSlot.Secondary : WeaponSlot.Primary;

        switch (targetSlot) {
            case WeaponSlot.Primary:
                SwapWeapon(secondaryWeapon, primaryWeapon);
                break;
            case WeaponSlot.Secondary:
                SwapWeapon(primaryWeapon, secondaryWeapon);
                break;
        }

        activeSlot = targetSlot;
    }


    void SwapWeapon(Transform current, Transform target) {
        SetWeaponParent(current, reserveWeaponPivot);
        SetWeaponParent(target, activeWeaponPivot);
        gunRigController.ChangeHandIKTargets(GetCachedWeaponIkTargets(target).Front, GetCachedWeaponIkTargets(target).Handle);
    }
    void SetWeaponParent(Transform weapon, Transform pivot) {
        Vector3 localPos = weapon.localPosition;
        Quaternion localRot = weapon.localRotation;
        weapon.SetParent(pivot);
        weapon.SetLocalPositionAndRotation(localPos, localRot);
    }

    public WeaponBase GetCurrentWeapon() {
        switch (activeSlot) {
            case WeaponSlot.Primary:
                return GetCachedWeaponBaseReference(primaryWeapon.gameObject);
            case WeaponSlot.Secondary:
                return GetCachedWeaponBaseReference(secondaryWeapon.gameObject);
        }
        return null;
    }

    WeaponIkTarget GetCachedWeaponIkTargets(Transform weaponObject) {
        int instanceId = weaponObject.GetInstanceID();
        if (!cachedWeaponIkTargets.ContainsKey(instanceId)) {
            cachedWeaponIkTargets[instanceId] = new WeaponIkTarget(weaponObject.GetChild(0), weaponObject.GetChild(1));
        }
        return cachedWeaponIkTargets[instanceId];
    }
    WeaponBase GetCachedWeaponBaseReference(GameObject weaponObject) {
        int instanceId = weaponObject.GetInstanceID();
        if (!cachedWeapons.ContainsKey(instanceId)) {
            cachedWeapons[instanceId] = weaponObject.GetComponent<WeaponBase>();
        }
        return cachedWeapons[instanceId];
    }
    enum WeaponSlot
    {
        Primary,
        Secondary,
    }
    struct WeaponIkTarget {
        public Transform Front { get; private set; }
        public Transform Handle { get; private set; }

        public WeaponIkTarget(Transform front, Transform handle) {
            Front = front;
            Handle = handle;
        }
    }
}



