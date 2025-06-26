using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Newtonsoft.Json;
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
        [Foldout("[Event] - Invoke機櫃/設備Asset資訊")]
        public UnityEvent<RackModelDataExtended> invokeClickRackRevitInfo = new();

        [Foldout("[Event] - Invoke機櫃/設備Asset資訊")]
        public UnityEvent<DeviceModelDataExtended> invokeClickDeviceRevitInfo = new();

        [Foldout("[資料項] - 機櫃與設備列表")] [SerializeField]
        private List<RackModelDataExtended> rackModels;

        /// 接收機櫃與設備資訊JsonString
        public void ReceiveRevitAssetJsonData(string jsonString)
        {
            rackModels = JsonConvert.DeserializeObject<List<RackModelDataExtended>>(jsonString);
            Debug.Log($"ReceiveDeviceData: 共{rackModels.Count}筆", this, EmojiEnum.DataBox);
        }

        /// 接收被點擊的Revit模型
        public void ReceiveClickedRevitModel(GameObject model)
        { 
            RevitModelDataExtended data = GetRevitAssetInfo(model);
            if (data != null)
            {
                switch (data)
                {
                    case RackModelDataExtended rackModelData: 
                        invokeClickRackRevitInfo?.Invoke(rackModelData);
                        break;
                    case DeviceModelDataExtended deviceModelData:
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
                    result = rackModels.First(rack =>
                        rack.devicePath.Equals(devicePath, StringComparison.OrdinalIgnoreCase));
                    break;
                case RevitHelper.EnumRevitModeType.Device:
                    result = rackModels.SelectMany(rack => rack.Containers).First(rack =>
                        rack.devicePath.Equals(devicePath, StringComparison.OrdinalIgnoreCase));
                    break;
            }
            return result;
        }
    }
}