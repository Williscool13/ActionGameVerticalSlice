using KinematicCharacterController.Examples;
using PlayerFiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelScripts
{
    public class OneWayTeleporter : MonoBehaviour
    {
        [SerializeField] private Transform teleportTarget;


        private void OnTriggerEnter(Collider other) {
            PlayerMovementStateMachine pms = other.GetComponent<PlayerMovementStateMachine>();
            if (pms) { 
                pms.Teleport(teleportTarget.transform.position, teleportTarget.transform.rotation);
            }
        }
    }

}