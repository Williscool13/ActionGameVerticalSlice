using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharmCollectionManager : MonoBehaviour
{
    [SerializeField] CharmCollection charmCollection;



    [SerializeField] private Transform leftGate;
    [SerializeField] private Transform rightGate;

    [SerializeField] private Canvas charmCanvas;
    [SerializeField] private Image[] charmImages;
    [SerializeField] private Image charmPanel;

    
    // charmImageIndex[0] will be the index of the image for charm 0
    Dictionary<int, int> charmImageIndex =
        new Dictionary<int, int> {
            {0, 0 },
            {1, 1 },
            {2, 4 },
            {3, 5 },
            {4, 3 },
            {5, 6 },
            {6, 8 },
            {7, 7 },
            {8, 2 }
        };


    private void Awake() {
        charmCollection.Reset();
    }

    private void Start() {
        charmCanvas.worldCamera = Camera.main;
        foreach (Image img in charmImages) {
            img.color = Color.black;
        }
    }

    public void CollectCharm(int charmIndex) {
        charmCollection.charmCollection[charmIndex] = true;

        CheckOpenGates();

        EnableCharmImage(charmIndex);

    }



    void CheckOpenGates() {
        if (!HasCollectedAllCharms()) {
            return;
        }

        OpenGates();

        charmPanel.DOColor(new Color(0, 255.0f*0.75f, 0), 1.0f);
    }


    [SerializeField] AudioSource gateOpenSound;
    [SerializeField] AudioClip gateOpenClip;

    [Button("Open Gates")]
    void OpenGates() {
        gateOpenSound.PlayOneShot(gateOpenClip);
        leftGate.DOLocalRotate(new Vector3(0, -75f, 0), 2.0f);
        rightGate.DOLocalRotate(new Vector3(0, 75f, 0), 2.0f);
    }

    public bool HasCollectedAllCharms() { 
        for (int i=0; i<charmCollection.charmCollection.Length; i++) {
            if (!charmCollection.charmCollection[i]) {
                return false;
            }
        }
        return true;
    }

    void EnableCharmImage(int charmIndex) {
        int imgIndex = charmImageIndex[charmIndex];
        charmImages[imgIndex].DOColor(Color.white, 1.0f);

        DOTween.Sequence()
            .Append(charmImages[imgIndex].transform.DOScale(1.2f, 3.0f))
            .Append(charmImages[imgIndex].transform.DOScale(1f, 3.0f))
            .SetLoops(-1)
            .Play();
    }

    public void OnSceneExitStart() {
        charmCanvas.gameObject.SetActive(false);
    }
}
