using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace VictorDev.Common
{
    /// 跟隨鼠標移動(3D)
    public class FollowMouse3D : MonoBehaviour
    {
        void Update()
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, layerMask))
            {
                Vector3 targetPosition = hit.point + offsetPos;
                targetPosition.y += RendererInstance.bounds.size.y;

                // 如果已有 Tween，先終止
                if (_moveTween != null && _moveTween.IsActive())
                {
                    _moveTween.Kill();
                }

                // 用 DOTween 平滑移動
                _moveTween = transform.DOMove(targetPosition, duration).SetEase(ease);
                _moveTween = transform.DOMove(targetPosition, duration).SetEase(ease);
            }
        }

        [Foldout("[碰撞設定]")] [SerializeField] private float rayDistance = 1000f;
        [Foldout("[碰撞設定]")] [SerializeField] private LayerMask layerMask = ~0;
        [Foldout("[位置設定]")] [SerializeField] private Vector3 offsetPos = Vector3.zero;

        [Foldout("[跟隨Ease設定]")] [SerializeField]
        private float duration = 0f;

        [Foldout("[跟隨Ease設定]")] [SerializeField]
        private Ease ease = Ease.OutQuad;

        private Tween _moveTween;

        private Renderer RendererInstance => _renderer ??= GetComponent<Renderer>();
        [NonSerialized] private Renderer _renderer;

        private Camera MainCamera => _mainCamera ??= Camera.main;
        [NonSerialized] private Camera _mainCamera;
    }
}