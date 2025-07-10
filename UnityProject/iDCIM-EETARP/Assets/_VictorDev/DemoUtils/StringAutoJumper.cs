using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Debug = VictorDev.Common.Debug;

namespace VictorDev.DemoUtils
{
    public class StringAutoJumper : MonoBehaviour, IAutoJumper
    {
        [Foldout("[Event] - Invoke字串")] public UnityEvent<string> onValueChangedString;

        [Foldout("[設定]")] [SerializeField] private bool isUseSoData;
        public bool isUseCustomString => !isUseSoData;

        [ShowIf(nameof(isUseSoData))] [SerializeField]
        private DemoStringData demoStringData;

        [ShowIf(nameof(isUseCustomString))] [SerializeField]
        private List<string> stringValues;

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
            string result = string.Empty;
            if (isUseSoData && demoStringData != null)
            {
                result = demoStringData.GetRandomStringValue();
            }
            else
            {
                if (stringValues.Count == 0)
                {
                    Debug.LogWarning($"stringValues haven't been set", this, EmojiEnum.Error);
                }
                else
                {
                    result = stringValues[Random.Range(0, stringValues.Count)];
                }
            }
            onValueChangedString?.Invoke(result);
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

        [Foldout("[設定]")] [SerializeField] private bool isStartInEnabled = true;

        [Foldout("[設定]")] [Header("更新時間間隔")] [SerializeField]
        private float intervalSec = 10f;

        private Coroutine _coroutine;

        #endregion
    }
}