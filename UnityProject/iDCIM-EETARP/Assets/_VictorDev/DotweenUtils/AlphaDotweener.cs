using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace VictorDev.DoTweenUtils
{
    public class AlphaDotweener : MonoBehaviour
    {
        public void ToAlpha(float alpha)
        {
            _tweener?.Kill();
            _tweener = canvasGroup.DOFade(alpha, duration).SetEase(easeType);
        }

        private Tweener _tweener;

        [Foldout("[設定]")] [SerializeField] private float duration = 0.15f;
        [Foldout("[設定]")] [SerializeField] private Ease easeType = Ease.OutQuad;

        private CanvasGroup canvasGroup => _canvasGroup ??= GetComponentInParent<CanvasGroup>(true);
        [NonSerialized] private CanvasGroup _canvasGroup;
    }
}