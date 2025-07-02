using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VictorDev.TCIT.AlarmModule
{
    public class AlarmListItem : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<bool> onValueChanged;
        private void OnEnable() => ToggleInstance.onValueChanged.AddListener(OnValueChangedHandler);
        private void OnDisable() => ToggleInstance.onValueChanged.RemoveListener(OnValueChangedHandler);
        private void OnValueChangedHandler(bool isOn)
        {
            onValueChanged?.Invoke(isOn);
        }

        public ToggleGroup toggleGroup
        {
            set => ToggleInstance.group = value;
        }
        
        private Toggle ToggleInstance => _toggle ??= transform.Find("Container").GetComponent<Toggle>();
        [NonSerialized] private Toggle _toggle;

        private Toggle AlarmType => _alarmType ??= transform.Find("Container/ToggleAlarmType").GetComponent<Toggle>();
        [NonSerialized] private Toggle _alarmType;
    }
}