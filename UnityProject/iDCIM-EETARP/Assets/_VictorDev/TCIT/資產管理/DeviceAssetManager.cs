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
        [Foldout("[Event] - Invoke機櫃Revit資訊")] public UnityEvent<RackModelDataExtended> invokeRackRevitInfo = new();

        [Foldout("[Event] - Invoke設備Revit資訊")] public UnityEvent<DeviceModelDataExtended> invokeDeviceRevitInfo = new();

        [Foldout("[資料項] - 機櫃與設備列表")] [SerializeField]
        private List<RackModelDataExtended> rackModels;

        /// 接收機櫃與設備資訊JsonString
        public void ReceiveDeviceJsonData(string jsonString)
        {
            rackModels = JsonConvert.DeserializeObject<List<RackModelDataExtended>>(jsonString);
            Debug.Log($"ReceiveDeviceData: 共{rackModels.Count}筆", this, EmojiEnum.DataBox);
        }

        /// 接收被點擊的設備模型
        public void ReceiveClickedDeviceModel(GameObject model) => GetModelInfo(model);

        private void GetModelInfo(GameObject model)
        {
            string devicePath = RevitHelper.GetDevicePath(model.name);

            switch (RevitHelper.CheckRevitModelType(model.name))
            {
                case RevitHelper.EnumRevitModeType.Rack:
                    var targetRack = rackModels.First(rack =>
                        rack.devicePath.Equals(devicePath, StringComparison.OrdinalIgnoreCase));
                    invokeRackRevitInfo?.Invoke(targetRack);
                    break;
                case RevitHelper.EnumRevitModeType.Device:
                    var targetDevice = rackModels.SelectMany(rack => rack.Containers).First(rack =>
                        rack.devicePath.Equals(devicePath, StringComparison.OrdinalIgnoreCase));
                    invokeDeviceRevitInfo?.Invoke(targetDevice);
                    break;
            }
        }
    }
}