using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace VictorDev.Advanced
{
    /// UI拖曳功能
    public class UiDragController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Foldout("[Event]")] public UnityEvent beginDragEvent, endDragEvent;

        public void OnBeginDrag(PointerEventData eventData)
        {
            // 當開始拖拉時讓物件穿透 raycast（可選）
            if (canvasGroup != null)
                canvasGroup.blocksRaycasts = false;
            
            // 停止先前的動畫，避免拖拉時卡住
            _moveTween?.Kill();
            _targetPosition = bodyRectTransform.anchoredPosition;
            
            beginDragEvent?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (canvas == null) return;

            // 計算新的目標位置
            _targetPosition += eventData.delta / canvas.scaleFactor;

            // 限制位置不能超出畫面範圍
            _targetPosition = ClampToCanvas(_targetPosition);

            // 使用 DoTween 動畫過渡
            _moveTween = bodyRectTransform.DOAnchorPos(_targetPosition, 0.15f).SetEase(Ease.OutQuad);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // 拖拉結束時恢復互動
            if (canvasGroup != null)
                canvasGroup.blocksRaycasts = true;
            endDragEvent?.Invoke();
        }
        
        /// 限制 UI 的 anchoredPosition 在 Canvas 區域內
        private Vector2 ClampToCanvas(Vector2 pos)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Vector2 halfSize = bodyRectTransform.rect.size * 0.5f;

            // canvas 範圍（以 pivot 為中心）
            Vector2 min = -canvasRect.rect.size * 0.5f + halfSize;
            Vector2 max =  canvasRect.rect.size * 0.5f - halfSize;

            return new Vector2(
                Mathf.Clamp(pos.x, min.x+borderPadding.x, max.x-borderPadding.y),
                Mathf.Clamp(pos.y, min.y+borderPadding.w, max.y-borderPadding.z)
            );
        }
        #region Variables
        
        // 拖拉目標位置
        private Vector2 _targetPosition;
        private Tweener _moveTween;

        [Foldout("[設定]")] [SerializeField] private RectTransform bodyRectTransform;
        [Foldout("[設定]")] [SerializeField] private float duration = 0.15f;
        [Foldout("[設定]")] [SerializeField] private Ease easeType = Ease.OutQuad;
        [Foldout("[設定]")] [Label("[邊界Padding-左右上下]")] [SerializeField] private Vector4 borderPadding = new Vector4(0, 0, 0, 0);
        
        private Canvas canvas => _canvas ??= GetComponentInParent<Canvas>(true);
        [NonSerialized] private Canvas _canvas;
        
        private CanvasGroup canvasGroup => _canvasGroup ??= GetComponentInParent<CanvasGroup>(true);
        [NonSerialized] private CanvasGroup _canvasGroup;
        #endregion
    }
}