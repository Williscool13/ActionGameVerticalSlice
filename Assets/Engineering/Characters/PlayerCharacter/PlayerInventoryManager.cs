using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour, IPlayerInventory
{
    [SerializeField] private PlayerInventory weaponInventory;
    [SerializeField] private PlayerInventory carryInventory;
    [SerializeField] private PlayerInventory internalInventory;

    [SerializeField] private PlayerLoadoutManager playerLoadoutManager;
    [SerializeField] private PlayerMoveController moveController;
    public List<ItemInstance> WeaponItems => weaponInventory.Items;
    public List<ItemInstance> Items => internalInventory.Items;

    ICharacterMotor charMotor;
    private void Start() {
        weaponInventory.Items.Clear();
        carryInventory.Items.Clear();
        internalInventory.Items.Clear();

        charMotor = moveController.GetComponent<ICharacterMotor>();
    }

    public void AddItem(ItemInstance item) {
        Debug.Assert(item.Type == InteractableType.Item,"Item must be of type Item if added to inventory");
        switch (item.ItemType) {
            case ItemTypes.Weapon:
                weaponInventory.AddItem(item);
                WeaponBase droppedItem = playerLoadoutManager.EquipWeapon((item as WeaponInstance).Weapon);
                if (droppedItem != null) {
                    WeaponInstance droppedItemInst = droppedItem.GetComponentInChildren<WeaponInstance>(true);
                    if (droppedItemInst != null) {
                        weaponInventory.RemoveItem(droppedItemInst);
                        droppedItemInst.DropLaunch();
                    }
                }
                
                break;
            case ItemTypes.Carryable:
                carryInventory.AddItem(item);
                break;
            case ItemTypes.Internal:
                internalInventory.AddItem(item);
                break;
        }

        float totalEncumbrance = 0f;
        foreach (ItemInstance ii in WeaponItems) {
            totalEncumbrance += ii.Item.ItemWeight;
        }
        foreach (ItemInstance ii in Items) {
            totalEncumbrance += ii.Item.ItemWeight;
        }
        foreach (ItemInstance ii in carryInventory.Items) {
            totalEncumbrance += ii.Item.ItemWeight;
        }
        charMotor.SetEncumbrance(totalEncumbrance);
    }

    public void DropCurrentWeapon() {
        WeaponInstance currWep = GetCurrentWeapon();
        if (currWep == null) {
            Debug.LogError("Curr wep is null");
            return; 
        }

        RemoveItem(currWep);

    }


    public void RemoveItem(ItemInstance item) {
        switch (item.ItemType) {
            case ItemTypes.Weapon:
                playerLoadoutManager.DropWeapon((item as WeaponInstance).Weapon);

                weaponInventory.RemoveItem(item);
                (item as WeaponInstance).DropLaunch();

                break;
            case ItemTypes.Carryable:
                carryInventory.RemoveItem(item);
                break;
            case ItemTypes.Internal:
                internalInventory.RemoveItem(item);
                break;
        }

        float totalEncumbrance = 0f;
        foreach (ItemInstance ii in WeaponItems) {
            totalEncumbrance += ii.Item.ItemWeight;
        }
        foreach (ItemInstance ii in Items) {
            totalEncumbrance += ii.Item.ItemWeight;
        }
        foreach (ItemInstance ii in carryInventory.Items) {
            totalEncumbrance += ii.Item.ItemWeight;
        }
        charMotor.SetEncumbrance(totalEncumbrance);
    }

    public WeaponInstance GetCurrentWeapon() {
        if (playerLoadoutManager.IsUnarmed()) { return null; }
        Debug.Log("Current weapon name is " + playerLoadoutManager.GetCurrentWeapon().name);
        return playerLoadoutManager.GetCurrentWeapon().GetComponentInChildren<WeaponInstance>(true);
    
    }
}


public interface IPlayerInventory
{
    public List<ItemInstance> WeaponItems { get; }
    public List<ItemInstance> Items { get; }
    public void AddItem(ItemInstance item);
    public void RemoveItem(ItemInstance item);
}