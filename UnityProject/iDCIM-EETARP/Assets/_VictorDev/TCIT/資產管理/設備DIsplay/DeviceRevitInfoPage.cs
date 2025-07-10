using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using VictorDev.Revit;

namespace VictorDev.TCIT
{
    /// 顯示設備資訊
    public class DeviceRevitInfoPage : MonoBehaviour, DeviceRevitInfoPage.IDeviceModelDataExtended
    {
        [Foldout("[設定] - 接收器組件")][SerializeField] private List<MonoBehaviour> receiverComps;
        [Foldout("[Event] - 接收到資料時Invoke")] public UnityEvent onReceiveDataEvent;

        public void ReceiveDeviceModelData(DeviceModelDataExtended deviceModelData)
        {
            _deviceRevitAssetData = deviceModelData;
            InvokeData();
        }

        private void InvokeData()
        {
            receiverComps.Cast<IDeviceModelDataExtended>().ToList().ForEach(receiver=> receiver.ReceiveDeviceModelData(_deviceRevitAssetData));
            gameObject.SetActive(true);
            onReceiveDataEvent?.Invoke();
        }

        private void OnValidate() => receiverComps = ObjectHelper.CheckTypoOfList<IDeviceModelDataExtended>(receiverComps);

        /// 從子物件裡取得Recivers
        [Button]
        private void GetReceiversFromChildren()
        {
            var components = GetComponentsInChildren<MonoBehaviour>();
            receiverComps = components.Where(comp=> comp != this && comp is IDeviceModelDataExtended).ToList();
        }
        
        /// 關閉頁面
        public void ClosePage() => gameObject.SetActive(false);
        
        public interface IDeviceModelDataExtended
        {
            void ReceiveDeviceModelData(DeviceModelDataExtended deviceModelData);
        }
        private DeviceModelDataExtended _deviceRevitAssetData;


        [Foldout("[Event] - 下架設備")] public UnityEvent<DeviceModelDataExtended> removeDeviceFromRack;
        public void ShutdownAndRemove()
        {
            removeDeviceFromRack?.Invoke(_deviceRevitAssetData);
        }
    }
}