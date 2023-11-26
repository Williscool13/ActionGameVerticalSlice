using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(fileName = "AimShoot", menuName = "Finite State Machine/Player Action/Actions/Aim/Aim Shoot")]
    public class AimShoot : PlayerActionStateAction
    {
        [SerializeField] private FloatReference aimRecoilMultiplier;
        public override void Execute(PlayerActionStateMachine machine) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // if rig is in position
            if (!machine.GunRigController.RigInPosition()) { return; }
            // check if gun can shoot
            WeaponBase currentWeapon = machine.PlayerLoadoutManager.GetCurrentWeapon();
            if (currentWeapon == null) { return; }

            bool canFire =
                (machine.ShootPress && currentWeapon.CanFire(false))
                || (machine.ShootHold && currentWeapon.CanFire(true));

            if (!canFire) { return; }

            RecoilData data = currentWeapon.Fire();
            // call gun fire
            Debug.Log("Shot");
            data.RecoilKick *= aimRecoilMultiplier.Value;
            // run recoil 
            machine.PlayerAimController.AddRecoil(data);

        }
    }
}