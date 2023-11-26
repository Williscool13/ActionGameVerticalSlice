using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ITarget
{
    void Hit(HitDataContainer damage);

    event EventHandler<HitDataContainer> OnTargetHit;
}
