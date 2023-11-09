using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanMove : MonoBehaviour
{
    [SerializeField] PlayerInput mainInput;
    [SerializeField] Animator anim;

    [SerializeField] float lerpUpSpeed;
    [SerializeField] float lerpDownSpeed;

    Vector2 currMove = Vector2.zero;
    void Update()
    {
        Vector2 movement = mainInput.actions["Move"].ReadValue<Vector2>();

        if (movement.x != 0) {
            currMove.x = Mathf.Lerp(currMove.x, movement.x, lerpUpSpeed * Time.deltaTime);
        } else {
            currMove.x = Mathf.Lerp(currMove.x, movement.x, lerpDownSpeed * Time.deltaTime);
        }

        if (movement.y != 0) {
            currMove.y = Mathf.Lerp(currMove.y, movement.y, lerpUpSpeed * Time.deltaTime);
        } else {
            currMove.y = Mathf.Lerp(currMove.y, movement.y, lerpDownSpeed * Time.deltaTime);
        }

        Debug.Log(movement.x + " " + movement.y);
        anim.SetFloat("MovementX", currMove.x);
        anim.SetFloat("MovementY", currMove.y);
    }


    int Sign(float val) {
        if (val > 0) { return 1; }
        if (val < 0) { return -1; }
        return 0;
    }
}
