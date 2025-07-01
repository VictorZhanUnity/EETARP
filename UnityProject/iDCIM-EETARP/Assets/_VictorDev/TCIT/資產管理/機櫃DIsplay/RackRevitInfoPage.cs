using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Revit;

namespace VictorDev.TCIT
{
    public class RackRevitInfoPage : MonoBehaviour, RackRevitInfoPage.IRackModelDataExtended
    {
        [SerializeField] private List<MonoBehaviour> receiverComps;
        
        public void ReceiveRackModelData(RackModelDataExtended rackModelData)
        {
            rackRevitAssetData = rackModelData;
            InvokeData();
        }

        private void InvokeData()
        {
            receiverComps.Cast<IRackModelDataExtended>().ToList().ForEach(receiver=> receiver.ReceiveRackModelData(rackRevitAssetData));
            gameObject.SetActive(true);
        }

        private void OnValidate() => receiverComps = ObjectHelper.CheckTypoOfList<IRackModelDataExtended>(receiverComps);

        /// 從子物件裡取得Recivers
        [Button]
        private void GetReceiversFromChildren()
        {
            var components = GetComponentsInChildren<MonoBehaviour>();
            receiverComps = components.Where(comp=> comp != this && comp is IRackModelDataExtended).ToList();
        }


        public interface IRackModelDataExtended
        {
            void ReceiveRackModelData(RackModelDataExtended rackModelData);
        }
                
        [Space(100)]        
        public RackModelDataExtended rackRevitAssetData;

    }
}