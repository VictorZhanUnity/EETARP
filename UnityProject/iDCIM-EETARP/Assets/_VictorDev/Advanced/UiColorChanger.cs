using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VictorDev.Advanced
{
    public class UiColorChanger : MonoBehaviour
    {
        [Foldout("[設定]")] [SerializeField] private List<Color> colors;
        [Foldout("[設定]")] [Header("[可選] - Toggle的IsOn判斷")][SerializeField] private Toggle toggleTarget;
        [Foldout("[目標對像：Text]")] [SerializeField] private List<TextMeshProUGUI> textTargets;
        [Foldout("[目標對像：Image]")] [SerializeField] private List<Image> imageTargets;

        public void ChangeColor(int index)
        {
            if (toggleTarget != null && toggleTarget.isOn) return;
            textTargets.ForEach(txt => txt.color = colors[index]);
            imageTargets.ForEach(img => img.color = colors[index]);
        }
    }
}