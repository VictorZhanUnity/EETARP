using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace VictorDev.Advanced
{
    public class LifeCycleEventDispatcher : MonoBehaviour
    {
        [Foldout("[Event] - Enable/Disabled")]
        public UnityEvent<bool> onEnableDisabledEvent;

        [Foldout("[Event] - Enable/Disabled")]
        public UnityEvent onEnableEvent, onDisableEvent;

        private void OnEnable()
        {
            onEnableEvent?.Invoke();
            onEnableDisabledEvent?.Invoke(true);
        }

        private void OnDisable()
        {
            onDisableEvent?.Invoke();
            onEnableDisabledEvent?.Invoke(false);
        }


        [Foldout("[Event] - Start")] public UnityEvent onStartEvent;
        private void Start() => onStartEvent?.Invoke();
    }
}