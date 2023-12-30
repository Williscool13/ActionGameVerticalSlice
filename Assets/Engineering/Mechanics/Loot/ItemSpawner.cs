using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] Item item;

    ItemInstance instance;
    // Start is called before the first frame update
    void Start()
    {
        if (item == null) return;


        instance = item.CreateItemInstance(transform.position, transform.rotation);
        instance.OnInteract += OnInteract_SpawnNewItem;
    }

    void OnInteract_SpawnNewItem(object o, InteractEventArgs e) {
        transform.position += Vector3.forward * 2f;
        instance.OnInteract -= OnInteract_SpawnNewItem;
        instance = item.CreateItemInstance(transform.position, transform.rotation);
        instance.OnInteract += OnInteract_SpawnNewItem;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
