using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanLook : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] float mouseSensitivity = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseDelta = playerInput.actions["Look"].ReadValue<Vector2>();
        transform.Rotate(new Vector3(0, mouseDelta.x * Time.deltaTime * mouseSensitivity, 0));
    }
}
