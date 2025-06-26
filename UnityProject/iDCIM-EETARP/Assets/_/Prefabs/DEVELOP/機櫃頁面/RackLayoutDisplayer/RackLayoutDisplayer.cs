using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Common;
using VictorDev.Revit;
using VictorDev.TCIT;

namespace VictorDev
{
    public class RackLayoutDisplayer : MonoBehaviour, RackRevitInfoPage.IRackModelDataExtended
    {
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
                RackLayoutListItem item = PrefabUtility.InstantiatePrefab(_rackLayoutListItem, DeviceContainer) as RackLayoutListItem;
                item.ReceiveDeviceModelData(deviceData);
            });

            ScrollRectList.verticalNormalizedPosition = 1;
        }

        private void OnDisable() => ScrollRectList.verticalNormalizedPosition = 1;

        #region Variables
        private RackModelDataExtended _rackModelData;
        private RackLayoutListItem _rackLayoutListItem;

        private ScrollRect ScrollRectList => _scrollRectList ??=
            transform.Find("Panel/Container/RackLayout滑動列表").GetComponent<ScrollRect>();

        [NonSerialized] private ScrollRect _scrollRectList;
        private Transform DeviceContainer => _deviceContainer ??= ScrollRectList.content.Find("DeviceContainer");
        [NonSerialized] private Transform _deviceContainer;
        #endregion
    }
}