using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDisintegrationZone : MonoBehaviour
{
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] ParticleSystem[] disintegrationParticles;
    [SerializeField] GameObject particleSystemParent;
    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out RockInstance ri)){

            particleSystemParent.transform.position = ri.transform.position;
            foreach (ParticleSystem ps in disintegrationParticles) { ps.Play(); }
            Destroy(ri.transform.root.gameObject);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxCollider.size);
    }
}
