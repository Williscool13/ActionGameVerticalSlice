using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Charm : MonoBehaviour
{
    [SerializeField] int charmId;


    [SerializeField] private IntEvent charmPickUpEvent;

    [Title("Sound")]
    [SerializeField] AudioSource pickupSoundSource;
    [SerializeField] AudioClip pickupSoundClip;

    [SerializeField] ParticleSystem[] pickupEffects; 

    bool deactivated = false;
    private void OnTriggerEnter(Collider other) {
        if (deactivated) return;
        if (!other.gameObject.CompareTag("Player")) { return; }

        PickUpCharm(other.gameObject);

        charmPickUpEvent.Raise(charmId);
    }

    [Button("Pickup")]
    void PickUpTest() {
        GameObject gob = GameObject.FindGameObjectWithTag("Player");
        PickUpCharm(gob);

        charmPickUpEvent.Raise(charmId);
    }

    void PickUpCharm(GameObject player) {

        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }

        foreach (ParticleSystem particle in pickupEffects) {
            particle.gameObject.SetActive(true);
            particle.Play();
        }

        pickupSoundSource.clip = pickupSoundClip;
        pickupSoundSource.pitch = Random.Range(0.8f, 1.2f);
        pickupSoundSource.Play();

        deactivated = true;

        trackPlayer = true;
        this.player = player;

        Destroy(gameObject, 3.0f);
    }

    bool trackPlayer = false;
    GameObject player;

    [SerializeField] float bobSpeed = 5f;
    [SerializeField] float bobDistance = 0.25f;
    [SerializeField] float rotateTime = 10f;
    [SerializeField] float yDiffRotateMultiplier = 20f;

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

        float remappedYDiff = MapValue(ydiff, new Vector2(0, 0.5f), new Vector2(0f, yDiffRotateMultiplier));
        remappedYDiff = 1 + yDiffRotateMultiplier - remappedYDiff;
        
        transform.Rotate(new Vector3(0f, 1f, 0f), (rotateTime * remappedYDiff)* Time.deltaTime);
        transform.Translate(new Vector3(0f, Mathf.Sin(Time.time * bobSpeed) * bobDistance * Time.deltaTime, 0f));
    }

    public void MoveCharm(Vector3 target, float time) {
        transform.DOMove(target, time);
        DOTween.To(() => baseYPos, x => baseYPos = x, target.y, time);
    }
    float MapValue(float value, Vector2 from, Vector2 to) => (value - from.x) / (from.y - from.x) * (to.y - to.x) + to.x;


}
