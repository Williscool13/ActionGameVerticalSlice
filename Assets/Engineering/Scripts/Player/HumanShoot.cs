using PlayerFiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanShoot : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Animator anim;
    [SerializeField] private float cooldown = 0.1f;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip shootClip;
    float timestamp = 0;

    [SerializeField] PlayerMovementStateMachine playerMovementStateMachine;

    public bool MidAction => midAction;
    bool midAction;
    void Update()
    {
        if (fireHold) {
            // get gun and call Shoot Function
            if (timestamp > cooldown) {
                anim.SetTrigger("Shoot");
                Debug.Log("SHOT");
                timestamp = 0;

                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(shootClip);
            }
        }

        timestamp += Time.deltaTime;
        firePress = false;
        reloadPress = false;
        swapPress = false;
    }

    bool fireHold = false;
    bool firePress = false;
    bool reloadHold = false;
    bool reloadPress = false;
    bool swapHold = false;
    bool swapPress = false;
    public void OnFire(InputAction.CallbackContext context) {
        if (context.started) {
            fireHold = true;
            firePress = true;
        } 
        if (context.canceled) {             
            fireHold = false;
        }


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnReload(InputAction.CallbackContext context) {
        if (context.started) {
            reloadHold = true;
            reloadPress = true; 
        }

        if (context.canceled) {
            reloadHold = false;
        }
    }
    public void OnWeaponSwap(InputAction.CallbackContext context) {
        if (context.started) {
            swapHold = true;
            swapPress = true;
        }
        if (context.canceled) {
            swapHold = false;
        }
    }

}
