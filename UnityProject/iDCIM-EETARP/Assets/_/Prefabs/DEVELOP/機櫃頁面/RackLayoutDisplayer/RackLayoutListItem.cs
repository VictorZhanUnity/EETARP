using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Revit;

namespace VictorDev.TCIT
{
    /// 機櫃設備份佈ListItem
    public class RackLayoutListItem : MonoBehaviour, RackRevitInfoPage.IDeviceModelDataExtended
    {
        [HideInInspector] public UnityEvent<bool, DeviceModelDataExtended, Toggle> onToggleValueChanged = new();

        public void ReceiveDeviceModelData(DeviceModelDataExtended deviceModelData)
        {
            _deviceModelData = deviceModelData;
            UpdateUI();
        }

        private void UpdateUI()
        {
            TxtLabel.SetText(_deviceModelData.DeviceName.Trim());
            name = _deviceModelData.DeviceName.Trim();
            CalculatePositionAndHeight();
        }

        /// 計算於Layout上的位置PosY
        private void CalculatePositionAndHeight()
        {
            int rackLocation = _deviceModelData.rackLocation;

            float startPosY = 1.5f;
            float eachLevelHeight = 30f;

            RectTransform rectTrans = GetComponent<RectTransform>();

            Vector2 sizeDelta = rectTrans.sizeDelta;
            sizeDelta.y = _deviceModelData.information.heightU * eachLevelHeight;
            rectTrans.sizeDelta = sizeDelta;

            rectTrans.localPosition = new Vector3(0, rackLocation * eachLevelHeight + startPosY, 0f);
        }

        private void OnValueChanged(bool isOn) => onToggleValueChanged?.Invoke(isOn, _deviceModelData, ToggleInstance);

        #region Initialize

        private void OnEnable() => ToggleInstance.onValueChanged.AddListener(OnValueChanged);
        private void OnDisable() => ToggleInstance.onValueChanged.RemoveListener(OnValueChanged);
        private void OnDestroy() => OnDisable();

        #endregion

        #region Variables

        private DeviceModelDataExtended _deviceModelData;

        public ToggleGroup ToggleGroup
        {
            set => ToggleInstance.group = value;
        }

        private Toggle ToggleInstance => _toggleInstance ??= GetComponent<Toggle>();
        private Toggle _toggleInstance;

        private TextMeshProUGUI TxtLabel => _txtLabel ??= transform.Find("TxtLabel").GetComponent<TextMeshProUGUI>();
        private TextMeshProUGUI _txtLabel;

        #endregion
    }
}