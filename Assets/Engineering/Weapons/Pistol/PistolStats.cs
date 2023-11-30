using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PistolStats", menuName = "ScriptableObjects/DataContainers/PistolStats")]
public class PistolStats : ScriptableObject
{
    public float damage;
    public float range;
    public float speed;
    public float fireRate;
    public float reloadSpeedMultiplier;
    public int magazineSize;
    public int reserveAmmo;
    public bool infiniteAmmo;

    public Vector2 recoilPower;
    public float recoilDuration;
    public float kickbackStrength;
    public Vector2 cinemachineRecoilVelocity;
}