using PlayerFiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSwapper : MonoBehaviour
{
    private TerrainChecker terrainChecker;
    [SerializeField] private FootstepCollection[] footstepCollections;
    [SerializeField] private FootstepManager footstepManager;
    [SerializeField] private float terrainCheckRange = 0.5f;
    private string currentLayerName;

    void Start()
    {
        terrainChecker = new TerrainChecker();

    }

    public void CheckLayers() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -footstepManager.transform.up, out hit, terrainCheckRange)) {

            // Mesh Check
            if (hit.transform.GetComponent<SurfaceType>() != null) {
                FootstepCollection fc = hit.transform.GetComponent<SurfaceType>().FootstepCollection;
                currentLayerName = fc.name;
                footstepManager.SetFootstepCollection(fc);
                Debug.Log("Found Mesh");
                return;
            }

            // Terrain Check
            if (hit.transform.GetComponent<Terrain>() != null) {
                Terrain t = hit.transform.GetComponent<Terrain>();

                string layerName = terrainChecker.GetLayerName(transform.position, t);
                int layerId = terrainChecker.GetLayerId(transform.position, t);
                if (currentLayerName != layerName) {
                    currentLayerName = layerName;
                    foreach (FootstepCollection fc in footstepCollections) {
                        if (fc.name == currentLayerName) {
                            footstepManager.SetFootstepCollection(fc);
                        }
                    }
                }
                Debug.Log("Found Terrain");
            }

        }
    }
}
