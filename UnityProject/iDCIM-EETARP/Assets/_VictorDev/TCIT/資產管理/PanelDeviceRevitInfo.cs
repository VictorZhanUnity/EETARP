using UnityEngine;
using VictorDev.Revit;

namespace VictorDev.TCIT
{
    public class PanelDeviceRevitInfo : MonoBehaviour
    {
        public DeviceModelDataExtended deviceRevitAssetInfo;

        public void ReceiveDeviceRevitInfo(DeviceModelDataExtended deviceAssetInfo)
        {
            Debug.Log($"Click Device: {deviceAssetInfo.devicePath}");
            deviceRevitAssetInfo = deviceAssetInfo;
        }
    }
}