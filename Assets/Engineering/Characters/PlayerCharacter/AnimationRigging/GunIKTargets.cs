using System;
using UnityEngine;

[Serializable]
public struct GunIKTargets
{
    [SerializeField] private LeftWristIK leftWristGunIk;
    [SerializeField] private RightWristIK rightWristGunIk;
    [SerializeField] private GunAnchorPoint anchorPoint;
    [SerializeField] private float priority;

    public LeftWristIK LeftWristGunIk => leftWristGunIk;
    public RightWristIK RightWristGunIk => rightWristGunIk;
    public GunAnchorPoint AnchorPoint => anchorPoint;
    public float Priority => priority;


    public GunIKTargets(LeftWristIK leftWristGunIk, RightWristIK rightWristGunIk, GunAnchorPoint anchorPoint, float priority) {
        this.leftWristGunIk = leftWristGunIk;
        this.rightWristGunIk = rightWristGunIk;
        this.anchorPoint = anchorPoint;
        this.priority = priority;
    }



    public enum LeftWristIK
    {
        None,
        Gun,
        Waist
    }

    public enum RightWristIK
    {
        None,
        Gun
    }

    public enum GunAnchorPoint
    {
        None,
        RightArm,
        ShoulderIdle,
        ShoulderAim,
        ShoulderObstructed
    }
}
