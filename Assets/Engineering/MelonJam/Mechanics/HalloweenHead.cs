using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class HalloweenHead : MonoBehaviour, IFestiveTarget
{
    [SerializeField] private Transform[] headModels;

    [SerializeField] private bool randomizeHead = false;

    [HideIf("randomizeHead")][SerializeField] private bool randomizeInitialHead = false;
    [HideIf("randomizeInitialHead")][SerializeField] private int initialHeadModel = 0;
    [HideIf("randomizeHead")][SerializeField] private int targetHeadModel = 0;
    [SerializeField] private ParticleSystem[] transformationParticles;
    [SerializeField] private ParticleSystem[] solvedParticles;
    [SerializeField][ReadOnly] int currentHead;
    [SerializeField][ReadOnly] int internalTargetHead;

    [SerializeField] private AudioSource jingleSource;
    [SerializeField] private AudioClip jingleClip;
    public bool Locked { get; set; } = false;

    public event EventHandler<EventArgs> convertedEvent;
    void Start()
    {
        if (randomizeHead) {
            initialHeadModel = UnityEngine.Random.Range(0, headModels.Length);

            targetHeadModel = UnityEngine.Random.Range(0, headModels.Length);
            while (targetHeadModel == initialHeadModel) {
                targetHeadModel = UnityEngine.Random.Range(0, headModels.Length);
            }
        } else if (randomizeInitialHead) {
            initialHeadModel = UnityEngine.Random.Range(0, headModels.Length);
            while (initialHeadModel == targetHeadModel) {
                initialHeadModel = UnityEngine.Random.Range(0, headModels.Length);
            }
        }


        foreach (ParticleSystem particle in solvedParticles) {
            particle.Stop();
        }

        internalTargetHead = targetHeadModel;
        currentHead = initialHeadModel;
        ChangeModel(currentHead);
    }

    void ChangeModel(int model) {
        for (int i=0; i<headModels.Length; i++) {
            if (i == model) {
                headModels[i].gameObject.SetActive(true);
            } else {
                headModels[i].gameObject.SetActive(false);
            }
        }

    }

    public void ActivateSolvedParticles() {
        foreach (ParticleSystem particle in solvedParticles) {
            particle.Play();
        }
    }



    public void OnHit(HitData damageData) {
        if (Locked) return;

        currentHead++;
        if (currentHead >= headModels.Length) { currentHead = 0; }
        ChangeModel(currentHead);


        foreach (ParticleSystem particle in transformationParticles) {
            particle.Play();
        }

        convertedEvent?.Invoke(this, EventArgs.Empty);

        jingleSource.PlayOneShot(jingleClip);
    }

    public bool IsSolved() {
        return currentHead == targetHeadModel;
    }


}
