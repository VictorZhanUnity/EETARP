using UnityEngine;
using VictorDev.Revit;
using Debug = VictorDev.Common.Debug;

namespace VictorDev.TCIT
{
    public class PanelDeviceRevitInfo : MonoBehaviour
    {
        public DeviceModelDataExtended deviceRevitAssetInfo;

        public void ReceiveDeviceRevitInfo(DeviceModelDataExtended deviceAssetInfo)
        {
            deviceRevitAssetInfo = deviceAssetInfo;
        }
    }
}