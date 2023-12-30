using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadoutManager : MonoBehaviour
{
    [SerializeField] Transform activeWeaponPivot;
    [SerializeField] Transform reserveWeaponPivot;
    [SerializeField] GunRigController gunRigController;

    [SerializeField] bool secondaryEnabled = true;
    Transform primaryWeapon;
    Transform secondaryWeapon;

    bool stowed = false;

    Dictionary<int, WeaponBase> cachedWeapons = new();
    Dictionary<int, WeaponIkTarget> cachedWeaponIkTargets = new();
    public WeaponBase EquipWeapon(WeaponBase targetWeapon) {
        
        if (primaryWeapon == null) {
            //primaryWeapon = targetWeapon.transform;
            SetPrimaryWeapon(targetWeapon.transform);
            targetWeapon.OnWeaponPickUp();
            return null;
        }
        else if (secondaryWeapon == null && secondaryEnabled) {
            SetSecondaryWeapon(primaryWeapon);
            SetPrimaryWeapon(targetWeapon.transform);
            targetWeapon.OnWeaponPickUp();
            return null;
        }
        else {
            WeaponBase dropped = GetCurrentWeapon();
            DropAndEquipWeapon(GetCurrentWeapon(), targetWeapon);
            return dropped;
        }
    }

    public void DropWeapon(WeaponBase weapon) {
        if (primaryWeapon == weapon.transform) {
            DropWeaponTransforms(primaryWeapon);
            primaryWeapon = null; 
        }
        else if (secondaryWeapon == weapon.transform) {
            DropWeaponTransforms(secondaryWeapon);
            secondaryWeapon = null; 
        }
        else { Debug.LogError("PlayerLoadoutManager: Attempted to drop weapon that is not equipped"); }

        weapon.OnWeaponDrop();

        if (primaryWeapon == null) {
            if (!IsUnarmed()) {
                SetPrimaryWeapon(secondaryWeapon);
                secondaryWeapon = null;
            }
        }
    }

    public void DropAndEquipWeapon(WeaponBase droppedWeapon, WeaponBase pickedUpWeapon) {
        if (droppedWeapon.transform == primaryWeapon) { 
            DropWeaponTransforms(primaryWeapon); 
            primaryWeapon = null; 
        } 
        else if (droppedWeapon.transform == secondaryWeapon) { 
            DropWeaponTransforms(secondaryWeapon);  
            secondaryWeapon = null; 
        }
        else { Debug.LogError("PlayerLoadoutManager: Attempted to drop weapon that is not equipped"); }

        droppedWeapon.OnWeaponDrop();


        if (primaryWeapon != null) {
            SetSecondaryWeapon(primaryWeapon);
            primaryWeapon = null;
        }
        SetPrimaryWeapon(pickedUpWeapon.transform);
        pickedUpWeapon.OnWeaponPickUp();
    }


    void DropWeaponTransforms(Transform weapon) {
        GetCachedWeaponBaseReference(weapon.gameObject).OnWeaponUnequip();
        weapon.SetParent(null);
    }


    public void StowWeapons() {
        stowed = true;
        GetCurrentWeapon().OnWeaponUnequip();
        GetCurrentWeapon().gameObject.SetActive(false);
    }

    public void DrawWeapons() {
        stowed = false;
        GetCurrentWeapon().gameObject.SetActive(true);
        GetCurrentWeapon().OnWeaponEquip();
    }

    public bool IsWeaponStowed() {
        return stowed;
    }

    public bool IsUnarmed() {
        return primaryWeapon == null && secondaryWeapon == null;
    }

    public bool CanSwap() {
        return secondaryWeapon != null;
    }
    public void SwapWeapon() {
        Debug.Assert(primaryWeapon != null, "PlayerLoadoutManager: Attempted to swap weapons without any weapons equipped");
        Debug.Assert(secondaryWeapon != null, "PlayerLoadoutManager: Attempted to swap weapons when only one weapon is equipped");
        Debug.Log("Swapping Weapons");

        Transform newSecondary = primaryWeapon;
        Transform newPrimary = secondaryWeapon;
        SetSecondaryWeapon(newSecondary);
        SetPrimaryWeapon(newPrimary);
    }

    void SetPrimaryWeapon(Transform weapon) {
        SetWeaponParent(weapon, activeWeaponPivot);
        GetCachedWeaponBaseReference(weapon.gameObject).OnWeaponEquip();
        gunRigController.ChangeHandIKTargets(GetCachedWeaponIkTargets(weapon).Front, GetCachedWeaponIkTargets(weapon).Handle);
        primaryWeapon = weapon;
    }
    void SetSecondaryWeapon(Transform weapon) {
        SetWeaponParent(weapon, reserveWeaponPivot);
        GetCachedWeaponBaseReference(weapon.gameObject).OnWeaponUnequip();
        secondaryWeapon = weapon;
    }

    void SetWeaponParent(Transform weapon, Transform pivot) {
        weapon.SetParent(pivot);

        weapon.transform.DOLocalMove(Vector3.zero, 0.05f);
        weapon.transform.DOLocalRotate(Vector3.zero, 0.05f);
    }

    public WeaponBase GetCurrentWeapon() {
        if (primaryWeapon == null) return null;
        return GetCachedWeaponBaseReference(primaryWeapon.gameObject);
    }
    public WeaponBase GetOtherWeapon() {
        if (secondaryWeapon == null) return null;
        return GetCachedWeaponBaseReference(secondaryWeapon.gameObject);
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
        None,
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



