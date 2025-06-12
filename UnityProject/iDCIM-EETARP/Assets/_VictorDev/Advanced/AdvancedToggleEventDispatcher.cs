using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VictorDev.Advanced
{
    /// 在Inspector裡進階處理Toggle.OnValueChange事件
    /// <para>+ 直接掛在GameObject上即可</para>
    public class AdvancedToggleEventDispatcher : MonoBehaviour
    {
        private void Awake()
        {
            ToggleInstance.onValueChanged.AddListener(
                (isOn) =>
                {
                    if (isOn) onValueToTrue?.Invoke();
                    else onValueToFalse?.Invoke();

                    onValueToReverse?.Invoke(!isOn);
                });
        }

        private void Start()
        {
            if (isInvokeInStart) ToggleInstance.onValueChanged.Invoke(ToggleInstance.isOn);
        }

        #region Variables

        [Header(">>> Toggle值反向Invoke")] public UnityEvent<bool> onValueToReverse;

        [Foldout("[Event] - On/Off事件個別設定")] [Header(">>> 當Toggle值為True時")]
        public UnityEvent onValueToTrue;

        [Foldout("[Event] - On/Off事件個別設定")] [Header(">>> 當Toggle值為False時")]
        public UnityEvent onValueToFalse;

        [Foldout(">>> Awake時自動Invoke")] public bool isInvokeInStart = true;

        private Toggle ToggleInstance => _toggle ??= GetComponent<Toggle>();
        [NonSerialized] private Toggle _toggle;

        #endregion
    }
}