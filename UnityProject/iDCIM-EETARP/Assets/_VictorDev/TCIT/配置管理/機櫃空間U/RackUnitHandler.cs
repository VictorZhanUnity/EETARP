using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Revit;
using VictorDev.TCIT;

public class RackUnitHandler : MonoBehaviour, DeviceAssetManager.IRackModelAssetList
{
    [Foldout("[Prefab] - RackUnit顯示")] public RackUnitDisplay rackSpaceDisplayPrefab;

    [NonSerialized] private List<RackModelDataExtended> _rackModelAssetList;
 

    public void ReceiveRackModelAssetList(List<RackModelDataExtended> data)
    {
        _rackModelAssetList = data;
        InitializeRackUSpacer();
    }

    /// 初始化建立機櫃的空格U
    private void InitializeRackUSpacer()
    {
        _rackModelAssetList.ForEach(rack =>
        {
            rack.AvailableULevels.ForEach(location =>
            {
                RackUnitDisplay rackUnit = ObjectHelper.Instantiate(rackSpaceDisplayPrefab, rack.Model);
                rackUnit.ReceiveRackData(rack);
                rackUnit.SetULocation(location);
                rackUnit.gameObject.SetActive(false);
                rack.AvailableUDisplayer.Add(rackUnit);
            });
        });
    }

    public void SetRackUnitVisible(bool isVisible)
    {
        _rackModelAssetList.SelectMany(rack => rack.AvailableUDisplayer).ToList()
            .ForEach(rackUnit => rackUnit.gameObject.SetActive(isVisible));
    }
}
