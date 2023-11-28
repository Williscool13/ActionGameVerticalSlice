using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(fileName = "IdleShoot", menuName = "Finite State Machine/Player Action/Actions/Idle/Idle Shoot")]
    public class IdleShoot : PlayerActionStateAction
    {
        public override void Execute(PlayerActionStateMachine machine) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // if rig is in position
            if (!machine.GunRigController.RigInPosition()) { return; }
            // check if gun can shoot
            WeaponBase currentWeapon = machine.PlayerLoadoutManager.GetCurrentWeapon();
            if (currentWeapon == null) { return; }

            bool canFire = 
                (machine.Inputs.ShootPress && currentWeapon.CanFire(false)) 
                || (machine.Inputs.ShootHold && currentWeapon.CanFire(true));

            if (!canFire) { return; }
            
            RecoilData data = currentWeapon.Fire();
            // call gun fire
            Debug.Log("Shot");
            // run recoil 
            machine.PlayerAimController.AddRecoil(data);
        }
    }
}