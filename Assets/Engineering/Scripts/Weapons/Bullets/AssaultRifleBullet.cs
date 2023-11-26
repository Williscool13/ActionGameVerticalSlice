using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifleBullet : BaseBullet
{
    [SerializeField] LayerMask targetMasks;

    Vector3 direction;
    float damage;
    float range;
    float speed;



    bool launched;
    float distanceTravelled;
    private void Start() {
        this.hideFlags = HideFlags.HideInHierarchy;
    }

    private void Update() {
        if (!launched) return;



        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, speed * Time.deltaTime, targetMasks)) {
            OnHit(hit);

            
            return;
        }

        distanceTravelled += speed * Time.deltaTime;
        if (distanceTravelled >= range) {
            bulletPooler.Release(this);
            return;
        }

        transform.position += speed * Time.deltaTime * direction;

    }



    public override void Initialize(float damage, Vector3 spawnPosition, Vector3 direction, float range, float speed) {
        transform.SetPositionAndRotation(spawnPosition, Quaternion.LookRotation(direction));

        this.damage = damage;
        this.direction = direction;
        this.range = range;
        this.speed = speed;

        launched = false;
        distanceTravelled = 0;
    }

    public override void Launch() {
        launched = true;
    }

    
    public override void OnHit(RaycastHit hitData) {
        hitData.transform.TryGetComponent(out ITarget target);
        if (target == null) {
            BaseBulletHole bbh = bulletHolePooler.Get();
            bbh.transform.SetPositionAndRotation(hitData.point, Quaternion.LookRotation(direction));


            // get material and do below accordingly
            // bullet hole
            // bullet hole vfx
        }
        else {
            Debug.Log("Did damage to collider object");
            target.Hit(new HitDataContainer(damage));
            // blood vfx
        }         
        // make hit sound
        Debug.Log("Bullet hit something " + hitData.collider.name);
        bulletPooler.Release(this);
    }

}
