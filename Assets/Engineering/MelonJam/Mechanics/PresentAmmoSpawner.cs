using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentAmmoSpawner : MonoBehaviour
{
    [SerializeField] GameObject presentAmmoPrefab;
    [SerializeField] Transform[] spawnPoints;

    [SerializeField] bool ammoRockOnly = false;
    [SerializeField] bool ammoRefillOnly = false;

    [SerializeField] bool spawnOnPickup = false;

    [SerializeField] bool randomSpawn = false;
    [HideIf("spawnOnPickup")][SerializeField] float spawnInterval = 5f;

    int spawnIndex = -1;
    void Start()
    {
        if (spawnPoints.Length < 0) {
            Debug.LogError("No spawn points set for PresentAmmoSpawner");
            Destroy(this.gameObject);
            return;
        }

        if (randomSpawn && ((spawnOnPickup && spawnPoints.Length <= 1)|| spawnInterval == 0)) {
            Debug.LogError("PresentAmmoSpawner cannot be set to spawn randomly and on pickup");
            Destroy(this.gameObject);
            return;
        }

        Debug.Assert(!(ammoRockOnly && ammoRefillOnly), "PresentAmmoSpawner: Cannot be set to only spawn ammo rocks and refill ammo");

        // initial spawn
        spawnIndex = 0;
        SpawnAmmo(spawnIndex);
        spawnable = false;
    }

    float timeSinceLastPickup = 0f;
    void Update()
    {
        if (spawnOnPickup) return; 
        if (!spawnable) return;
        // if we're not spawning on pickup, spawn at a regular interval
        timeSinceLastPickup += Time.deltaTime;
        if (timeSinceLastPickup < spawnInterval) {
            return;
        }

        spawnable = false;

        SetSpawnIndex();
        SpawnAmmo(spawnIndex);
    }

    void SpawnAmmo(int index) {
        GameObject presentAmmo = Instantiate(presentAmmoPrefab, spawnPoints[index].position, Quaternion.identity);
        presentAmmo.transform.parent = spawnPoints[index];
        AmmoPickup ap = presentAmmo.GetComponent<AmmoPickup>();
        if (spawnOnPickup) {
            ap.OnPickup += SpawnNext;
        } else {
            ap.OnPickup += AmmoPickedUp;
        }
        ap.AmmoRockOnly = ammoRockOnly;
        ap.AmmoRefillOnly = ammoRefillOnly;
    }

    void SpawnNext(object sender, System.EventArgs e) {
        Debug.Log("Told to spawn next");
        SetSpawnIndex();
        SpawnAmmo(spawnIndex);
    }
    void SetSpawnIndex() {
        if (randomSpawn) {
            if (spawnPoints.Length == 1) {
                spawnIndex = 0;
                return;
            }
            int prevSpawnIndex = spawnIndex;
            while (spawnIndex == prevSpawnIndex) {
                spawnIndex = Random.Range(0, spawnPoints.Length);
            }

        }
        else {
            spawnIndex++;
            if (spawnIndex >= spawnPoints.Length) {
                spawnIndex = 0;
            }
        }
    }

    bool spawnable;
    void AmmoPickedUp(object sender, System.EventArgs e) {
        spawnable = true;
        timeSinceLastPickup = 0f;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        foreach (Transform t in spawnPoints) {
            Gizmos.DrawWireSphere(t.position, 0.5f);
        }
    }

}
