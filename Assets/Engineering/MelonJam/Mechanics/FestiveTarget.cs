using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FestiveTarget : MonoBehaviour, IFestiveTarget
{
    [SerializeField] private Transform[] nonSantaMeshes;
    [SerializeField] private Transform[] christmasMeshes;
    [SerializeField] private ParticleSystem santaTransformationParticles;

    [SerializeField] private float health = 100.0f;
    [SerializeField][ReadOnly] private float currentHealth = 100.0f;

    [Title("Sound")]
    [SerializeField] private AudioSource jingleSource;
    [SerializeField] private AudioClip jingleClip;

    [Title("Animator Properties")]
    [SerializeField] private Animator festiveTargetAnimator;
    [SerializeField] private float idleAnimCooldown = 5.0f;


    [Title("Ragdoll Properties")]

    [SerializeField] private Transform[] ragdollChristmasMeshes;
    [SerializeField] private Rigidbody ragdollCenterMass;
    [SerializeField] private GameObject ragdollObject;
    [SerializeField] private float ragdollForceMultiplier = 10.0f;

    [Title("Game Statistics")]
    [SerializeField] private NullEvent festiveConvertedEvent;
    [SerializeField] private NullEvent christmasCasualtiesEvent;


    public event EventHandler<EventArgs> convertedEvent;

    void Start()
    {
        nonSantaMeshes[UnityEngine.Random.Range(0, nonSantaMeshes.Length)].gameObject.SetActive(true);
        currentHealth = health;
        santaTransformationParticles.Stop();
    }

    float idleAnimTimer = 0.0f;

    private void Update() {
        idleAnimTimer += Time.deltaTime;
        if (idleAnimTimer > idleAnimCooldown) {
            idleAnimTimer = 0.0f;
            if (santa) {
                festiveTargetAnimator.SetInteger("Animation_int", 4);
            } else {
                festiveTargetAnimator.SetInteger("Animation_int", UnityEngine.Random.Range(0, 9) + 1);
            }
        } 
    }

    bool santa = false;
    bool ragdoll = false;
    [Button("Hit")]
    public void HitTest() {
        this.OnHit(new HitData(transform.position, transform.forward, 100f));
    }
    public void OnHit(HitData hitdata) {
        if (ragdoll) return;

        if (santa) {
            Ragdoll(hitdata); 
            jingleSource.clip = jingleClip;
            jingleSource.Play();
            DestroyAfterDelay();
            ragdoll = true;
            return;
        }
        currentHealth -= hitdata.damage;

        if (currentHealth <= 0.0f) {
            int index = UnityEngine.Random.Range(0, christmasMeshes.Length);
            christmasMeshes[index].gameObject.SetActive(true);
            ragdollChristmasMeshes[index].gameObject.SetActive(true);
            santaTransformationParticles.Play();
            foreach (Transform t in nonSantaMeshes) {
                t.gameObject.SetActive(false);
            }

            santa = true;
            jingleSource.clip = jingleClip;
            jingleSource.Play();

            convertedEvent?.Invoke(this, EventArgs.Empty);
            festiveConvertedEvent.Raise(null);
        }
    }

    void Ragdoll(HitData hitData) {
        for (int i = 0; i < christmasMeshes.Length; i++) {
            christmasMeshes[i].gameObject.SetActive(false);    
        }

        ragdollObject.SetActive(true);

        ragdollCenterMass.AddForce(hitData.directionOfContact * ragdollForceMultiplier, ForceMode.Impulse);

        santaTransformationParticles.Stop();
        santaTransformationParticles.Clear();

        GetComponent<Collider>().enabled = false;

        christmasCasualtiesEvent.Raise(null);
    }

    void DestroyAfterDelay() {
        Destroy(this.gameObject, 10.0f);
    }
}
