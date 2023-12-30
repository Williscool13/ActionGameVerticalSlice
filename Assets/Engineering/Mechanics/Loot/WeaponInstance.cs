using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class WeaponInstance : ItemInstance
{
    [SerializeField] WeaponBase weapon;
    [SerializeField] GameObject weaponItem;
    [SerializeField] GameObject weaponGun;

    public WeaponBase Weapon => weapon;
    protected Rigidbody weaponItemRB;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    private void Awake() {
        weapon.OnWeaponPickedUp += OnWeaponPickedUp;
        weapon.OnWeaponDropped += OnWeaponDropped;
        weaponItemRB = weaponItem.GetComponent<Rigidbody>();
    }

    private void OnWeaponDropped(object sender, System.EventArgs e) {
        weaponItem.SetActive(true);
        weaponGun.SetActive(false);
        weaponItem.transform.SetPositionAndRotation(weapon.transform.position + weaponGun.transform.localPosition + weapon.transform.forward * weapon.transform.localScale.x, weapon.transform.rotation * weaponGun.transform.localRotation);
        //weaponItem.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    private void OnWeaponPickedUp(object sender, System.EventArgs e) {
        weaponItem.SetActive(false);
        weaponGun.SetActive(true);

        weapon.transform.position = weaponItem.transform.position;
        weapon.transform.rotation = weaponItem.transform.rotation;
        weaponItemRB.velocity = Vector3.zero;

    }


    public void DropLaunch() {
        weaponItemRB.AddForce(weapon.transform.forward * 10.0f, ForceMode.Impulse);
    }

    public override void Interact() {
        base.Interact();
    }
    public override void Highlight() {
        base.Highlight();
    }
}   
