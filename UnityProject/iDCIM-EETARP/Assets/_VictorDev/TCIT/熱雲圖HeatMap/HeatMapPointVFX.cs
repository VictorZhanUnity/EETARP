using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace VictorDev.HeatMapUtiils
{
//熱力圖點位資訊
    public class HeatMapPointVFX : MonoBehaviour
    {
        /// 設置在範圍內的雲物件HeatMapItem，每次設置皆會重置先前儲存的HeatMapItem
        public void SetHeatMapItemInRange(List<Transform> targets)
        {
            ResetHeatMapItemValue();
            _heatMapItemInRange = targets.OrderBy(target => Vector3.Distance(target.position, transform.position))
                .ToList();
            UpdateHeatMapItem();
            
            Debug.Log($"目標點位:{_heatMapItemInRange[0]}");
        }
        
        /// 設定雲物件的點位值/權重值
        private void UpdateHeatMapItem(bool isReset = false)
        {
            _heatMapItemInRange.ForEach(heatMapItem =>
            {
                IHeatMapItem targetPointItem = heatMapItem.GetComponent<IHeatMapItem>();
                if (isReset) targetPointItem.SetValue(0);
                if (heatMapItem == _heatMapItemInRange.First())
                {
                    targetPointItem.SetValue(value); //設定值給最靠近的雲物件
                    targetPointItem.IsHeatMapPoint = true;
                }
                else
                {
                   // targetPointItem.SetWeightValue(value,Vector3.Distance(transform.position, heatMapItem.position)); //以距離為權重值
                }
            });
        }

        /// 重設熱力圖FogItem的Value權重為0
        public void ResetHeatMapItemValue()
            => _heatMapItemInRange?.Select(target => target.GetComponent<IHeatMapItem>()).ToList()
                .ForEach(target => target.SetValue(0));

        private void OnValidate() => Txt.SetText(value.ToString());

        #region Variables

        [Range(0, 100)] [SerializeField] int value;

        /// 點位值
        public int Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValidate();
                UpdateHeatMapItem(true);
            }
        }

        /// 在範圍內的雲物件HeatMapItem
        [NonSerialized] private List<Transform> _heatMapItemInRange = new();

        private TextMeshPro Txt => _txt ??= transform.GetComponentInChildren<TextMeshPro>();
        [NonSerialized]private TextMeshPro _txt;

        #endregion
    }
}