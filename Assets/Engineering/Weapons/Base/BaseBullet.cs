using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class BaseBullet : MonoBehaviour
{


    public abstract void Initialize(float damage, Vector3 spawnPosition, Vector3 direction, float range, float speed);
    public abstract void Launch();

    public void SetBulletHolePooler(IObjectPool<BaseBulletHole> pool) { this.bulletHolePooler = pool; }
    public void SetBulletPooler(IObjectPool<BaseBullet> pool) { this.bulletPooler = pool; }

    protected IObjectPool<BaseBullet> bulletPooler;
    protected IObjectPool<BaseBulletHole> bulletHolePooler;

    public abstract void OnGetFromPool();
    public abstract void OnReleaseToPool();



    protected virtual void Release() {
        bulletPooler.Release(this);
    }
}
