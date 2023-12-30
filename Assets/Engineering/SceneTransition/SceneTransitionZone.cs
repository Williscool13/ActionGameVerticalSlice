using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionZone : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            SceneTransitionManager.Instance.Transition(sceneName);
        }
    }
}
