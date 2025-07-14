using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using VictorDev.DoTweenUtils;
using VictorDev.TCIT.AlarmModule;
using XCharts.Runtime;

public class AlarmPieChartHandler : MonoBehaviour, AlarmDataManager.IAlarmDataList
{
   private List<AlarmData> _alarmData;

    public void ReceiveData(List<AlarmData> data)
    {
        _alarmData = data;
        UpdatePieChart();
        UpdateLineChart();
    }

    private void UpdateLineChart()
    {
        var criticalList = _alarmData.Where(alarm=> alarm.alarmType == AlarmData.AlarmType.Critical).ToList();
        var warningList = _alarmData.Where(alarm=> alarm.alarmType == AlarmData.AlarmType.Warning).ToList();
    }

    private void UpdatePieChart()
    {
        int ACMV = _alarmData.Count(data => data.alarmSystem == AlarmData.AlarmSystem.ACMV);
        int EE = _alarmData.Count(data => data.alarmSystem == AlarmData.AlarmSystem.EE);
        int FS = _alarmData.Count(data => data.alarmSystem == AlarmData.AlarmSystem.FS);
        int PS = _alarmData.Count(data => data.alarmSystem == AlarmData.AlarmSystem.PS);
        int WE = _alarmData.Count(data => data.alarmSystem == AlarmData.AlarmSystem.WE);
        
        txtACMV.SetText(ACMV.ToString());
        txtEE.SetText(EE.ToString());
        txtFS.SetText(FS.ToString());
        txtPS.SetText(PS.ToString());
        txtWE.SetText(WE.ToString());
        
        pieChart.series[0].ClearData();
        pieChart.series[0].AddData(new List<double>(){ACMV}, "ACMV");
        pieChart.series[0].AddData(new List<double>(){EE}, "EE");
        pieChart.series[0].AddData(new List<double>(){FS}, "FS");
        pieChart.series[0].AddData(new List<double>(){PS}, "PS");
        pieChart.series[0].AddData(new List<double>(){WE}, "WE");
    }

    [Foldout("[設定] - PieChart")] [SerializeField]
    private TextDotweener txtACMV, txtEE, txtFS, txtPS, txtWE;
    [Foldout("[設定] - PieChart")] [SerializeField]
    private PieChart pieChart;
    
    [Foldout("[設定] - LineChart")] [SerializeField]
    private LineChart lineChart;
}
