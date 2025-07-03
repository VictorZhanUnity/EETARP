using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Revit;
using Debug = VictorDev.Common.Debug;

namespace VictorDev.TCIT.StorageAssetUtils
{
    /// 配置管理 - 設備/庫存 管理器
    public class StorageAssetManager : MonoBehaviour
    {
        public void ReceiveStorageAssetJsonData(string jsonString) => TempDataHandler(jsonString);

        private void TempDataHandler(string jsonString)
        {
           rackModels = JsonConvert.DeserializeObject<List<RackModelDataExtended>>(jsonString);

            // 設備清單
           deviceModels = rackModels.SelectMany(rack => rack.Containers)
                .GroupBy(device => device.DeviceType)
                .Select(group => group.First()).ToList();

            // 機櫃清單
            rackModels = rackModels.GroupBy(rack => rack.DeviceType).Select(group => group.First()).ToList();

            CombineStorageAssetData();
        }

        private void CombineStorageAssetData()
        {
            if (_revitModels == null || _revitModels.Count == 0)
            {
                _revitModels = deviceModels.Cast<RevitModelDataExtended>()
                    .Concat(rackModels).ToList();
            }
            invokeStorageAssetData?.Invoke(_revitModels);
            
            Debug.Log($"TempDataHandler: 共{_revitModels.Count}筆", this, EmojiEnum.DataBox);
        }
        
        private void Start() => CombineStorageAssetData();

        #region Variables

        [Foldout("[Event] - Invoke庫存Asset資料(機櫃/設備)")]
        public UnityEvent<List<RevitModelDataExtended>> invokeStorageAssetData;
        private List<RevitModelDataExtended> _revitModels;

        [Foldout("[庫存Asset資料項]")] [SerializeField]
        private List<DeviceModelDataExtended> deviceModels;
        [Foldout("[庫存Asset資料項]")] [SerializeField]
        private List<RackModelDataExtended> rackModels;
        
        #endregion

    }
}