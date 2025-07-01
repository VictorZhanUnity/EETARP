using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace VictorDev.Advanced
{
    /// 處理IPointerTrigger事件，會擋住ScrollRect的滑動事件
    public class PointerEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Foldout("[Event] - OnPointerEvent(isEnter)")]
        public UnityEvent<bool> pointEvent;
        [Foldout("[Event] - OnPointerEnter")]
        public UnityEvent pointEnterEvent;
        [Foldout("[Event] - OnPointerExit")]
        public UnityEvent pointExitEvent;

        [Foldout("[設定]")] [SerializeField] private bool isActivatedInStart = true;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            pointEnterEvent?.Invoke();
            pointEvent?.Invoke(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            pointExitEvent?.Invoke();
            pointEvent?.Invoke(false);
        }

        private void Start()
        {
            if(isActivatedInStart) OnPointerExit(null);
        }
    }
}