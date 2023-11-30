using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Combat
{
    public interface ITarget
    {
        TargetType GetTargetType();
        void Hit(HitDataContainer damage);

        event EventHandler<HitDataContainer> OnTargetHit;
    }

    public enum TargetType
    {
        Terrain,
        Unit
    }

}