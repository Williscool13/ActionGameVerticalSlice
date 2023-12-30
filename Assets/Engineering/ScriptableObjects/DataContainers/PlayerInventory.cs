using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "PlayerInventory", menuName = "Inventory/PlayerInventory")]
public class PlayerInventory : ScriptableObject
{
    [SerializeField]private List<ItemInstance> items = new List<ItemInstance>();

    public List<ItemInstance> Items { get { return items; } }
    public void AddItem(ItemInstance item) { items.Add(item); }
    public void RemoveItem(ItemInstance item) { items.Remove(item); }
}

