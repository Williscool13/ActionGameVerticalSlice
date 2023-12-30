using Combat;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : BaseBullet
{
    [SerializeField] LayerMask targetMasks;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] MeshRenderer bulletRenderer;
    Vector3 direction;
    float damage;
    float range;
    float speed;

    bool launched;
    bool finished;
    float distanceTravelled;

    private void Update() {
        if (!launched) return;
        if (finished) return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, speed * Time.deltaTime, targetMasks)) { OnHit(hit); }
        else {
            distanceTravelled += speed * Time.deltaTime;
            if (distanceTravelled + speed * Time.deltaTime >= range) { DespawnBullet(); }
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
        bulletRenderer.enabled = true;
        finished = false;
        distanceTravelled = 0;
    }

    public override void Launch() {
        launched = true;
    }


    public void OnHit(RaycastHit hitData) {
        hitData.transform.TryGetComponent(out ITarget target);
        if (target != null) {
            switch (target.GetTargetType()) {
                case TargetType.Terrain:
                    SpawnBulletHole(hitData);
                    BaseBulletHole bbh = bulletHolePooler.Get();
                    bbh.transform.SetPositionAndRotation(hitData.point, Quaternion.LookRotation(direction));
                    bbh.Initialize(hitData.collider);
                    break;
                case TargetType.Unit:
                    target.Hit(new HitDataContainer(damage));
                    // deal damage (specify point of contact and normal for blood splatter
                    break;
            }
        } else {
            if (hitData.collider.gameObject.layer == LayerMask.NameToLayer("Terrain")) {
                SpawnBulletHole(hitData);
            }
        }
        DespawnBullet();
    }



    void SpawnBulletHole(RaycastHit hitData) {
        BaseBulletHole bbh = bulletHolePooler.Get();
        bbh.transform.SetPositionAndRotation(hitData.point, Quaternion.LookRotation(direction));
        bbh.Initialize(hitData.collider);
    }

    void DespawnBullet() {
        finished = true;
        bulletRenderer.enabled = false;

        DOTween.Sequence()
            .AppendInterval(trailRenderer.time * 1.5f) 
            .OnComplete(() => {
                bulletPooler.Release(this);

            });
        //StartCoroutine(DestroyAfterTrailFaded());
    }


    IEnumerator DestroyAfterTrailFaded() {
        yield return new WaitForSeconds(trailRenderer.time * 1.5f);
        bulletPooler.Release(this);
    }
}
