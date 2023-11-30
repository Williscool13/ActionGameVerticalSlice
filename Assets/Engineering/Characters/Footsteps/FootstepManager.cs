using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    List<AudioClip> currentWalkFootstepCollection = new();
    List<AudioClip> currentRunFootstepCollection = new();
    List<AudioClip> currentJumpFootstepCollection = new();
    List<AudioClip> currentLandFootstepCollection = new();

    [SerializeField] FootstepSwapper footstepSwapper;

    public void SetFootstepCollection(FootstepCollection footstepCollection) {
        currentWalkFootstepCollection.Clear();
        currentRunFootstepCollection.Clear();
        currentJumpFootstepCollection.Clear();
        currentLandFootstepCollection.Clear();

        foreach (AudioClip ac in footstepCollection.footstepWalkSounds) {
            currentWalkFootstepCollection.Add(ac);
        }
        foreach (AudioClip ac in footstepCollection.footstepRunSounds) {
            currentRunFootstepCollection.Add(ac);
        }
        foreach (AudioClip ac in footstepCollection.footstepJumpSounds) {
            currentJumpFootstepCollection.Add(ac);
        }
        foreach (AudioClip ac in footstepCollection.footstepLandSounds) {
            currentLandFootstepCollection.Add(ac);
        }
    }

    public AudioClip GetWalkSound() {
        footstepSwapper.CheckLayers();
        Debug.Assert(currentWalkFootstepCollection.Count > 0, "No walk sounds in current footstep collection");
        return currentWalkFootstepCollection[Random.Range(0, currentWalkFootstepCollection.Count)];
    }

    public AudioClip GetRunSound() {
        footstepSwapper.CheckLayers();
        Debug.Assert(currentRunFootstepCollection.Count > 0, "No run sounds in current footstep collection");
        return currentRunFootstepCollection[Random.Range(0, currentRunFootstepCollection.Count)];
    }

    public AudioClip GetJumpSound() {
        footstepSwapper.CheckLayers();
        Debug.Assert(currentJumpFootstepCollection.Count > 0, "No jump sounds in current footstep collection");
        return currentJumpFootstepCollection[Random.Range(0, currentJumpFootstepCollection.Count)];
    }

    public AudioClip GetLandSound() {
        footstepSwapper.CheckLayers();
        Debug.Assert(currentLandFootstepCollection.Count > 0, "No land sounds in current footstep collection");
        return currentLandFootstepCollection[Random.Range(0, currentLandFootstepCollection.Count)];
    }
}
