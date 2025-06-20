using UnityEngine;
using VictorDev.Revit;

namespace VictorDev.TCIT
{
    public class HUD_RackRevitnfo : MonoBehaviour
    {
        public RackModelDataExtended rackRevitAssetInfo;

        public void ReceiveRackRevitInfo(RackModelDataExtended rackAssetInfo)
        {
            rackRevitAssetInfo = rackAssetInfo;
        }
    }
}