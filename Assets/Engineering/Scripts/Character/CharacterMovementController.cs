using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ScriptableObjectDependencyInjection;
public class CharacterMovementController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] FloatReference speed;
    void Start()
    {
    }

    void Update()
    {
        float xMov = animator.GetFloat("MovementX");
        float yMov = animator.GetFloat("MovementY");
        
        float nextXMov = Mathf.Lerp(xMov, moveData.x, 0.15f);
        float nextYMov = Mathf.Lerp(yMov, moveData.y, 0.15f); 

        animator.SetFloat("MovementX", nextXMov);
        animator.SetFloat("MovementY", nextYMov);
        animator.SetBool("Crouch", crouching);
    }



    Vector2 moveData = Vector2.zero;
    bool jumping = false;
    bool crouching = false;
    public void OnMove(InputValue context) {
        moveData = context.Get<Vector2>();
    }
    public void OnJump(InputValue context) {
        Debug.Log("Jump 2");
        jumping = context.isPressed;
    }
    public void OnCrouch(InputValue context) {
        Debug.Log("Crouch 2");
        crouching = context.Get<float>() == 1;
    }

    public void OnGameEventTrigger() {
        Debug.Log("YOU HAVE TRIGGERED AN EVENT");
    }
}
