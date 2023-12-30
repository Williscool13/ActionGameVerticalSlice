using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GunStats", menuName = "ScriptableObjects/DataContainers/AssaultRifleStats")]
public class GunStats : ScriptableObject
{
    public string weaponName;

    public float damage;
    public float range;
    public float speed;
    public float fireRate;
    public float reloadSpeedMultiplier;
    public int magazineSize;

    public bool infiniteAmmo;

    [HideIf("infiniteAmmo")] public int reserveAmmo;


    public Vector2 recoilPower;
    public float recoilDuration;
    public float kickbackStrength;
    public Vector2 cinemachineRecoilVelocity;
}