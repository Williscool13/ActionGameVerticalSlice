using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    //[SerializeField] private Vector3 jumpForce = Vector3.up * 100 + Vector3.forward * 10;
    [SerializeField] private Transform particleSystemParent;
    [SerializeField] private ParticleSystem[] activationParticles;

    [Title("Sound")]
    [SerializeField] private AudioSource wooshSource;
    [SerializeField] private AudioClip wooshClip;

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Player")) return;

        ICharacterMovement player = other.gameObject.GetComponent<ICharacterMovement>();
        if (player == null) return;

        player.ApplyForce(transform.forward * jumpForce, ForceMode.Impulse);
        particleSystemParent.position = other.transform.position + Vector3.up * 1.5f;
        foreach (ParticleSystem particleSystem in activationParticles) {
            particleSystem.Play();
        }

        wooshSource.PlayOneShot(wooshClip);
    }

    

    [SerializeField] private SphereCollider sphereCollider;
    private void OnDrawGizmos() {
        if (sphereCollider == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
    }
}
