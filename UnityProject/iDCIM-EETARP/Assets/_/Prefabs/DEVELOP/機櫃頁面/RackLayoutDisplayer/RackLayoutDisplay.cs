using System;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;
using VictorDev.Revit;

namespace VictorDev.TCIT
{
    /// 機櫃裡的設備Layout列表
    public class RackLayoutDisplay : MonoBehaviour, RackRevitInfoPage.IRackModelDataExtended
    {
        [Foldout("[Event] - 點擊設備Item時Invoke")]
        public UnityEvent<DeviceModelDataExtended, Toggle> onDeviceClicked = new();

        [Foldout("[Event] - 點擊設備詳細資訊時Invoke")]
        public UnityEvent<DeviceModelDataExtended> onDeviceDetailClicked = new();
        [Foldout("[Event] - 點擊設備詳細資訊時Invoke")]
        public UnityEvent<GameObject> onDeviceModelClicked = new();
        
        public void ReceiveRackModelData(RackModelDataExtended rackModelData)
        {
            _rackModelData = rackModelData;
            UpdateUI();
        }

        private void UpdateUI()
        {
            ObjectHelper.DestoryObjectsOfContainer(DeviceContainer);

            _rackModelData.Containers.ForEach(deviceData =>
            {
                RackLayoutListItem item = ObjectHelper.Instantiate(listItemPrefab, DeviceContainer);
                item.ReceiveDeviceModelData(deviceData);
                item.toggleGroup = ToggleGroupInstance;
                item.onToggleValueChanged.AddListener((data, tg)=>
                {
                    onDeviceDetailClicked?.Invoke(data);
                    onDeviceModelClicked?.Invoke(data.Model.gameObject);
                });
            });
            ScrollRectList.verticalNormalizedPosition = 1;
        }

        private void OnDisable() => ScrollRectList.verticalNormalizedPosition = 1;

        #region Variables

        private RackModelDataExtended _rackModelData;
        
        [Foldout("[設定]")]
        [SerializeField] private RackLayoutListItem listItemPrefab;

        private ScrollRect ScrollRectList => _scrollRectList ??=
            transform.Find("Panel/Container/RackLayout滑動列表").GetComponent<ScrollRect>();

        [NonSerialized] private ScrollRect _scrollRectList;
        private Transform DeviceContainer => _deviceContainer ??= ScrollRectList.content.Find("DeviceContainer");
        [NonSerialized] private Transform _deviceContainer;

        private ToggleGroup ToggleGroupInstance => _toggleGroup ??= DeviceContainer.GetComponent<ToggleGroup>();
        [NonSerialized] private ToggleGroup _toggleGroup;

        #endregion
    }
}