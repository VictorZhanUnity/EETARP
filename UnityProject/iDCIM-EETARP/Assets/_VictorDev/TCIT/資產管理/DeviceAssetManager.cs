using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Revit;
using VictorDev.RevitUtils;
using Debug = VictorDev.Common.Debug;

namespace VictorDev.TCIT
{
    /// 設備資訊管理器
    public class DeviceAssetManager : MonoBehaviour
    {
        [Foldout("[Event] - Invoke所有機櫃Asset資訊")]
        public UnityEvent<List<RackModelDataExtended>> invokeAllRackDataInfo = new();
        
        [Foldout("[Event] - Invoke點擊的機櫃/設備Asset資訊")]
        public UnityEvent<RackModelDataExtended> invokeClickRackRevitInfo = new();

        [Foldout("[Event] - Invoke點擊的機櫃/設備Asset資訊")]
        public UnityEvent<DeviceModelDataExtended> invokeClickDeviceRevitInfo = new();

        [Foldout("[資料項] - 機櫃與設備列表")] [SerializeField]
        private List<RackModelDataExtended> rackAssetModels;

        private bool _isOn;
        /// 當非設備資產頁面時，不處理點擊事件
        public void SetSwitchOn(bool value) => _isOn = value;

        /// 接收機櫃與設備資訊JsonString
        public void ReceiveRevitAssetJsonData(string jsonString)
        {
            rackAssetModels = JsonConvert.DeserializeObject<List<RackModelDataExtended>>(jsonString);
            invokeAllRackDataInfo?.Invoke(rackAssetModels);
            Debug.Log($"ReceiveDeviceData: 共{rackAssetModels.Count}筆", this, EmojiEnum.DataBox);
        }

        [Button]
        private void SelectAllRackModel()
        {
            #if UNITY_EDITOR
            Selection.objects = rackAssetModels.Select(rack => rack.Model.gameObject).ToArray();
            #endif
        }

        /// 接收被點擊的Revit模型
        public void ReceiveClickedRevitModel(GameObject model)
        {
            if (_isOn == false) return;
            
            RevitModelDataExtended data = GetRevitAssetInfo(model);
            if (data != null)
            {
                switch (data)
                {
                    case RackModelDataExtended rackModelData: 
                        invokeClickRackRevitInfo?.Invoke(rackModelData);
                        break;
                    case DeviceModelDataExtended deviceModelData:
                       RackModelDataExtended target = rackAssetModels.FirstOrDefault(rack=> rack.devicePath.Equals(deviceModelData.rackDevicePath, StringComparison.OrdinalIgnoreCase));
                       deviceModelData.rackModelData ??= target;
                        invokeClickDeviceRevitInfo?.Invoke(deviceModelData);
                        break;
                }
            }
        }

        /// 依照模型取得相對應的RevitData
        public RevitModelDataExtended GetRevitAssetInfo(GameObject model)
        {
            string devicePath = RevitHelper.GetDevicePath(model.name);
            RevitModelDataExtended result = null;
            switch (RevitHelper.CheckRevitModelType(model.name))
            {
                case RevitHelper.EnumRevitModeType.Rack:
                    result = rackAssetModels.First(rack =>
                        rack.devicePath.Equals(devicePath, StringComparison.OrdinalIgnoreCase));
                    break;
                case RevitHelper.EnumRevitModeType.Device:
                    result = rackAssetModels.SelectMany(rack => rack.Containers).First(rack =>
                        rack.devicePath.Equals(devicePath, StringComparison.OrdinalIgnoreCase));
                    break;
            }
            return result; 
        }
        
        public interface IRackModelAssetList
        {
            void ReceiveRackModelAssetList(List<RackModelDataExtended> rackModelAssetList);
        }

        private void Start()
        {
            if(isInvokeRackAssetInStart) invokeAllRackDataInfo?.Invoke(rackAssetModels);
        }

        [Foldout("[設定]")]
        [SerializeField] private bool isInvokeRackAssetInStart = true;
    }
}