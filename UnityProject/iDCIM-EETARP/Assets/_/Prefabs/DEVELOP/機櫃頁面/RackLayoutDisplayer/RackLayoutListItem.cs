using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Revit;

namespace VictorDev.TCIT
{
    /// 機櫃設備份佈ListItem
    public class RackLayoutListItem : MonoBehaviour, DeviceRevitInfoPage.IDeviceModelDataExtended
    {
        [HideInInspector] public UnityEvent<DeviceModelDataExtended, Toggle> onToggleValueChanged = new();
        public DeviceModelDataExtended _deviceModelData;
        
        public void ReceiveDeviceModelData(DeviceModelDataExtended deviceModelData)
        {
            _deviceModelData = deviceModelData;
            UpdateUI();
        }

        private void UpdateUI()
        {
            icon.sprite = _deviceModelData.ModelAssetIcon;
            TxtDeviceType.SetText(_deviceModelData.DeviceType);
            name = $"[U{_deviceModelData.rackLocation}][{_deviceModelData.DeviceKind}] - {_deviceModelData.DeviceName} - {_deviceModelData.information.heightU}U";
            
            // 設置Rack位置
            int posY = Mathf.RoundToInt((_deviceModelData.rackLocation - 1) * _originalItemHeight);
            transform.localPosition = new Vector3(0, posY, 0f);
            // 設置高度
            ItemHeight = _deviceModelData.information.heightU * ItemHeight;
        }

        #region Initialize

        private void OnEnable()
        { 
            // 取得Item原始高度
            if(_originalItemHeight == 0) _originalItemHeight= ItemHeight;
            ToggleInstance.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDisable() => ToggleInstance.onValueChanged.RemoveListener(OnValueChanged);
        private void OnValueChanged(bool isOn) => onToggleValueChanged?.Invoke(_deviceModelData, ToggleInstance);
        private void OnDestroy() => OnDisable();

        #endregion

        #region Variables

        public ToggleGroup toggleGroup
        {
            set => ToggleInstance.group = value;
        }

        private Toggle ToggleInstance => _toggleInstance ??= GetComponent<Toggle>();
        [NonSerialized] private Toggle _toggleInstance;
        
        private Image icon => _icon ??= transform.Find("ICON").GetComponent<Image>();
        [NonSerialized] private Image _icon;

        private TextMeshProUGUI TxtDeviceType =>
            _txtDeviceType ??= transform.Find("TxtDeviceType").GetComponent<TextMeshProUGUI>();

        [NonSerialized] private TextMeshProUGUI _txtDeviceType;


        /// 原始尺吋高度
        private int _originalItemHeight;
        
        /// 目前尺吋高度
        private int ItemHeight
        {
            get => Mathf.RoundToInt((_rectTransform ??= GetComponent<RectTransform>()).sizeDelta.y);
            set
            {
                Vector2 size = (_rectTransform ??= GetComponent<RectTransform>()).sizeDelta;
                size.y = value;
                _rectTransform.sizeDelta = size;
            }
        }

        [NonSerialized] private RectTransform _rectTransform;

        #endregion
    }
}