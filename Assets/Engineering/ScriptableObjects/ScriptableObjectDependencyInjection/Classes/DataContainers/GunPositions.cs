using UnityEngine;


namespace ScriptableObjectDependencyInjection
{
    [CreateAssetMenu(fileName = "GunPositions", menuName = "ScriptableObjects/DataContainers/GunPositions")]
    public class GunPositions : ScriptableObject
    {
        public Vector3 idleLocalPosition;
        public Vector3 aimLocalPosition;
        public Vector3 walkLocalPosition;
        public Vector3 crouchLocalPosition;
    }
}