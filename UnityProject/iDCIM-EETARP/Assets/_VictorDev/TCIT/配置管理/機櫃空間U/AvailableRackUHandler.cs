using System.Collections.Generic;
using System.Linq;
using _VictorDEV.Revit;
using UnityEngine;
using VictorDev.Revit;
using Debug = VictorDev.Common.Debug;

public class AvailableRackUHandler : MonoBehaviour, DeviceDataManager.IReceiverRackModelDataExtended
{

    public void ReceiverData(List<RackModelDataExtended> data)
    {
        rackModeldatas = data;
        rackModeldatas.ForEach(CreateRackSpaceDisplayer);
    }

    private void CreateRackSpaceDisplayer(RackModelDataExtended rackData)
    {
        rackData.AvailableULevels.ForEach(level =>
        {
            RackUnitDisplay display = Instantiate(itemPrefab, rackData.Model);
            display.ReceiveRackData(rackData);
            display.SetULocation(level);
            //displayer.gameObject.SetActive(false);
            rackData.AvailableUDisplayer.Add(display);
        });
    }

    private List<RackModelDataExtended> rackModeldatas;
    public RackUnitDisplay itemPrefab;
}
