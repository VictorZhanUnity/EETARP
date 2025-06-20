using UnityEngine;
using VictorDev.Revit;
using Debug = VictorDev.Common.Debug;

namespace VictorDev.TCIT
{
    public class PanelRackRevitInfo : MonoBehaviour
    {
        public RackModelDataExtended rackRevitAssetInfo;

        public void ReceiveRackRevitInfo(RackModelDataExtended rackAssetInfo)
        {
            rackRevitAssetInfo = rackAssetInfo;
        }
    }
}