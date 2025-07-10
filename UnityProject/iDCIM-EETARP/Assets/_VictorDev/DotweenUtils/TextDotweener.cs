using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace VictorDev.DoTweenUtils
{
    public class TextDotweener : MonoBehaviour
    {
        public void SetText(string text)
        {
            text = text.Trim();
            DotweenHelper.ToBlink(Txt, text, duration, delay, isRandomDelay);
        }

        [Foldout("設定")] [SerializeField] private float duration = 0.1f;
        [Foldout("設定")] [SerializeField] private float delay = 0.3f;
        [Foldout("設定")] [SerializeField] private bool isRandomDelay = true;
        
        private TextMeshProUGUI Txt => _txt ??= GetComponent<TextMeshProUGUI>();
        [NonSerialized] private TextMeshProUGUI _txt;
    }
}