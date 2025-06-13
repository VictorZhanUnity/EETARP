using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.MathUtils;

namespace VictorDev.Advanced
{
    /// [Mediator] - 數值轉接器
    public class ValueMediator : MonoBehaviour
    {
        public void SetValue(float value)
        {
            _value = value;
            InvokeValueHandler();
        }

        public void SetValue01(float value) => SetValue(value * 100f);

        private void InvokeValueHandler()
        {
            invokeString?.Invoke(MathHelper.ToDotNumberString(_value, dotNumber));
            invokeFloat?.Invoke(MathHelper.ToDotNumberFloat(_value, dotNumber));
            invokeFloat01?.Invoke(MathHelper.ToPercent01(_value, maxValue, dotNumber));
            invokeInteger?.Invoke(Mathf.RoundToInt(_value));
        }
        
        #region Variabls
        [Foldout("發送字串")] public UnityEvent<string> invokeString;
        [Foldout("發送float")] public UnityEvent<float> invokeFloat;
        [Foldout("發送float01")] public UnityEvent<float> invokeFloat01;
        [Foldout("發送Integer")] public UnityEvent<float> invokeInteger;

        [Foldout("[設定]")] [SerializeField] private int dotNumber;
        [Foldout("[設定]")] [SerializeField] private int maxValue;
        private float _value;
        #endregion
    }
}