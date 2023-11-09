using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectDependencyInjection
{
    public abstract class GameEventListener<T> : MonoBehaviour
    {
        public ScriptableGameEvent<T> Event;
        public UnityEvent<T> Response;

        private void OnEnable() {
            Event.RegisterListener(this);
        }

        private void OnDisable() {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(T eventData) {
            Response.Invoke(eventData);
        }
    }
}