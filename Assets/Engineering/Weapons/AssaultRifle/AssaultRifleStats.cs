using UnityEngine;

[CreateAssetMenu(fileName = "AssaultRifleStats", menuName = "ScriptableObjects/DataContainers/AssaultRifleStats")]
public class AssaultRifleStats : ScriptableObject
{
    public float damage;
    public float range;
    public float speed;
    public float fireRate;
    public float reloadSpeedMultiplier;
    public int magazineSize;
    public int reserveAmmo;

    public Vector2 recoilPower;
    public float recoilDuration;
    public float kickbackStrength;
    public Vector2 cinemachineRecoilVelocity;
}