using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;
using VictorDev.Revit;

namespace VictorDev.TCIT.StorageAssetUtils
{
    /// 配置管理 - 設備/庫存 列表
    public class StorageAssetList : MonoBehaviour
    {
        [Foldout("[Event] - 當項目被選擇時Invoke")]
        public UnityEvent<StorageAssetListItem> onItemSelected;
        
        public void ReciveData(List<RevitModelDataExtended> dataList)
        {
            _revitModelDataList = dataList;
            ShowServerList();
        }

        /// 切換到Server列表
        public void ShowServerList() => FilterByRevitModelKind(EnumRevitModelKind.Server);
        /// 切換到Router列表
        public void ShowRouterList() => FilterByRevitModelKind(EnumRevitModelKind.Router);
        /// 切換到Switch列表
        public void ShowSwitchList() => FilterByRevitModelKind(EnumRevitModelKind.Switch);

        private void FilterByRevitModelKind(EnumRevitModelKind modelKind)
        {
            _filteredRevitModelDataList = _revitModelDataList.Where(data=> data.DeviceKind == modelKind)
                .OrderBy(data=>data.System)
                .ThenBy(data => data.DeviceKind)
                .ThenBy(data => data.DeviceName)
                .ToList();
            UpdateUI();
        }
        
        // 切換到庫存列表
        public void ShowStorageList()
        {
            _filteredRevitModelDataList = _revitModelDataList.Distinct().ToList();
            UpdateUI();
        }
        
        private void UpdateUI()
        {
            ObjectHelper.DestoryObjectsOfContainer(ScrollRectInstance.content);
            _filteredRevitModelDataList.ForEach(data =>
            {
                StorageAssetListItem item = ObjectHelper.InstantiatePrefab(assetListItemPrefab, ScrollRectInstance.content);
                item.ReceiveData(data);
                item.onSelected.AddListener(onItemSelected.Invoke);
            });

            ScrollRectInstance.verticalNormalizedPosition = 1;
        }

        private void OnDisable()
        {
            _revitModelDataList?.Clear();
            defaultToggle.isOn = true;
        }
        
        #region Variables
        [Foldout("[設定]")]
        [SerializeField] private StorageAssetListItem assetListItemPrefab;
        [Foldout("[設定]")]
        [SerializeField] private Toggle defaultToggle;
        private List<RevitModelDataExtended> _revitModelDataList, _filteredRevitModelDataList;
        
        private ScrollRect ScrollRectInstance => _scrollRect ??= transform.Find("Panel/Container/ScrollRect滑動列表").GetComponent<ScrollRect>();
        [NonSerialized] private ScrollRect _scrollRect;
        #endregion
    }

    /// For ListItem
    public interface IRevitModelDataExtended
    {
        public bool IsOn { get; set; }

        public RevitModelDataExtended ReviteModelData { get; }
        [CanBeNull] public RackModelDataExtended RackModelDataInfo { get; }
        [CanBeNull] public DeviceModelDataExtended DeviceModelDataInfo { get; }
        void ReceiveData(RevitModelDataExtended revitModelDataExtended);
    }
}