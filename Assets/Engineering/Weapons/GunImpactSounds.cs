using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunImpactSounds", menuName = "ScriptableObjects/DataContainers/GunImpactSounds")]
public class GunImpactSounds : ScriptableObject
{
    [SerializeField] private AudioClip[] concrete;
    [SerializeField] private AudioClip[] metal;
    [SerializeField] private AudioClip[] glass;

    public AudioClip[] Concrete { get { return concrete; } }
    public AudioClip[] Metal { get { return metal; } }
    public AudioClip[] Glass { get { return glass; } }

}
