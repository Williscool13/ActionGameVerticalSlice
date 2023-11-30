using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FootstepCollection", menuName = "ScriptableObjects/DataContainers/FootstepCollection")]
public class FootstepCollection : ScriptableObject
{
    public List<AudioClip> footstepWalkSounds = new();
    public List<AudioClip> footstepRunSounds = new();
    public List<AudioClip> footstepJumpSounds = new();
    public List<AudioClip> footstepLandSounds = new();
}
