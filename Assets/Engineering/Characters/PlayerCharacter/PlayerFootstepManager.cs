using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepManager : MonoBehaviour
{
    [SerializeField] private FootstepManager footstepManager;
    [SerializeField] private AudioSource walkFootstepSource;
    public void FootstepWalk() {
        AudioClip clip = footstepManager.GetWalkSound();
        walkFootstepSource.clip = clip;
        walkFootstepSource.Play();
    }

    public void FootstepRun() {
        AudioClip clip = footstepManager.GetRunSound();
        walkFootstepSource.clip = clip;
        walkFootstepSource.Play();
    }

    public void FootstepJump() {
        AudioClip clip = footstepManager.GetJumpSound();
        walkFootstepSource.clip = clip;
        walkFootstepSource.Play();
    }

    public void FootstepLand() {
        AudioClip clip = footstepManager.GetLandSound();
        walkFootstepSource.clip = clip;
        walkFootstepSource.Play();
    }

}
