using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBulletHole : BaseBulletHole
{
    [SerializeField] GunImpactSounds sounds;

    public override void Initialize(Collider collidedObject) {
        // get object material

        // play sound
        // play vfx
        // change bullet hole decal image
    }
}
