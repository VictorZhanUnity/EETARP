using System;
using TMPro;
using UnityEngine;
using VictorDev.DoTweenUtils;
using VictorDev.Revit;

namespace VictorDev.TCIT
{
    /// 顯示機櫃剩餘可用電力、負重、空間
    public class RackResourceDisplayer : MonoBehaviour, RackRevitInfoPage.IRackModelDataExtended
    {
        private RackModelDataExtended _rackModelData;

        public void ReceiveRackModelData(RackModelDataExtended rackModelData)
        {
            _rackModelData = rackModelData;
            UpdateUI();
        }

        private void UpdateUI()
        {
            void ToBlink(TextMeshProUGUI target, string text)
            {
                DotweenHelper.ToBlink(target, text, 0.1f, 0.3f, true);
            }

            ToBlink(TxtAvailableWatt, _rackModelData.RemainOfWatt.ToString("N0"));
            ToBlink(TxtAvailableWeight, _rackModelData.RemainOfWeight.ToString("N0"));
            ToBlink(TxtAvailableSpace, _rackModelData.RemainOfHeightU.ToString("N0"));

            ToBlink(TxtMaxWatt, $"Max:{_rackModelData.information.watt_limit:N0}w");
            ToBlink(TxtMaxWeight, $"Max:{_rackModelData.information.weight_limit:N0}kg");
            ToBlink(TxtMaxSpace, $"Max:{_rackModelData.information.heightU:N0}u");
        }

        #region Variables

        private TextMeshProUGUI TxtAvailableWatt => _txtAvailableWatt ??=
            transform.Find("Panel/Container/ItemAvaiablePower/TxtValue").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtAvailableWeight => _txtAvailableWeight ??=
            transform.Find("Panel/Container/ItemAvaiableWeight/TxtValue").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtAvailableSpace => _txtAvailableSpace ??=
            transform.Find("Panel/Container/ItemAvaiableSpace/TxtValue").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtMaxWatt => _txtMaxWatt ??=
            transform.Find("Panel/Container/ItemAvaiablePower/TxtMax").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtMaxWeight => _txtMaxWeight ??=
            transform.Find("Panel/Container/ItemAvaiableWeight/TxtMax").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtMaxSpace => _txtMaxSpace ??=
            transform.Find("Panel/Container/ItemAvaiableSpace/TxtMax").GetComponent<TextMeshProUGUI>();

        [NonSerialized] private TextMeshProUGUI _txtAvailableWatt,
            _txtAvailableWeight,
            _txtAvailableSpace,
            _txtMaxWatt,
            _txtMaxWeight,
            _txtMaxSpace;

        #endregion
    }
}