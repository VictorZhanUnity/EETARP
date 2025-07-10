using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using VictorDev.Revit;

namespace VictorDev.TCIT
{
    /// 顯示機櫃資訊
    public class RackRevitInfoPage : MonoBehaviour, RackRevitInfoPage.IRackModelDataExtended
    {
        [Foldout("[設定] - 接收器組件")][SerializeField] private List<MonoBehaviour> receiverComps;
        [Foldout("[Event] - 接收到資料時Invoke")] public UnityEvent onReceiveDataEvent;
        
        public void ReceiveRackModelData(RackModelDataExtended rackModelData)
        {
            _rackRevitAssetData = rackModelData;
            InvokeData();
        }

        private void InvokeData()
        {
            receiverComps.Cast<IRackModelDataExtended>().ToList().ForEach(receiver=> receiver.ReceiveRackModelData(_rackRevitAssetData));
            gameObject.SetActive(true);
            onReceiveDataEvent?.Invoke();
        }

        private void OnValidate() => receiverComps = ObjectHelper.CheckTypoOfList<IRackModelDataExtended>(receiverComps);

        /// 從子物件裡取得Recivers
        [Button]
        private void GetReceiversFromChildren()
        {
            var components = GetComponentsInChildren<MonoBehaviour>();
            receiverComps = components.Where(comp=> comp != this && comp is IRackModelDataExtended).ToList();
        }

        /// 關閉頁面
        public void ClosePage() => gameObject.SetActive(false);

        public interface IRackModelDataExtended
        {
            void ReceiveRackModelData(RackModelDataExtended rackModelData);
        }
        private RackModelDataExtended _rackRevitAssetData;
    }
}