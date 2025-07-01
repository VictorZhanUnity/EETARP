using System;
using TMPro;
using UnityEngine;
using VictorDev.DoTweenUtils;
using VictorDev.Revit;

namespace VictorDev.TCIT
{
    /// 顯示機櫃剩餘可用電力、負重、空間
    public class RackResourceDisplayer : MonoBehaviour, RackRevitInfoPage.IRackModelDataExtended, DeviceRevitInfoPage.IDeviceModelDataExtended
    {
        private RevitModelDataExtended _revitModelData;

        public void ReceiveRackModelData(RackModelDataExtended rackModelData)
        {
            _revitModelData = rackModelData;
            UpdateUI();
            
        }
        
        public void ReceiveDeviceModelData(DeviceModelDataExtended deviceModelData)
        {
            _revitModelData = deviceModelData;
            UpdateUI();
        }

        private void UpdateUI()
        {
            void ToBlink(TextMeshProUGUI target, string text)
            {
                DotweenHelper.ToBlink(target, text, 0.1f, 0.3f, true);
            }

            int watt = 0, weight= 0, heightU= 0;
            if (_revitModelData is RackModelDataExtended rackModelData)
            {
                watt = Mathf.RoundToInt(rackModelData.RemainOfWatt);
                weight = Mathf.RoundToInt(rackModelData.RemainOfWeight);
                heightU = Mathf.RoundToInt(rackModelData.RemainOfHeightU);
                
                ToBlink(TxtMaxWatt, $"Max:{rackModelData.information.watt_limit:N0}w");
                ToBlink(TxtMaxWeight, $"Max:{rackModelData.information.weight_limit:N0}kg");
                ToBlink(TxtMaxSpace, $"Max:{rackModelData.information.heightU:N0}u");
            }
            else if (_revitModelData is DeviceModelDataExtended deviceModelData)
            {
                watt = deviceModelData.information.watt;
                weight = deviceModelData.information.weight;
                heightU = deviceModelData.information.heightU;
            }
            ToBlink(TxtAvailableWatt, watt.ToString("N0"));
            ToBlink(TxtAvailableWeight, weight.ToString("N0"));
            ToBlink(TxtAvailableSpace, heightU.ToString("N0"));
        }
        
        #region Variables

        private TextMeshProUGUI TxtAvailableWatt => _txtAvailableWatt ??=
            transform.Find("Panel/Container/ItemAvailablePower/TxtValue").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtAvailableWeight => _txtAvailableWeight ??=
            transform.Find("Panel/Container/ItemAvailableWeight/TxtValue").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtAvailableSpace => _txtAvailableSpace ??=
            transform.Find("Panel/Container/ItemAvailableSpace/TxtValue").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtMaxWatt => _txtMaxWatt ??=
            transform.Find("Panel/Container/ItemAvailablePower/TxtMax").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtMaxWeight => _txtMaxWeight ??=
            transform.Find("Panel/Container/ItemAvailableWeight/TxtMax").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtMaxSpace => _txtMaxSpace ??=
            transform.Find("Panel/Container/ItemAvailableSpace/TxtMax").GetComponent<TextMeshProUGUI>();

        [NonSerialized] private TextMeshProUGUI _txtAvailableWatt,
            _txtAvailableWeight,
            _txtAvailableSpace,
            _txtMaxWatt,
            _txtMaxWeight,
            _txtMaxSpace;

        #endregion

       
    }
}