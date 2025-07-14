using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VictorDev.Advanced
{
    public class SliderValueInvoker : MonoBehaviour
    {
        [Foldout("[Event] - Invoke字串型態")] public UnityEvent<string> onValueChangedString;
        [Foldout("[Event] - Invoke數字型態")] public UnityEvent<float> onValueChangedFloat;

        private void OnEnable()
        {
            SliderInstance.onValueChanged.AddListener(OnValueChanged);
            SliderInstance.onValueChanged.Invoke(SliderInstance.value);
        }

        private void OnDisable() => SliderInstance.onValueChanged.RemoveListener(OnValueChanged);

        private void OnValueChanged(float value)
        {
            float multiplier = Mathf.Pow(10f, afterDotNumber);
            string dotFormat = (afterDotNumber > 0) ? "." + new string('#', afterDotNumber) : "";
            onValueChangedFloat?.Invoke(Mathf.Round(value * multiplier) / multiplier);
            onValueChangedString?.Invoke(value.ToString($"0{dotFormat}"));
        }

        #region Variables

        [Foldout("[設定]")] [Header("小數點後幾位")] [SerializeField]
        private int afterDotNumber = 2;

        private Slider SliderInstance => _sliderInstance ??= GetComponent<Slider>();
        [NonSerialized] private Slider _sliderInstance;

        #endregion
    }
}