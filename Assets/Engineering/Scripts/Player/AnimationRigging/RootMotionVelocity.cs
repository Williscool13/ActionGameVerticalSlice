using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionVelocity : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private void OnAnimatorMove() {
        Vector3 velocity = animator.deltaPosition / Time.deltaTime;
        velocity.y = 0;
        //transform.position += velocity;
    }
}
