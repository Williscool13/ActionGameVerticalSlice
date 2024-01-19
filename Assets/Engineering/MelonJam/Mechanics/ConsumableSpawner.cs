using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject consumablePrefab;    
    [SerializeField] protected Transform[] spawnPoints;
    [SerializeField] protected float pollRate = 0.1f;
    protected float pollTimer = 0f;

    public bool Active { get; protected set; } = true;


    protected virtual void Update() {
        if (!Active) return;

        pollTimer += Time.deltaTime;
        if (pollTimer < pollRate) return;
        pollTimer = 0f;

        int index = GetNextSpawnIndex();
        if (index == -1) return;
        Debug.Assert(index < spawnPoints.Length, "Spawn index out of range");

        SpawnConsumable(index);
    }
    protected virtual void SpawnConsumable(int index) {

        GameObject consumable = Instantiate(consumablePrefab, spawnPoints[index].position, Quaternion.identity);
        consumable.transform.parent = spawnPoints[index];
        AmmoPickup ap = consumable.GetComponent<AmmoPickup>();

        ap.OnPickup += OnAmmoPickedUp;
    }
    protected abstract int GetNextSpawnIndex();
    protected abstract void OnAmmoPickedUp(object sender, EventArgs e);
    
    void Start()
    {
        if (spawnPoints.Length < 0) {
            Debug.LogError("No spawn points set for ConsumableSpawner named: " + name);
            Destroy(this.gameObject);
            return;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        foreach (Transform t in spawnPoints) {
            Gizmos.DrawWireCube(t.position, Vector3.one * 0.5f);
        }
    }
}
