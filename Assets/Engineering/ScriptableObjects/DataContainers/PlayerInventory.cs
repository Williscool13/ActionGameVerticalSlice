using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerInventory : ScriptableObject
{
    [SerializeField][ReadOnly]private List<Item> items = new List<Item>();

    public List<Item> Items { get { return items; } }
    public void AddItem(Item item) { items.Add(item); }
    public void RemoveItem(Item item) { items.Remove(item); }
}


public class Item
{
    public string name;
    public int id;
    public Item(string name, int id) {
        this.name = name;
        this.id = id;
    }



    public static Item CreateItem(int id) {
        switch (id) {
            case 0:
                return new Item("MoneyStack", 0);
            case 1:
                return new Item("Gold", 1);
            case 2:
                return new Item("Silver", 2);
            default:
                return null;
        }
    }
}