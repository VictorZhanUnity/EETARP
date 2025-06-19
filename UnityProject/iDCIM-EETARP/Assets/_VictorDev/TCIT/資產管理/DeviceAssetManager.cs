using System;
using System.Collections.Generic;
using System.Linq;
using _VictorDEV.Revit;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using Debug = VictorDev.Common.Debug;

namespace VictorDev.TCIT
{
    public class DeviceAssetManager : MonoBehaviour
    {
        [Foldout("[資料項] - 機櫃與設備列表")] [SerializeField] private List<RackModelDataExtended> rackModels;

        [NonSerialized]private GameObject _currentClickModel;
        
        public void ReceiveDeviceJsonData(string jsonData)
        {
            rackModels = JsonConvert.DeserializeObject<List<RackModelDataExtended>>(jsonData);
            Debug.Log($"ReceiveDeviceData: 共{rackModels.Count}筆", this, EmojiEnum.DataBox);
        }

        public void ReceiveClickedDeviceModel(List<GameObject> models)
        {
            _currentClickModel = models.First();
            Debug.Log($"OnClickDeviceModel: {_currentClickModel.name}");

            if (_currentClickModel.TryGetComponent(out MeshRenderer meshRenderer))
            {
                meshRenderer.enabled = !meshRenderer.enabled;
            }
        }
    }
}