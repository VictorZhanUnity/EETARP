using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.DoTweenUtils;

namespace VictorDev.TCIT.AlarmModule
{
    public class AlarmDetailInfoWindow : MonoBehaviour, AlarmDataManager.IAlarmListItem
    {
        [Foldout("[Event] - Invoke目標模型")] public UnityEvent<GameObject> invokeTargetMode;
        
        public void ReceiveData(AlarmListItem item)
        {
            _alarmListItem = item;
            UpdateUI();
            LookAtTargetMode();
        }

        private void UpdateUI()
        {
            if(gameObject.activeInHierarchy) DotweenFade2D.ToShow();
            else gameObject.SetActive(true);
            
            AlarmType.isOn = _alarmListItem.alarmData.alarmType == AlarmData.AlarmType.Critical;
            TxtAlarmSystem.SetText(_alarmListItem.alarmData.alarmSystem.ToString());
            TxtOrigin.SetText(_alarmListItem.alarmData.OriginDeviceName);
            TxtTime.SetText(_alarmListItem.alarmData.alarmTime.ToString("hh:mm:ss tt yyyy/MM/dd", System.Globalization.CultureInfo.CreateSpecificCulture("en-US")));
            TxtStatus.SetText(_alarmListItem.alarmData.status.ToString());
            InputNote.text = _alarmListItem.alarmData.note;
        }

        public void LookAtTargetMode()
        {
            invokeTargetMode?.Invoke(_alarmListItem.alarmData.alarmTargetModel.gameObject);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            if(_alarmListItem != null) _alarmListItem.IsOn = false;
        }

        #region Variabls

        private AlarmListItem _alarmListItem;
        
        private Toggle AlarmType => _alarmType ??= transform.Find("Panel/Container/AlarmType").GetComponent<Toggle>();
        [NonSerialized] private Toggle _alarmType;
        
        private TextDotweener TxtAlarmSystem => _txtAlarmSystem ??= transform.Find("Panel/Container/TxtAlarmSystem").GetComponent<TextDotweener>();
        private TextDotweener TxtOrigin => _txtOrigin ??= transform.Find("Panel/Container/TxtOrigin").GetComponent<TextDotweener>();
        private TextDotweener TxtTime => _txtTime ??= transform.Find("Panel/Container/TxtTime").GetComponent<TextDotweener>();
        private TextDotweener TxtStatus => _txtStatus ??= transform.Find("Panel/Container/TxtStatus").GetComponent<TextDotweener>();
        [NonSerialized] private TextDotweener _txtAlarmSystem, _txtOrigin, _txtTime, _txtStatus;
        
        private TMP_InputField InputNote => _inputNote ??= transform.Find("Panel/Container/Note/InputNote").GetComponent<TMP_InputField>();
        [NonSerialized] private TMP_InputField _inputNote;
        
        private DotweenFade2DWithEnabled DotweenFade2D => _dotweenFade2D ??= transform.Find("Panel").GetComponent<DotweenFade2DWithEnabled>();
        [NonSerialized] private DotweenFade2DWithEnabled _dotweenFade2D;
        #endregion
    }
}