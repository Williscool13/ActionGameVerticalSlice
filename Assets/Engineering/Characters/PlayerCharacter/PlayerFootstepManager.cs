using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepManager : MonoBehaviour
{
    [SerializeField] private FootstepManager footstepManager;
    [SerializeField] private AudioSource walkFootstepSource;
    [SerializeField] private bool oneShot = false;
    public void FootstepWalk() {
        AudioClip clip = footstepManager.GetWalkSound();
        if (oneShot) {
            walkFootstepSource.PlayOneShot(clip);
            return;
        }
        walkFootstepSource.clip = clip;
        walkFootstepSource.Play();
    }

    public void FootstepRun() {
        AudioClip clip = footstepManager.GetRunSound();
        if (oneShot) {
            walkFootstepSource.PlayOneShot(clip);
            return;
        }
        walkFootstepSource.clip = clip;
        walkFootstepSource.Play();
    }

    public void FootstepJump() {
        AudioClip clip = footstepManager.GetJumpSound();
        if (oneShot) {
            walkFootstepSource.PlayOneShot(clip);
            return;
        }
        walkFootstepSource.clip = clip;
        walkFootstepSource.Play();
    }

    public void FootstepLand() {
        AudioClip clip = footstepManager.GetLandSound();
        if (oneShot) {
            walkFootstepSource.PlayOneShot(clip);
            return;
        }
        walkFootstepSource.clip = clip;
        walkFootstepSource.Play();
    }

}
