using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VictorDev.Advanced
{
    /// Image FillAmount事件處理
    public class ImageFillAmountInvoker : MonoBehaviour
    {
        [Foldout("[Event] - fillAmount為1時")] public UnityEvent invokeOnFillAmount1;
        [Foldout("[Event] - fillAmount為0時")] public UnityEvent invokeOnFillAmount0;

        [Foldout("[Event] - 當fillAmount不為01時")]
        public UnityEvent invokeOnFillAmountNot01;

        [Foldout("[設定] - Delay發送Event")] [SerializeField]
        private float delaySecInvokeFillAmount1 = 1f;

        [Foldout("[Event] - 發送invokeFillAmount值")]
        public UnityEvent<float> invokeFillAmount;

        private float _preFillAmount = -1f;
        private bool _isNear1, _isNear0;

        private void Start()
        {
            image ??= GetComponent<Image>();
            Update();
        }

        private void Update()
        {
            if (!Mathf.Approximately(_preFillAmount, image.fillAmount))
            {
                _isNear1 = Mathf.Approximately(image.fillAmount, 1);
                _isNear0 = Mathf.Approximately(image.fillAmount, 0);

                if (_isNear1) InvokeEvent(invokeOnFillAmount1);
                else if (_isNear0) invokeOnFillAmount0?.Invoke();

                if (_isNear1 == false && _isNear0 == false)
                {
                    invokeOnFillAmountNot01?.Invoke();
                }

                invokeFillAmount?.Invoke(image.fillAmount);

                _preFillAmount = image.fillAmount;
            }
        }

        private void InvokeEvent(UnityEvent unityEvent)
        {
            StartCoroutine(Func());

            IEnumerator Func()
            {
                yield return new WaitForSeconds(delaySecInvokeFillAmount1);
                unityEvent?.Invoke();
            }
        }

        [SerializeField] private Image image;
    }
}