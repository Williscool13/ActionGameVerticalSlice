using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerMovementFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Sprint/Update")]
    public class SprintUpdate : PlayerMovementStateAction
    {
        [SerializeField] float lerpUpSpeed = 12f;
        [SerializeField] float lerpDownSpeed = 20f;
        public override void Execute(PlayerMovementStateMachine machine) {
            Vector2 move = machine.MoveDelta;
            Vector2 currMove = machine.CurrentMove;
            if (move.x != 0) {
                currMove.x = Mathf.Lerp(currMove.x, move.x, lerpUpSpeed * Time.deltaTime);
            }
            else {
                currMove.x = Mathf.Lerp(currMove.x, move.x, lerpDownSpeed * Time.deltaTime);
            }

            if (move.y != 0) {
                currMove.y = Mathf.Lerp(currMove.y, move.y, lerpUpSpeed * Time.deltaTime);
            }
            else {
                currMove.y = Mathf.Lerp(currMove.y, move.y, lerpDownSpeed * Time.deltaTime);
            }


            machine.SetAnimatorFloat("MovementX", currMove.x);
            machine.SetAnimatorFloat("MovementY", currMove.y);

            machine.CurrentMove = currMove;
            Debug.Log("Sprint Update");
        }
    }
}