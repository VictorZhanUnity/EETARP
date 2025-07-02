using System;
using UnityEngine;
using UnityEngine.UI;

namespace VictorDev.Advanced
{
    public class ButtonClickInvoker : MonoBehaviour
    {
        public void InvokeClickedEvent() => Btn.onClick.Invoke();

        private Button Btn => _btn ??= GetComponent<Button>();
        [NonSerialized] private Button _btn;
    }
}