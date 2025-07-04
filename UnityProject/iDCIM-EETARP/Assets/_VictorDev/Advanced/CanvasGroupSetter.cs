using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace VictorDev.Advanced
{
    /// 設定CanvasGroup的alpha / isOn
    public class CanvasGroupSetter : MonoBehaviour
    {
        public float alpha
        {
            set
            {
                DOTween.Kill(canvasGroup);
                canvasGroup.DOFade(value, duration).SetDelay(delay).SetEase(ease);
                canvasGroup.interactable = canvasGroup.blocksRaycasts = Mathf.Approximately(value, 1);
            }
        }

        public bool isOn
        {
            set => alpha = value ? 1 : falseValue;
        }

        private void Awake() => canvasGroup ??= GetComponent<CanvasGroup>();

        [Foldout("[設定]")] [SerializeField] private float falseValue = 0.3f, duration = 0.15f, delay = 0;
        [Foldout("[設定]")] [SerializeField] private Ease ease = Ease.OutQuad;

        [Foldout("[設定]"), Header("可選")] [SerializeField]
        private CanvasGroup canvasGroup;
    }
}