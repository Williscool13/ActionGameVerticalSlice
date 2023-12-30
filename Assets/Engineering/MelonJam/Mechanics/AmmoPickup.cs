using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] int ammoPickupValue = 1;
    [SerializeField] Item weaponItem;
    [SerializeField] string weaponItemName = "Present Gun";
    [SerializeField] GameObject ammoRockPrefab;

    [SerializeField] AudioSource pickupSoundSource;
    [SerializeField] AudioClip pickupSoundClip;
    [SerializeField] AudioClip ammoRefillSound;

    [SerializeField] bool ammoRockOnly = false;
    [SerializeField] bool ammoRefillOnly = false;
    public bool AmmoRockOnly { get { return ammoRockOnly; } set { ammoRockOnly = value; } }
    public bool AmmoRefillOnly { get { return ammoRefillOnly; } set { ammoRefillOnly = value; } }

    public event EventHandler OnPickup;

    private void OnTriggerEnter(Collider other) {
        if (deactivated) return;
        if (!other.gameObject.CompareTag("Player")) { return; }

        PlayerInventoryManager pim = other.GetComponent<PlayerInventoryManager>();
        Debug.Assert(pim != null, "Object with tag Player has no PlayerInventoryManager attached");
        RefillAmmo(pim.WeaponItems.ToArray(), pim);
    }

    bool deactivated = false;
    void RefillAmmo(ItemInstance[] items, PlayerInventoryManager pim) {

        bool destroy = false;
        bool ammoRefilled = false;
        if (!ammoRockOnly) {
            foreach (ItemInstance ii in items) {
                if (ii is WeaponInstance wi) {
                    if (ii.Item != weaponItem) { continue; }
                    //if (wi.Weapon.WeaponName != weaponItemName) { continue; }
                    wi.Weapon.AddCurrentAmmo(ammoPickupValue);
                    destroy = true;
                    ammoRefilled = true;

                    pickupSoundSource.clip = ammoRefillSound;
                    pickupSoundSource.Play();
                }
            }
        }

        if (!ammoRefillOnly) {
            if (!ammoRefilled && ammoRockPrefab != null) {
                GameObject gob = Instantiate(ammoRockPrefab, transform.position, Quaternion.identity);
                ItemInstance ii = gob.GetComponentInChildren<ItemInstance>(true);
                Debug.Assert(ii != null, "Rock spawned has no ItemInstance component in its children");
                pim.AddItem(ii);

                destroy = true;

                pickupSoundSource.clip = pickupSoundClip;
                pickupSoundSource.Play();
            }
        }


        if (destroy) {
            foreach (Transform child in transform) {
                child.gameObject.SetActive(false);
            }
            Destroy(gameObject, 3.0f);
            deactivated = true;

            OnPickup?.Invoke(this, EventArgs.Empty);
        }

    }

    [SerializeField] float bobSpeed = 5f;
    [SerializeField] float bobDistance = 0.25f;
    [SerializeField] float rotateTime = 10f;

    private void Update() {
        transform.Rotate(new Vector3(0f, 1f, 0f), rotateTime * Time.deltaTime);
        transform.Translate(new Vector3(0f, Mathf.Sin(Time.time * bobSpeed) * bobDistance * Time.deltaTime, 0f));
    }
}
