using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
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
            serieDataIndex.ForEach(index => chartTarget.series[index].ClearData());

            int numOfDatas = DateTime.Now.Hour;
            XAxis xAxis = chartTarget.GetChartComponent<XAxis>();

            if (numOfDatas >= 18)
            {
                xAxis.splitNumber =  Mathf.RoundToInt(numOfDatas * 0.5f);
            }
            
            List<string> xLabels = new List<string>();
            
            for (int i = 0; i < numOfDatas; i++)
            {
                // 設置X軸文字
                xLabels.Add(i.ToString("D2"));
                
                // 設置值
                float value = Random.Range(minValue, maxValue);
                float multiplier = Mathf.Pow(10f, afterDotNumber);
                serieDataIndex.ForEach(index =>
                {
                    chartTarget.series[index].AddData(Mathf.Round(value * multiplier) / multiplier);
                });
            }
            xAxis.data = xLabels;
            
            onGenerateDataEvent?.Invoke();
        }

        public void ValueUpdateByManual()
        {
            Debug.Log($"XChartAutoJumper: {name}");
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

        [Foldout("[SerieDataIndex集合]")] [SerializeField]
        private List<int> serieDataIndex = new List<int>(){0};

        [Foldout("[設定]")] [SerializeField] private bool isStartInEnabled = false;

        [Foldout("[設定]")] [Header("更新時間間隔")] [SerializeField]
        private float intervalSec = 10f;

        [Foldout("[設定]")] [Header("小數點後幾位")] [SerializeField]
        private int afterDotNumber = 1;

        [Foldout("[設定]")] [SerializeField] float minValue = 17f, maxValue = 23f;

        private Coroutine _coroutine;

        #endregion
    }
}