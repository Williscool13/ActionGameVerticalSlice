using Cinemachine;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : WeaponBase
{
    public override string WeaponName => stats.weaponName;
    public override bool UsesBullets => true;
    public override bool ImpactDamage => true;

    [SerializeField] private GunStats stats;

    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletHolePrefab;
     
    [SerializeField] private AudioSource shootSource;
    [SerializeField] private AudioClip shootClip;

    [SerializeField] private ParticleSystem[] muzzleFlashParticles;

    [SerializeField] private bool hasAmmoText;
    [ShowIf("hasAmmoText")][SerializeField] private TextMeshPro ammoText;
    
    [SerializeField] private CinemachineImpulseSource recoilSource;
    [SerializeField] private CinemachineImpulseSource kickbackSource;


    [SerializeField] int currentAmmo;
    [SerializeField] int currentReserveAmmo;

    [SerializeField] NullEvent OnShoot;

    IObjectPool<BaseBullet> bulletPool;
    IObjectPool<BaseBulletHole> bulletHolePool;

    void Start()
    {
        currentAmmo = stats.magazineSize;
        currentReserveAmmo = stats.reserveAmmo;
        bulletPool = new ObjectPool<BaseBullet>(BulletCreate, BulletOnTakeFromPool, BulletOnReleaseToPool, BulletOnDestroyPoolObject, true, 10, 60);
        bulletHolePool = new ObjectPool<BaseBulletHole>(BulletHoleCreate, OnTakeFromPool, OnReleaseToPool, OnDestroyPoolObject, true, 10, 60);
        SetAmmoText(currentAmmo);
    }

    public override float GetReloadSpeedMultiplier() {
        return stats.reloadSpeedMultiplier;
    }

    float timeSinceLastFire = 0.0f;
    private void Update() {
        timeSinceLastFire += Time.deltaTime;
    }

    public override bool CanFire(bool buttonHold) {
        // if off cooldown (holding/pressing doesnt matter)
        if (timeSinceLastFire < 60 / stats.fireRate) { return false; }
        return currentAmmo > 0;
    }
    public override RecoilData Fire() {
        currentAmmo -= 1;
        SetAmmoText(currentAmmo);

        BaseBullet b = bulletPool.Get();
        b.Initialize(stats.damage, muzzleTransform.position, transform.forward, stats.range, stats.speed);
        b.Launch();
        shootSource.PlayOneShot(shootClip);
        for (int i = 0; i < muzzleFlashParticles.Length; i++) {
            muzzleFlashParticles[i].Play();
        }

        timeSinceLastFire = 0.0f;

        recoilSource.GenerateImpulseWithVelocity(new Vector3(stats.cinemachineRecoilVelocity.x, stats.cinemachineRecoilVelocity.y, 0));
        kickbackSource.GenerateImpulseWithVelocity(new Vector3(0, 0, -stats.kickbackStrength));

        OnShoot.Raise(null);

        return new RecoilData(new Vector2(stats.recoilPower.x, stats.recoilPower.y), stats.recoilDuration);
    }

    
    public override bool CanReload() {
        return currentReserveAmmo > 0 && currentAmmo < stats.magazineSize;
    }

    public override void ReloadEnd() {
        if (currentAmmo + currentReserveAmmo <= stats.magazineSize) {
            currentAmmo += currentReserveAmmo;
            currentReserveAmmo = 0;
            return;
        }

        int diff = stats.magazineSize - currentAmmo;

        currentAmmo = stats.magazineSize;
        SetAmmoText(currentAmmo);

        currentReserveAmmo -= diff;

        if (stats.infiniteAmmo) {
            currentReserveAmmo = stats.reserveAmmo;
        }

    }

    public override void ReloadStart() {
    }

    void SetAmmoText(int value) {
        if (!hasAmmoText) return;
        ammoText.SetText(value.ToString());
    }

    public override void AddCurrentAmmo(int count, bool exceedMax = false) {
        currentAmmo += count;
        if (!exceedMax) { currentAmmo = Mathf.Clamp(currentAmmo, 0, stats.magazineSize); }

        SetAmmoText(currentAmmo);
    }

    public override void AddReserveAmmo(int count, bool exceedMax = false) {
        currentReserveAmmo += count;
        if (!exceedMax) { currentReserveAmmo = Mathf.Clamp(currentReserveAmmo, 0, stats.reserveAmmo); }
    }

    #region Bullet Pooling
    BaseBullet BulletCreate() {
        GameObject bullet = Instantiate(bulletPrefab, new Vector3(-100, -100, -100), Quaternion.identity);
        BaseBullet baseBullet = bullet.GetComponent<BaseBullet>();

        baseBullet.SetBulletPooler(bulletPool);
        baseBullet.SetBulletHolePooler(bulletHolePool);
        return baseBullet;
    }

    void BulletOnReleaseToPool(BaseBullet item) {
        item.OnReleaseToPool();
        item.gameObject.SetActive(false);
    }
    void BulletOnTakeFromPool(BaseBullet item) {
        item.gameObject.SetActive(true);
        item.OnGetFromPool();
    }

    void BulletOnDestroyPoolObject(BaseBullet item) {
        Destroy(item.gameObject);
    }
    #endregion

    #region Bullet Hole Pooling
    BaseBulletHole BulletHoleCreate() {
        GameObject bulletHole = Instantiate(bulletHolePrefab, new Vector3(-100, -100, -100), Quaternion.identity);
        BaseBulletHole baseBulletHole = bulletHole.GetComponent<BaseBulletHole>();

        baseBulletHole.SetBulletHolePooler(bulletHolePool);

        return baseBulletHole;
    }

    void OnReleaseToPool(BaseBulletHole item) {
        item.gameObject.SetActive(false);
    }

    void OnTakeFromPool(BaseBulletHole item) {
        item.gameObject.SetActive(true);
    }

    void OnDestroyPoolObject(BaseBulletHole item) {
        Destroy(item.gameObject);
    }

    #endregion
}
