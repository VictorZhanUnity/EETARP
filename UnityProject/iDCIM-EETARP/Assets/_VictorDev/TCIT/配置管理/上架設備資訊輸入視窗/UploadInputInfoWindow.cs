using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Revit;

namespace VictorDev.TCIT.StorageAssetUtils
{
    /// 設備上架資訊輸入視窗
    public class UploadInputInfoWindow : MonoBehaviour
    {
        [Foldout("[Event] - 上架設備成功時Invoke")]
        public UnityEvent<DeviceModelDataExtended> onUploadSuccess;

        public void ReceiveData(DeviceModelDataExtended device, RackModelDataExtended rack, List<int> rackUnits)
        {
            _uploadDevice = device;
            _toRack = rack;
            _occupyRackUnits = rackUnits;

            UpdateUI();
        }

        private void UpdateUI()
        {
            TxtOccupyRackUnits.SetText($"U{_occupyRackUnits.Min()} ~ U{_occupyRackUnits.Max()}");
            TxtDeviceType.SetText(_uploadDevice.DeviceType);
            TxtWatt.SetText(_uploadDevice.information.watt.ToString());
            TxtWeight.SetText(_uploadDevice.information.weight.ToString());
            TxtRackUnit.SetText(_uploadDevice.information.heightU.ToString());
            gameObject.SetActive(true);
        }

        /// 進行上架，儲存至WebAPI
        public void UploadDevice()
        {
            //WebAPI記錄 ... 
            OnUploadDeviceSuccess();
        }

        private void OnUploadDeviceSuccess()
        {
            List<RackUnitDisplay> needToRemove = _toRack.AvailableUDisplayer.Where(rackUnitFree=> _occupyRackUnits.Contains(rackUnitFree.ULevel)).ToList();
            needToRemove.ForEach(target=>
            {
                target.IsPinULocationVisible = false;
                target.HideULevel();
            });
            _toRack.AvailableUDisplayer = _toRack.AvailableUDisplayer.Except(needToRemove).ToList();
            onUploadSuccess?.Invoke(_uploadDevice);
            CancelUpload();
        }
        
        /// 取消上架
        public void CancelUpload()
        {
            gameObject.SetActive(false);
            InputDeviceName.text = string.Empty;
            InputIpAddress.text = string.Empty;
            InputNote.text = string.Empty;

            _uploadDevice = null;
            _toRack = null;
            _occupyRackUnits = null;
        }
        
        
        #region Variables

        [NonSerialized] private DeviceModelDataExtended _uploadDevice;
        [NonSerialized] private RackModelDataExtended _toRack;
        [NonSerialized] private List<int> _occupyRackUnits;

        
        
        private TMP_InputField InputDeviceName => _inputDeviceName ??= GetComponent<TMP_InputField>();
        private TMP_InputField InputIpAddress => _inputIpAddress ??= GetComponent<TMP_InputField>();
        private TMP_InputField InputNote => _inputNote ??= GetComponent<TMP_InputField>();
        [NonSerialized] private TMP_InputField _inputDeviceName, _inputIpAddress, _inputNote;
        
        private TextMeshProUGUI TxtDeviceType => _txtDeviceType ??= GetComponent<TextMeshProUGUI>();
        private TextMeshProUGUI TxtWatt => _txtWatt ??= GetComponent<TextMeshProUGUI>();
        private TextMeshProUGUI TxtWeight => _txtWeight ??= GetComponent<TextMeshProUGUI>();
        private TextMeshProUGUI TxtRackUnit => _txtRackUnit ??= GetComponent<TextMeshProUGUI>();
        private TextMeshProUGUI TxtOccupyRackUnits => _txtOccupyRackUnits ??= GetComponent<TextMeshProUGUI>();
        [NonSerialized] private TextMeshProUGUI _txtDeviceType, _txtWatt, _txtWeight, _txtRackUnit, _txtOccupyRackUnits;
        #endregion
    }
}

