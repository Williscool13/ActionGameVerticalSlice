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



    void Update()
    {
        if (shooting) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (timestamp > cooldown) {
                anim.SetTrigger("Shoot");
                Debug.Log("SHOT");
                timestamp = 0;

                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(shootClip);
            }
        }

        if (shotPressed) {
            
        }

        timestamp += Time.deltaTime;
        shotPressed = false;
    }


    bool shooting = false;
    bool shotPressed = false;
    public void OnFire(InputAction.CallbackContext context) {
        if (context.started) {
            shooting = true;
            shotPressed = true;
        } 
        if (context.canceled) {             
            shooting = false;
            shotPressed = false;
        }

    }
}
