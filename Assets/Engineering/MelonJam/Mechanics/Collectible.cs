using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Collectible : MonoBehaviour
{
    [Title("Collectible")]
    [SerializeField] int collectibleId;
    [SerializeField] private IntEvent collectiblePickupEvent;

    [Title("Bobbing")]
    [SerializeField] float bobSpeed = 5f;
    [SerializeField] float bobDistance = 0.25f;
    [SerializeField] float rotateTime = 10f;
    [SerializeField] float yDiffRotateMultiplier = 20f;

    [Title("Sound")]
    [SerializeField] AudioSource pickupSoundSource;
    [SerializeField] AudioClip pickupSoundClip;

    [Title("Effects")]
    [SerializeField] ParticleSystem[] pickupEffects;

    [Title("Misc")]
    [SerializeField] float destroyTimePad = 0.1f;

    bool deactivated = false;
    bool trackPlayer = false;
    GameObject player;



    private void Start() {
        baseYPos = transform.position.y;
    }

    private void Update() {
        BobRotate();

        if (trackPlayer) { 
            transform.position = player.transform.position + Vector3.up * 1.5f; 
        }
    }


    float baseYPos;
    void BobRotate() {
        float ydiff = transform.position.y - baseYPos;
        float remappedYDiff = ydiff.MapValue(0, 0.5f, 0f, yDiffRotateMultiplier);
        remappedYDiff = 1 + yDiffRotateMultiplier - remappedYDiff;
        
        transform.Rotate(new Vector3(0f, 1f, 0f), (rotateTime * remappedYDiff)* Time.deltaTime);
        transform.Translate(new Vector3(0f, Mathf.Sin(Time.time * bobSpeed) * bobDistance * Time.deltaTime, 0f));
    }

    /// <summary>
    /// External function to move a charm to a target position
    /// </summary>
    /// <param name="target"></param>
    /// <param name="time"></param>
    public void MoveCharm(Vector3 target, float time) {
        transform.DOMove(target, time);
        DOTween.To(() => baseYPos, x => baseYPos = x, target.y, time);
    }

    private void OnTriggerEnter(Collider other) {
        if (deactivated) return;
        if (!other.gameObject.CompareTag("Player")) { return; }

        PickUpCharm(other.gameObject);

        collectiblePickupEvent.Raise(collectibleId);
    }

    void PickUpCharm(GameObject player) {
        // disable everything
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
        // enable effects
        foreach (ParticleSystem particle in pickupEffects) {
            particle.gameObject.SetActive(true);
            particle.Play();
        }
        // play sound
        pickupSoundSource.clip = pickupSoundClip;
        pickupSoundSource.pitch = Random.Range(0.8f, 1.2f);
        pickupSoundSource.Play();


        deactivated = true;
        // sound follows player
        trackPlayer = true;
        this.player = player;

        Destroy(gameObject, pickupSoundClip.length + destroyTimePad);
    }


#if UNITY_EDITOR
    [Button("Pickup")]
    void PickUpTest() {
        GameObject gob = GameObject.FindGameObjectWithTag("Player");
        PickUpCharm(gob);

        collectiblePickupEvent.Raise(collectibleId);
    }
#endif
}
