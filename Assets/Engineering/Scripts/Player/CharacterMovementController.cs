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

    Vector2 currentSpeed =  Vector2.zero;
    void Update()
    {
        
        float xInput = moveData.x;
        float yInput = moveData.y;

        int xSign = Sign(xInput);
        int ySign = Sign(yInput);


        if (xSign != 0) {
            currentSpeed.x = Mathf.Lerp(currentSpeed.x, xSign * speed.Value, Time.deltaTime);
        } else {
            currentSpeed.x = Mathf.Lerp(currentSpeed.x, 0, Time.deltaTime);
        }

        if (ySign != 0) {
            currentSpeed.y = Mathf.Lerp(currentSpeed.y, ySign * speed.Value, Time.deltaTime);
        } else {
            currentSpeed.y = Mathf.Lerp(currentSpeed.y, 0, Time.deltaTime);
        }

        transform.Translate(new Vector3(currentSpeed.x, -9.81f, currentSpeed.y) * Time.deltaTime);

        /*
        float xMov = animator.GetFloat("MovementX");
        float yMov = animator.GetFloat("MovementY");
        
        float nextXMov = Mathf.Lerp(xMov, moveData.x, 0.15f);
        float nextYMov = Mathf.Lerp(yMov, moveData.y, 0.15f); 

        animator.SetFloat("MovementX", nextXMov);
        animator.SetFloat("MovementY", nextYMov);
        animator.SetBool("Crouch", crouching);

        animator.SetFloat("TurnDirection", mouseDelta.x);*/
        crouching = false;
        jumping = false;
        //moveData = Vector2.zero;
        //mouseDelta = Vector2.zero;
    }



    Vector2 moveData = Vector2.zero;
    Vector2 mouseDelta = Vector2.zero;

    bool jumping = false;
    bool crouching = false;
    public void OnMove(InputAction.CallbackContext context) {
        moveData = context.ReadValue<Vector2>();
        //moveData = context.Get<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context) {
        Debug.Log("Jump 2");
        jumping = true;
    }
    public void OnCrouch(InputAction.CallbackContext context) {
        Debug.Log("Crouch 2");
        //crouching = context.Get<float>() == 1;
        crouching = true;
    }

    public void OnLook(InputAction.CallbackContext context) {
        mouseDelta = context.ReadValue<Vector2>();
        //mouseDelta = context.Get<Vector2>();
    }

    public void OnGameEventTrigger() {
        Debug.Log("YOU HAVE TRIGGERED AN EVENT");
    }

    int Sign(float val) {
        if (val > 0) return 1;
        if (val < 0) return -1;
        return 0;
    }
}
