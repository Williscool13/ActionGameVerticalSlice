using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmSpawnerManager : MonoBehaviour
{
    [SerializeField][SceneObjectsOnly] GameObject charmObject;
    [SerializeField][SceneObjectsOnly] Transform charmtargetPosition;
    [SerializeField][AssetsOnly] GameObject festiveCharacterPrefab;

    [ChildGameObjectsOnly]
    [SerializeField] Transform[] spawnPoints;

    int totalFestives;
    int convertedFestives = 0;
    // Start is called before the first frame update
    void Start()
    {
        totalFestives = spawnPoints.Length;
        foreach (Transform spawnPoint in spawnPoints) {
            //GameObject charm = Instantiate(charmPrefab, spawnPoint.position, Quaternion.identity);
            //charm.transform.parent = spawnPoint;

            
            GameObject festiveCharacter = Instantiate(festiveCharacterPrefab, spawnPoint.position, spawnPoint.rotation);
            festiveCharacter.transform.parent = spawnPoint;
            festiveCharacter.GetComponent<FestiveTarget>().convertedEvent += OnFestiveConverted;
        }
    }

    void OnFestiveConverted(object sender, System.EventArgs e) {
        convertedFestives++;
        Debug.Log("Festive Converted");

        if (convertedFestives == totalFestives) {
            Debug.Log("All festives converted");
            MoveCharmToTargetPosition();
        }
    }

    void MoveCharmToTargetPosition() {
        charmObject.GetComponent<Charm>().MoveCharm(charmtargetPosition.position, 4.0f);
    }

    private void OnDrawGizmos() {
        foreach (Transform spawnPoint in spawnPoints) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spawnPoint.position, 1f);
        }
    }
}
