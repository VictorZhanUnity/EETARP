using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.MathUtils;
using XCharts.Runtime;
using Random = UnityEngine.Random;

namespace VictorDev.DemoUtils
{
    public class XChartAutoJumper : MonoBehaviour, IAutoJumper
    {
        [Foldout("[Event] - 套用資料時Invoke")] public UnityEvent onGenerateDataEvent;

        private void OnEnable()
        {
            if (isStartInEnabled) StartJump();
        }

        [Button]
        public void StartJump()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            if (Application.isPlaying)
            {
                IEnumerator JumpValue()
                {
                    while (true)
                    {
                        ValueUpdate();
                        yield return new WaitForSeconds(intervalSec);
                    }
                }

                _coroutine = StartCoroutine(JumpValue());
            }
            else
            {
                ValueUpdate();
            }
        }

        /// 設置Value
        private void ValueUpdate()
        {
            serieDataSetting.ForEach(setting => chartTarget.series[setting.seriesIndex].ClearData());

            // 更改X軸顯示數量
            int numOfDatas = DateTime.Now.Hour;
            if (numOfDatas >= 18)
            {
                xAxis.splitNumber = Mathf.RoundToInt(numOfDatas * 0.5f);
            }

            float yAxisMax = 0;
            List<string> xLabels = new List<string>();
            float rndValue, value;
            
            for (int i = 0; i < numOfDatas; i++)
            {
                // 設置X軸文字
                xLabels.Add(i.ToString("D2"));

                serieDataSetting.ForEach(setting =>
                {
                    // 設置值
                    float multiplier = Mathf.Pow(10f, setting.afterDotNumber);
                    rndValue = Random.Range(setting.minValue, setting.maxValue);
                    value = Mathf.Round(rndValue * multiplier) / multiplier;
                    chartTarget.series[setting.seriesIndex].AddData(value);

                    yAxisMax = Mathf.Max(yAxisMax, value);
                });
            }

            xAxis.data = xLabels;
            yAxis.max = MathHelper.GetNumberLevelMax(yAxisMax, 5);

            onGenerateDataEvent?.Invoke();
        }

        public void ValueUpdateByManual()
        {
            ValueUpdate();
            if (isStartInEnabled) StartJump();
        }

        private void OnDisable()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
        }

        #region Variables

        [Foldout("[XChart對像]")] [SerializeField]
        private BaseChart chartTarget;

        [Label("[SeriesDataIndex集合]")] [SerializeField]
        private List<SeriesSetting> serieDataSetting;

        [Foldout("[設定]")] [SerializeField] private bool isStartInEnabled = false;

        [Foldout("[設定]")] [Header("更新時間間隔")] [SerializeField]
        private float intervalSec = 10f;

        private Coroutine _coroutine;

        private XAxis xAxis => _xAxis ??= chartTarget.GetChartComponent<XAxis>();
        [NonSerialized] private XAxis _xAxis;
        private YAxis yAxis => _yAxis ??= chartTarget.GetChartComponent<YAxis>();
        [NonSerialized] private YAxis _yAxis;

        #endregion

        [Serializable]
        public class SeriesSetting
        {
            [Header("SeriesIndex")] public int seriesIndex;
            [Header("小數點後幾位")] public int afterDotNumber = 1;
            public float minValue = 17f, maxValue = 23f;
        }
    }
}