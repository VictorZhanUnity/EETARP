using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.DoTweenUtils;
using VictorDev.Revit;

namespace VictorDev.TCIT
{
    /// 機櫃基本資訊
    public class RackBasicInfoDisplayer : MonoBehaviour, RackRevitInfoPage.IRackModelDataExtended
    {

        public void ReceiveRackModelData(RackModelDataExtended rackModelData)
        {
            _rackModelDataExtended = rackModelData;
            UpdateUI();
        }

        private void UpdateUI()
        {
            void ToBlink(TextMeshProUGUI target, string text) => DotweenHelper.ToBlink(target, text, 0.1f, 0.3f, true);
            DotweenHelper.ToBlink(TxtDeviceName, _rackModelDataExtended.DeviceName);
            ToBlink(TxtDeviceType, _rackModelDataExtended.DeviceType);
            ToBlink(TxtFloor, _rackModelDataExtended.Floor);
            ToBlink(TxtRoom, _rackModelDataExtended.Room);
            Icon.sprite = _rackModelDataExtended.ModelAssetIcon;
        }

        #region Variables
        private RackModelDataExtended _rackModelDataExtended;
        
        private TextMeshProUGUI TxtDeviceName =>
            _txtDeviceName ??= transform.Find("Panel/Container/TxtDeviceName").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtDeviceType =>
            _txtDeviceType ??= transform.Find("Panel/Container/TxtDeviceType").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtFloor =>
            _txtFloor ??= transform.Find("Panel/Container/TxtFloor").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtRoom =>
            _txtRoom ??= transform.Find("Panel/Container/TxtRoom").GetComponent<TextMeshProUGUI>();

        [NonSerialized] private TextMeshProUGUI _txtDeviceName, _txtDeviceType, _txtFloor, _txtRoom;

        
        private Image Icon =>
            _icon ??= TxtDeviceName.transform.Find("ICON").GetComponent<Image>();
        [NonSerialized] private Image _icon;

        #endregion
    }
}