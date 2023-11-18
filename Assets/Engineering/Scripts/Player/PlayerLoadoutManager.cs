using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadoutManager : MonoBehaviour
{
    WeaponSlot currentWeapon = WeaponSlot.Primary;
    [SerializeField] Transform activeWeaponPivot;
    [SerializeField] Transform reserveWeaponPivot;
    [SerializeField] Transform primaryWeapon;
    [SerializeField] Transform secondaryWeapon;
    [SerializeField] GunRigController gunRigController;

    public WeaponSlot CurrentWeapon => currentWeapon;

    public void SwapWeapon(WeaponSlot target) {
        Debug.Log("Swapping weapon");
        if (target == currentWeapon) { Debug.Log("Attempting to swap to currently equipped weapon"); return; }

        switch (target) {
            case WeaponSlot.Primary:
                SwapWeapon(secondaryWeapon, primaryWeapon);
                break;
            case WeaponSlot.Secondary:
                SwapWeapon(primaryWeapon, secondaryWeapon);
                break;
        }

        currentWeapon = target;
        
    }

    void SwapWeapon(Transform current, Transform target) {
        Vector3 localCurrentPos = current.localPosition;
        Quaternion localCurrentRot = current.localRotation;
        current.SetParent(reserveWeaponPivot);
        current.SetLocalPositionAndRotation(localCurrentPos, localCurrentRot);

        Vector3 localTargetPos = target.localPosition;
        Quaternion localTargetRot = target.localRotation;
        target.SetParent(activeWeaponPivot);
        target.SetLocalPositionAndRotation(localTargetPos, localTargetRot);
        gunRigController.ChangeHandIKTargets(target.GetChild(0), target.GetChild(1));
    }
}



public enum WeaponSlot
{
    Primary,
    Secondary,
}