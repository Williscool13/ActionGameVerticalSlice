using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(fileName = "ShootAction", menuName = "Finite State Machine/Player Action/Actions/Common/Shoot")]
    public class ShootAction : PlayerActionStateAction {
        [SerializeField] PlayerMovementState[] whiteList;
        public override void Enter(PlayerActionStateMachine machine) {
        }

        public override void Exit(PlayerActionStateMachine machine) {
        }

        public override void Execute(PlayerActionStateMachine machine) {
            // if rig is in position
            if (!machine.IsRigInPosition()) { return; }
            // check if gun can shoot
            WeaponBase currentWeapon = machine.GetCurrentWeapon();
            if (currentWeapon == null) { return; }

            bool canFire =
                (machine.Inputs.ShootPress && currentWeapon.CanFire(false))
                || (machine.Inputs.ShootHold && currentWeapon.CanFire(true));
            canFire &= whiteList.Contains(machine.PlayerMovementStateMachine.CurrentState);

            if (!canFire) { return; }



            RecoilData data = currentWeapon.Fire();
            // call gun fire
            Debug.Log("Shot");
            // run recoil 
            machine.AddRecoil(data);
        }
    }
}
