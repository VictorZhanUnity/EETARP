using System;
using NaughtyAttributes;
using UnityEngine;
using XCharts.Runtime;

namespace VictorDev.XChartUtils
{
    /// 控制播放XChart的動畫
    public class XChartAnimationController : MonoBehaviour
    {
        [Foldout("[設定]")] [SerializeField] private float duration = 0.7f;

        private void Start()
        {
            Chart.series.ForEach(s =>
            {
                s.animation.fadeIn.duration = duration * 1000;
                s.animation.fadeOut.duration = duration * 1000;
            });
        }

        private void OnValidate() => Start();

        private void OnEnable() => ToAnimationFadeIn();

        [Button]
        public void ToAnimationFadeIn() => Chart.AnimationFadeIn(true);

        private BaseChart Chart => _chart ??= GetComponent<BaseChart>();
        [NonSerialized] private BaseChart _chart;
    }
}