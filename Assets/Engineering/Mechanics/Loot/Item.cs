using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [SerializeField] string itemName;
    [SerializeField] int itemId;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] ItemTypes itemType;
    [SerializeField] float itemWeight = 0f;
    public string ItemName { get { return itemName; } }
    public int ItemId { get { return itemId; } }
    public GameObject ItemPrefab { get { return itemPrefab; } }
    public ItemTypes ItemType { get { return itemType; } }
    public float ItemWeight { get { return itemWeight; } }

    public ItemInstance CreateItemInstance() {
        ItemInstance _i = GameObject.Instantiate(itemPrefab).GetComponent<ItemInstance>();
        _i.InitializeItem(this);
        return _i;
    }

    public ItemInstance CreateItemInstance(GameObject instance) {
        ItemInstance _i = instance.GetComponent<ItemInstance>();
        _i.InitializeItem(this);
        return _i;
    }


    public ItemInstance CreateItemInstance(Vector3 position, Quaternion rotation) {
        ItemInstance _i = GameObject.Instantiate(itemPrefab, position, rotation).GetComponent<ItemInstance>();
        _i.InitializeItem(this);
        return _i;
    }

}

public enum ItemTypes
{
    Weapon,
    Carryable,
    Internal
}