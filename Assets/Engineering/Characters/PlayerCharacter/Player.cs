using PlayerFiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject playerObject;

    CharacterObject currentCharacter;

    bool shootPress;
    bool shootHold;
    bool aimPress;
    bool aimHold;
    bool reloadPress;
    bool reloadHold;
    bool swapPress;
    bool swapHold;
    bool stowPress;
    bool stowHold;
    bool interactPress;
    bool interactHold;
    bool dropPress;
    bool dropHold;

    bool sprintPress;
    bool sprintHold;
    bool crouchPress;
    bool crouchHold;
    bool jumpPress;
    bool jumpHold;

    Vector2 move;
    Vector2 look;
    // Start is called before the first frame update
    PlayerMoveController pc;
    void Start()
    {
        currentCharacter = new CharacterObject(playerObject, playerObject.GetComponent<PlayerMovementStateMachine>(), playerObject.GetComponent<PlayerActionStateMachine>());
        pc = playerObject.GetComponent<PlayerMoveController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (pc != null) {
            HandlePlayerMovementInput();
            HandlePlayerActionInput();

        }

        shootPress = false;
        aimPress = false;
        reloadPress = false;
        swapPress = false;
        stowPress = false;
        interactPress = false;
        dropPress = false;

        sprintPress = false;
        crouchPress = false;
        jumpPress = false;
    }


    public void OnShoot(InputAction.CallbackContext context) {
        if (context.started) {
            shootPress = true;
            shootHold = true;
        }

        if (context.canceled) {
            shootHold = false;
        }
    }
    public void OnReload(InputAction.CallbackContext context) {
        if (context.started) {
            reloadPress = true;
            reloadHold = true;
        }

        if (context.canceled) {
            reloadHold = false;
        }
    }
    public void OnAim(InputAction.CallbackContext context) {
        if (context.started) {
            aimPress = true;
            aimHold = true;
        }

        if (context.canceled) {
            aimHold = false;
        }
    }
    public void OnSwap(InputAction.CallbackContext context) {
        if (context.started) {
            swapPress = true;
            swapHold = true;
        }

        if (context.canceled) {
            swapHold = false;
        }
    }
    public void OnSprint(InputAction.CallbackContext context) {
        if (context.started) {
            sprintPress = true;
            sprintHold = true;
        }

        if (context.canceled) {
            sprintHold = false;
        }
    }
    public void OnCrouch(InputAction.CallbackContext context) {
        if (context.started) {
            crouchPress = true;
            crouchHold = true;
        }

        if (context.canceled) {
            crouchHold = false;
        }
    }
    public void OnJump(InputAction.CallbackContext context) {
        if (context.started) {
            jumpPress = true;
            jumpHold = true;
        }

        if (context.canceled) {
            jumpHold = false;
        }
    }
    public void OnStow(InputAction.CallbackContext context) {
        if (context.started) {
            stowPress = true;
            stowHold = true;
        }

        if (context.canceled) {
            stowHold = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if (context.started) {
            interactPress = true;
            interactHold = true;
        }

        if (context.canceled) {
            interactHold = false;
        }
    }

    public void OnDrop(InputAction.CallbackContext context) {
        if (context.started) {
            dropPress = true;
            dropHold = true;
        }

        if (context.canceled) {
            dropHold = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        move = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context) {
        look = context.ReadValue<Vector2>();
    }
    public void HandlePlayerActionInput() {
        currentCharacter.playerActionStateMachine.SetInputs(
            reloadPress,
            reloadHold,
            swapPress,
            swapHold,
            shootPress,
            shootHold,
            aimPress,
            aimHold,
            stowPress,
            stowHold,
            interactPress,
            interactHold, 
            dropPress, 
            dropHold,
            look
            );
    }

    public void HandlePlayerMovementInput(){
        currentCharacter.playerMovementStateMachine.SetInput(
            move,
            look,
            sprintPress,
            sprintHold,
            crouchPress,
            crouchHold,
            jumpPress,
            jumpHold
            );
    }
}

public struct CharacterObject
{
    public GameObject character;
    public PlayerMovementStateMachine playerMovementStateMachine;
    public PlayerActionStateMachine playerActionStateMachine;

    public CharacterObject(GameObject character, PlayerMovementStateMachine playerMovementStateMachine, PlayerActionStateMachine playerActionStateMachine) {
        this.character = character;
        this.playerMovementStateMachine = playerMovementStateMachine;
        this.playerActionStateMachine = playerActionStateMachine;
    }
}
