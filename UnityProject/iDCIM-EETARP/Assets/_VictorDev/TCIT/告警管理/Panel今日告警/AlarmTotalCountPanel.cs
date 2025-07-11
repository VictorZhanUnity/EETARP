using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using VictorDev.DoTweenUtils;
using VictorDev.TCIT.AlarmModule;

public class AlarmTotalCountPanel : MonoBehaviour, AlarmDataManager.IAlarmDataList
{
    private List<AlarmData> _alarmData;
    
    public void ReceiveData(List<AlarmData> data)
    {
        _alarmData = data;
        UpdateUI();
    }

    private void UpdateUI()
    {
        TxtTotalCount.SetText(_alarmData.Count.ToString());
        TxtCriticalCount.SetText(_alarmData.Where(alarm=> alarm.alarmType == AlarmData.AlarmType.Critical).ToList().Count.ToString());
        TxtWarningCount.SetText(_alarmData.Where(alarm=> alarm.alarmType == AlarmData.AlarmType.Warning).ToList().Count.ToString());
    }

    #region Variables
    private TextDotweener TxtTotalCount => _txtTotalCount ??= transform.Find("Panel/Container/TxtTotalAlarm").GetComponent<TextDotweener>();
    private TextDotweener TxtCriticalCount => _txtCriticalCount ??= transform.Find("Panel/Container/ButtonCritical/TxtCritical").GetComponent<TextDotweener>();
    private TextDotweener TxtWarningCount => _txtWarningCount ??= transform.Find("Panel/Container/ButtonWarning/TxtWarning").GetComponent<TextDotweener>();
    [NonSerialized] private TextDotweener _txtTotalCount, _txtCriticalCount, _txtWarningCount;
    #endregion
}
