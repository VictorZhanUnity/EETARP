using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using VictorDev.Common;
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

        public void ReceiveJsonString(string jsonString)
        {
            alarmData?.Clear();
            alarmData = JsonConvert.DeserializeObject<List<AlarmData>>(jsonString);
            CheckAllDataExists();
        }

        private void InvokeData()
        {
            receivers.OfType<IAlarmDataList>().ToList().ForEach(target =>
            {
                target.ReceiveData(alarmData);
            });
        }

        private void CheckAllDataExists()
        {
            if (alarmData.IsNullOrEmpty() || _rackModelAssetList.IsNullOrEmpty()) return;
            TempDataFixed();
            alarmData = alarmData.OrderBy(data => data.alarmTime).ToList();
            InvokeData();
        }

        private void OnValidate() => receivers = ObjectHelper.CheckTypoOfList<IAlarmDataList>(receivers);

        #region 暫用ForDemo

        private List<RackModelDataExtended> _rackModelAssetList;
        public void ReceiveRackModelAssetList(List<RackModelDataExtended> rackModelAssetList)
        {
            _rackModelAssetList?.Clear();
            _rackModelAssetList = rackModelAssetList;
            CheckAllDataExists();
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
            int removeCount = Random.Range(0, alarmData.Count);
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