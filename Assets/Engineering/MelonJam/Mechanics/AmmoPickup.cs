using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [Title("Pickup Properties")]
    [SerializeField] int ammoPickupValue = 1;
    [SerializeField] bool gunSpecific = true;
    [ShowIf("gunSpecific")][SerializeField][InfoBox("Target weapon this pickup is designed for")] Item weaponItem;

    [Title("Sound Properties")]
    [SerializeField] AudioSource ammoSoundSource;
    [SerializeField] AudioClip ammoSoundClip;

    [Title("Bobbing Properties")]
    [SerializeField] float bobSpeed = 5f;
    [SerializeField] float bobDistance = 0.25f;
    [SerializeField] float rotateTime = 10f;

    [Title("Misc Properties")]
    [SerializeField] float destroyPadTime = 0.1f;

    bool deactivated = false;

    public event EventHandler OnPickup;

    private void OnTriggerEnter(Collider other) {
        if (deactivated) return;
        if (!other.gameObject.CompareTag("Player")) { return; }

        IWeaponInventory gunInventory = other.GetComponent<IWeaponInventory>();
        if (gunInventory == null) { Debug.Log("Player has no IWeaponInventory component"); return; }

        WeaponInstance target;
        // specific gun in inventory
        if (gunSpecific) {
            Debug.Assert(weaponItem != null, "AmmoPickup: WeaponItem is null");
            Debug.Assert(weaponItem.ItemType == ItemTypes.Weapon, "AmmoPickup: WeaponItem is not a weapon");
            target = gunInventory.TryGetWeapon(weaponItem);
        } 
        // current gun player is holding
        else {
            target = gunInventory.GetCurrentWeapon();

            if (target.Weapon.UsesBullets == false) { target = null; }
        }

        if (target == null) { return; }
        else { RefillAmmo(target); }
    }

    void RefillAmmo(ItemInstance item) {
        if (item is WeaponInstance wi) {
            wi.Weapon.AddReserveAmmo(ammoPickupValue);
            deactivated = true;
            OnPickup?.Invoke(this, EventArgs.Empty);

            PickupEffect();
        } else {
            Debug.Log("ItemInstance is not a WeaponInstance");
        }
    }

    void PickupEffect() {
        foreach (Transform child in transform) { child.gameObject.SetActive(false); } 
        ammoSoundSource.clip = ammoSoundClip;
        ammoSoundSource.Play();
        Destroy(this.gameObject, ammoSoundClip.length + destroyPadTime);
    }

    private void Update() {
        transform.Rotate(new Vector3(0f, 1f, 0f), rotateTime * Time.deltaTime);
        transform.Translate(new Vector3(0f, Mathf.Sin(Time.time * bobSpeed) * bobDistance * Time.deltaTime, 0f));
    }
}
