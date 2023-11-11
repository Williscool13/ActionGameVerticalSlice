using UnityEngine;

namespace ScriptableObjectDependencyInjection
{
    [CreateAssetMenu(fileName = "DamageEventDataEvent", menuName = "ScriptableObjects/GameEvent/DamageEventData")]
    public class DamageDataEvent : ScriptableGameEvent<DamageData> { }
}