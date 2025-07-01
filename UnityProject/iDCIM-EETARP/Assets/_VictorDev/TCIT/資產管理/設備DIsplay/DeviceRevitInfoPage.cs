using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Revit;

namespace VictorDev.TCIT
{
    public class DeviceRevitInfoPage : MonoBehaviour, DeviceRevitInfoPage.IDeviceModelDataExtended
    {
        [SerializeField] private List<MonoBehaviour> receiverComps;
        
        public void ReceiveDeviceModelData(DeviceModelDataExtended deviceModelData)
        {
            
            deviceRevitAssetData = deviceModelData;
            InvokeData();
        }

        private void InvokeData()
        {
            receiverComps.Cast<IDeviceModelDataExtended>().ToList().ForEach(receiver=> receiver.ReceiveDeviceModelData(deviceRevitAssetData));
            gameObject.SetActive(true);
        }

        private void OnValidate() => receiverComps = ObjectHelper.CheckTypoOfList<IDeviceModelDataExtended>(receiverComps);

        /// 從子物件裡取得Recivers
        [Button]
        private void GetReceiversFromChildren()
        {
            var components = GetComponentsInChildren<MonoBehaviour>();
            receiverComps = components.Where(comp=> comp != this && comp is IDeviceModelDataExtended).ToList();
        }
        public interface IDeviceModelDataExtended
        {
            void ReceiveDeviceModelData(DeviceModelDataExtended deviceModelData);
        }
        public DeviceModelDataExtended deviceRevitAssetData;
    }
}