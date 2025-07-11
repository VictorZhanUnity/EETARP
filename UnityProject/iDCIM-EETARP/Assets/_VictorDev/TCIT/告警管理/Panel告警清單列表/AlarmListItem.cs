using System;
using _VictorDEV.DateTimeUtils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.DoTweenUtils;

namespace VictorDev.TCIT.AlarmModule
{
    /// 告警列表資料項目
    public class AlarmListItem : MonoBehaviour, AlarmDataManager.IAlarmData
    {
        [HideInInspector] public UnityEvent<AlarmListItem> onItemSelected;
        public AlarmData alarmData { get; private set; }

        public void ReceiveData(AlarmData data)
        {
            alarmData = data;
            UpdateUI();
        }

        private void UpdateUI()
        {
            ToggleAlarmType.isOn = alarmData.alarmType == AlarmData.AlarmType.Critical;
            TxtSystem.SetText(alarmData.alarmSystem.ToString());
            TxtTime.SetText(alarmData.alarmTime.ToString("hh:mm:SS tt", System.Globalization.CultureInfo.CreateSpecificCulture("en-US")));
        }

        #region Initialized
        private void OnEnable() => ToggleInstance.onValueChanged.AddListener(OnValueChangedHandler);
        private void OnDisable()
        {
            ToggleInstance.onValueChanged.RemoveListener(OnValueChangedHandler);
        }

        private void OnValueChangedHandler(bool isOn)
        {
            if (isOn) onItemSelected?.Invoke(this);
        }
        #endregion

       
        #region Variables
       
        public ToggleGroup toggleGroup
        {
            set => ToggleInstance.group = value;
        }

        public bool IsOn
        {
            set => ToggleInstance.isOn = value;
        }
        
        private Toggle ToggleInstance => _toggle ??= transform.Find("Container").GetComponent<Toggle>();
        private Toggle ToggleAlarmType => _toggleAlarmType ??= transform.Find("Container/ToggleAlarmType").GetComponent<Toggle>();
        [NonSerialized] private Toggle _toggle, _toggleAlarmType;
        private TextDotweener TxtSystem => _txtSystem ??= transform.Find("Container/TxtSystem").GetComponent<TextDotweener>();
        private TextDotweener TxtTime => _txtTime ??= transform.Find("Container/TxtTime").GetComponent<TextDotweener>();
        [NonSerialized] private TextDotweener _txtSystem, _txtTime;
        #endregion
       

    }
}