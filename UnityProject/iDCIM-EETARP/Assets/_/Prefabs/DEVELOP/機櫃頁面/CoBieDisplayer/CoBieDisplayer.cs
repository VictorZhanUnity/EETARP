using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Common;
using VictorDev.Revit;

namespace VictorDev.TCIT
{
    /// 顯示COBie資訊
    public class CoBieDisplayer : MonoBehaviour, RackRevitInfoPage.IRackModelDataExtended
    {
        public void ReceiveRackModelData(RackModelDataExtended data)
        {
            _rackModelData = data;
            UpdateUI();
        }

        private void UpdateUI()
        {
            ObjectHelper.DestoryObjectsOfContainer(ScrollRectInstance.content);
            Dictionary<string, string> cobieInfo = _rackModelData.information.ToDictionary();

            cobieInfo.ToList().ForEach(keyPair =>
            {
                CoBieListItem item = Instantiate(itemPrefab, ScrollRectInstance.content);
                item.ColumnName = keyPair.Key;
                item.Value = keyPair.Value;
            });
            ScrollRectInstance.verticalNormalizedPosition = 1;
        }


        #region Variables
 
        private RackModelDataExtended _rackModelData;
        [SerializeField] private CoBieListItem itemPrefab;

        private ScrollRect ScrollRectInstance =>
            _scrollRect ??= transform.Find("Panel/Container/ScrollRect滑動列表").GetComponent<ScrollRect>();

        [NonSerialized] private ScrollRect _scrollRect;

        #endregion
    }
}