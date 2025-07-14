using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;
using Debug = UnityEngine.Debug;

namespace VictorDev.TCIT.AlarmModule
{
    public class AlarmList : MonoBehaviour, AlarmDataManager.IAlarmDataList
    {
        [Foldout("[Event] - 點擊Item時Invoke")]
        public UnityEvent<AlarmListItem> onItemSelected;


        public void ReceiveData(List<AlarmData> data)
        {
            _alarmData = data;
            UpdateUI();
        }
        

        private void UpdateUI()
        {
            ClearData();
            
            _alarmData.ForEach(data =>
            {
                AlarmListItem item = ObjectHelper.Instantiate(listItemPrefab, ScrollRectInstance.content);
                item.ReceiveData(data);
                item.toggleGroup = ToggleGroupInstance;
                item.onItemSelected.AddListener(OnItemSelectedHandler); 
                _listItems.Add(item);
            });
        }

        private void OnItemSelectedHandler(AlarmListItem lisItem)
        {
            onItemSelected?.Invoke(lisItem);
        }

        /// 點擊告警的模型時
        public void ClickModelAsset(GameObject target)
        {
            AlarmListItem item = _listItems.FirstOrDefault(item => item.alarmData.alarmTargetModel == target.transform);
            if (item != null)
            {
                item.onItemSelected.Invoke(item);
                item.IsOn = true;
            }
        }

        private void ClearData()
        {
            _listItems ??= new List<AlarmListItem>();
            _listItems?.Clear();
            ObjectHelper.DestoryObjectsOfContainer(ScrollRectInstance.content);
            ScrollRectInstance.verticalNormalizedPosition = 1;
        }

        private void OnDisable()
        {
            _alarmData?.Clear();
            ClearData();
        }

        #region Variables
        
        [Foldout("[Prefab]")]
        [SerializeField] private AlarmListItem listItemPrefab;
        private List<AlarmData> _alarmData;

        [NonSerialized] private List<AlarmListItem> _listItems;
        
        private ScrollRect ScrollRectInstance => _scrollRect ??= transform.Find("Panel/Container/排序Table表格/ScrollRect滑動列表").GetComponent<ScrollRect>();
        [NonSerialized] private ScrollRect _scrollRect;
        
        private ToggleGroup ToggleGroupInstance => _toggleGroupInstance ??= GetComponent<ToggleGroup>();
        [NonSerialized] private ToggleGroup _toggleGroupInstance;
        #endregion
    }
}