using System;
using System.Linq;
using TMPro;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Revit;

public class Window_RemoveDevice : MonoBehaviour
{
    public void ReceiveData(DeviceModelDataExtended data)
    {
        _deviceRevitAssetData = data;
        UpdateUI();
    }

    private void UpdateUI()
    {
        TxtDeviceName.SetText(_deviceRevitAssetData.DeviceName);
        BlackBkg.SetActive(true);
        gameObject.SetActive(true);
    }

    public void ShutdownAndRemove()
    {
        ObjectHelper.DestoryObject(_deviceRevitAssetData.Model);
        
        int rackLocation = _deviceRevitAssetData.rackLocation;
        int heightU = _deviceRevitAssetData.information.heightU;
        RackModelDataExtended rackModelData = _deviceRevitAssetData.rackModelData;
        
        for (int i = rackLocation; i < rackLocation + heightU; i++)
        {
            RackUnitDisplay rackUnit = ObjectHelper.Instantiate(rackUnitDisplayPrefab, rackModelData.Model);
            rackUnit.ReceiveRackData(rackModelData);
            rackUnit.SetULocation(i);
            rackUnit.gameObject.SetActive(false);
            rackModelData.AvailableUDisplayer.Add(rackUnit);
        }
        
        rackModelData.AvailableUDisplayer = rackModelData.AvailableUDisplayer.OrderBy(rackUnit => rackUnit.ULevel).ToList();
        
        Cancel();
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
        BlackBkg.SetActive(false);
    }

    private DeviceModelDataExtended _deviceRevitAssetData;
    
    [SerializeField] private RackUnitDisplay rackUnitDisplayPrefab;
    
    private TextMeshProUGUI TxtDeviceName => _txtDeviceName ??= transform.Find("Panel/Container/TxtDeviceName").GetComponent<TextMeshProUGUI>();
    [NonSerialized] private TextMeshProUGUI _txtDeviceName;

    private GameObject BlackBkg => _blackBkg ??= transform.parent.Find("BlackBkg").gameObject;
    [NonSerialized] private GameObject _blackBkg;

}
