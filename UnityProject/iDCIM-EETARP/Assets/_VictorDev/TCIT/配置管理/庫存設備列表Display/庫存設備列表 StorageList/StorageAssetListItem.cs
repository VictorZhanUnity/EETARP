using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Revit;

namespace VictorDev.TCIT.StorageAssetUtils
{
    /// 配置管理 - (庫存)設備列表ListItem 
    public class StorageAssetListItem : MonoBehaviour, IRevitModelDataExtended
    {
        [Foldout("[Event] - 當被選擇時Invoke")]
        public UnityEvent<StorageAssetListItem> onSelected;

        public bool IsOn
        {
            get => ToggleInstance.isOn;
            set => ToggleInstance.isOn = value;
        }

        public RevitModelDataExtended ReviteModelData { get; private set; }
        public RackModelDataExtended RackModelDataInfo { get; private set; }
        public DeviceModelDataExtended DeviceModelDataInfo{ get; private set; }
        
        public void ReceiveData(RevitModelDataExtended revitModelDataExtended)
        {
            ReviteModelData = revitModelDataExtended;
            UpdateUI();
        }

        private void UpdateUI()
        {
            string deviceType = "";

            if (ReviteModelData is DeviceModelDataExtended dataDevice)
            {
                DeviceModelDataInfo = dataDevice;
                deviceType = dataDevice.DeviceType;
            }
            else if (ReviteModelData is RackModelDataExtended dataRack)
            {
                RackModelDataInfo = dataRack;
                deviceType = dataRack.DeviceType;
            }
            
            TxtDeviceType.SetText(deviceType);
            TxtWatt.SetText(ReviteModelData.information.watt.ToString("N0"));
            TxtWeight.SetText(ReviteModelData.information.height.ToString("N0"));
            TxtRackUnit.SetText(ReviteModelData.information.heightU.ToString("N0"));

            Icon.sprite = ReviteModelData.ModelAssetIcon;
        }

        #region Initialized

        private void Start() => ToggleInstance.group = GetComponentInParent<ToggleGroup>();
        private void OnEnable() => ToggleInstance.onValueChanged.AddListener(OnToggleValueChanged);
        private void OnDisable() => ToggleInstance.onValueChanged.RemoveListener(OnToggleValueChanged);
        private void OnToggleValueChanged(bool isOn) => onSelected?.Invoke(this);

        #endregion

        #region Variables

        private Toggle ToggleInstance => _toggle ??= GetComponent<Toggle>();
        [NonSerialized] private Toggle _toggle;

        private Image Icon => _icon ??= transform.Find("Container/ICON").GetComponent<Image>();
        [NonSerialized] private Image _icon;

        private TextMeshProUGUI TxtDeviceType =>
            _txtDeviceType ??= transform.Find("Container/TxtDeviceType").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtWatt =>
            _txtWatt ??= transform.Find("Container/HLayout/TxtWatt").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtWeight =>
            _txtWeight ??= transform.Find("Container/HLayout/TxtWeight").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtRackUnit => _txtRackUnit ??=
            transform.Find("Container/HLayout/TxtRackUnit").GetComponent<TextMeshProUGUI>();

        [NonSerialized] private TextMeshProUGUI _txtDeviceType, _txtWatt, _txtWeight, _txtRackUnit;

        #endregion
    }
}