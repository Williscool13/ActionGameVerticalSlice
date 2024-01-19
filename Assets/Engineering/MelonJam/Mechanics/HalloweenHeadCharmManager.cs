using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalloweenHeadCharmManager : MonoBehaviour
{
    [SerializeField] private HalloweenHead[] halloweenHeads;

    [SerializeField] private AudioSource cageOpenSource;
    [SerializeField] private AudioClip cageOpenClip;

    [SerializeField]
    [SceneObjectsOnly]
    private GameObject targetCharm;

    [SerializeField]
    [SceneObjectsOnly]
    private Transform targetCharmPosition;


    [SerializeField]
    [SceneObjectsOnly]
    private GameObject[] targetBars;

    [SerializeField]
    [SceneObjectsOnly]
    private Transform[] targetBarPositions;

    void Start()
    {
        foreach (HalloweenHead head in halloweenHeads) {
            head.convertedEvent += OnHeadConverted;
        }
    }

    void OnHeadConverted(object o, System.EventArgs e) {
        CheckSolution(); 
    }

    void CheckSolution() {
        foreach (HalloweenHead head in halloweenHeads) {
            if (!head.IsSolved()) {
                return;
            }
        }

        Solution();
    }

    [Button("Solve")] 
    void Solution() {
        foreach (HalloweenHead head in halloweenHeads) {
            head.Locked = true; 
            head.ActivateSolvedParticles();
        }

        targetCharm.GetComponent<Collectible>().MoveCharm(targetCharmPosition.position, 8.0f);

        for (int i = 0; i < targetBars.Length; i++) {
            targetBars[i].transform.DOMove(targetBarPositions[i].position, 2.0f);
        }

        cageOpenSource.PlayOneShot(cageOpenClip);
    }
}
