using Combat;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicBullet : BaseBullet
{

    [SerializeField]
    TrailRenderer[] trails;
    [SerializeField]
    ParticleSystem[] particles;



    Vector3 direction;
    float damage;
    float range;
    float speed;

    Rigidbody thisRb;
    bool projectileActive = false;
    float timeAlive = 0;


    public void OnHit(Collision col) {
        if (!projectileActive) { return; }
        if(col.transform.TryGetComponent(out IFestiveTarget target)) {
            target.OnHit(new HitData(col.GetContact(0).point, thisRb.velocity, damage));
            foreach (TrailRenderer trail in trails) {
                trail.Clear();
                trail.enabled = false;
            }
            foreach (ParticleSystem particle in particles) {
                particle.Clear();
                particle.Stop();
            }
            projectileActive = false;
        }
    }

    void Update()
    {
        timeAlive += Time.deltaTime;
        
        if (timeAlive > 10) {
            Release();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        OnHit(collision);
    }

    protected override void Release() {
        thisRb.velocity = Vector3.zero;
        base.Release();
    }
    public override void OnGetFromPool() {
        foreach (TrailRenderer trail in trails) {
            trail.Clear();
            trail.enabled = false;
        }

        foreach (ParticleSystem particle in particles) {
            particle.Clear();
            particle.Stop();
        }
    }
    public override void OnReleaseToPool() { }

    public override void Initialize(float damage, Vector3 spawnPosition, Vector3 direction, float range, float speed) {


        transform.SetPositionAndRotation(spawnPosition, Quaternion.LookRotation(direction));

        this.damage = damage;
        this.direction = direction;
        this.range = range;
        this.speed = speed;


        this.timeAlive = 0;

        thisRb = GetComponent<Rigidbody>();
        thisRb.velocity = Vector3.zero;
    }


    public override void Launch() {
        thisRb.AddForce(direction * speed, ForceMode.Impulse);
        thisRb.AddTorque(UnityEngine.Random.insideUnitSphere * 10, ForceMode.Impulse);

        foreach (TrailRenderer trail in trails) {
            trail.Clear();
            trail.enabled = true;
        }

        foreach (ParticleSystem particle in particles) {
            particle.Clear();
            particle.Play();
        }

        projectileActive = true;
    }
}

public interface IFestiveTarget
{
    public event EventHandler<EventArgs> convertedEvent;
    void OnHit(HitData damageData);
}

public class HitData
{
    public Vector3 pointOfContact;
    public Vector3 directionOfContact;
    public float damage;

    public HitData(Vector3 pointOfContact, Vector3 directionOfContact, float damage) {
        this.pointOfContact = pointOfContact;
        this.directionOfContact = directionOfContact;
        this.damage = damage;
    }
}