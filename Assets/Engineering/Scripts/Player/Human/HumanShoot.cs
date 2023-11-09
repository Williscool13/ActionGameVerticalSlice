using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanShoot : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Shot: " + shot);
        if (shot) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    bool shot = false;
    public void OnFire(InputAction.CallbackContext context) {
        if (context.started) {
            shot = true;
        } 
        if (context.canceled) {             
            shot = false;
        }

    }
}
