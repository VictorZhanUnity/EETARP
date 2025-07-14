using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Common;
using VictorDev.MaterialUtils;
using VictorDev.Revit;
using Random = UnityEngine.Random;

namespace VictorDev.TCIT.AlarmModule
{
    public class AlarmDataManager : MonoBehaviour, DeviceAssetManager.IRackModelAssetList
    {
        [Label("接收器")]
        public List<MonoBehaviour> receivers;
        
        [Label("[資料項]")]
        [SerializeField] private List<AlarmData> alarmData;

        [NonSerialized] private List<AlarmData> _todayAlarmData = null;
        
        [Foldout("[Toggle Lock]")]
        [SerializeField] private Toggle toggleLock;
        
        public void ReceiveJsonString(string jsonString)
        {
            alarmData?.Clear();
            alarmData = JsonConvert.DeserializeObject<List<AlarmData>>(jsonString);
            CheckAllDataReceived();
        }

        private void InvokeData()
        {
            receivers.OfType<IAlarmDataList>().ToList().ForEach(target =>
            {
                target.ReceiveData(alarmData);
            });
        }

        private void CheckAllDataReceived()
        {
            if (alarmData.IsNullOrEmpty() || _rackModelAssetList.IsNullOrEmpty()) return;
            TempDataFixed();
            alarmData = alarmData.OrderBy(data => data.alarmTime).ToList();

            //暫存Today的資料
            _todayAlarmData ??= alarmData.Copy();
            InvokeData();

           if(toggleLock.isOn) ShowTargetDevices();
        }

        /// 顯示告警的設備
        public void ShowTargetDevices()
        {
            HashSet<Transform> devices = alarmData.Select(data => data.alarmTargetModel).ToHashSet();
            ModelMaterialHandler.ReplaceMaterialWithExclude(devices);
        }

        public void ShowTodayAlarms()
        {
            alarmData = _todayAlarmData.Copy();
            InvokeData();
        }

        private void OnValidate() => receivers = ObjectHelper.CheckTypoOfList<IAlarmDataList>(receivers);

        #region 暫用ForDemo

        private List<RackModelDataExtended> _rackModelAssetList;
        public void ReceiveRackModelAssetList(List<RackModelDataExtended> rackModelAssetList)
        {
            _rackModelAssetList?.Clear();
            _rackModelAssetList = rackModelAssetList;
            CheckAllDataReceived();
        }

        private void TempDataFixed()
        {
            for (int i = 0; i < alarmData.Count; i++)
            {
                DateTime dateTime = alarmData[i].alarmTime;
                dateTime = DateTime.Today.Add(dateTime.TimeOfDay);
                alarmData[i].dateTimeString = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                // 設定目標模型
                alarmData[i].alarmTargetModel = _rackModelAssetList.SelectMany(rack=> rack.Containers)
                    .OrderBy(x => Random.value).First().Model;
            }
            
            // 隨機刪除幾樣資料項
            int removeCount = Random.Range(0, Mathf.RoundToInt(alarmData.Count*0.8f));
            for (int i = 0; i < removeCount; i++)
            {
                int randomIndex = Random.Range(0, alarmData.Count);
                alarmData.RemoveAt(randomIndex);
            }
        }
        #endregion

        public interface IAlarmDataList
        {
            /// 接收告警列表資料
            void ReceiveData(List<AlarmData> data);
        }
        public interface IAlarmData
        {
            /// 接收告警資料
            void ReceiveData(AlarmData item);
        }
        public interface IAlarmListItem
        {
            /// 接收告警ListItem
            void ReceiveData(AlarmListItem item);
        }
    }
}