using System;
using System.Collections;
using TMPro;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Revit;

namespace VictorDev.TCIT
{
    /// [ToolTip] - 配置設備資訊
    public class RevitAssetMouseToolTip : MonoBehaviour
    {
        public void ReceiveRevitModel(GameObject model)
        {
            var revitAssetInfo = DeviceAssetManagerInstance.GetRevitAssetInfo(model);
    
            // 檢查是否有指向到RevitAsset資產模型
            if (revitAssetInfo == null)
            {
                if(_coroutine != null) StopCoroutine(_coroutine);
                return;
            }else if(revitAssetInfo == _currentRevitAssetInfo)
            {
                // 若AssetInfo與上一個相同，則跳過
                return;
            }
            
            _currentRevitAssetInfo = revitAssetInfo;
            Container.gameObject.SetActive(false);
            if(_coroutine != null) StopCoroutine(_coroutine);
            
            IEnumerator Func()
            {
                yield return new WaitForSeconds(delaySec);
                Container.gameObject.SetActive(true);
                string msg = $"{revitAssetInfo.DeviceName}\n";
                switch (revitAssetInfo)
                {
                    case RackModelDataExtended rackAsset:
                        msg += $"\n型號：{rackAsset.DeviceType}" +
                               $"\n剩餘電力：{rackAsset.RemainOfWatt}kMh" +
                               $"\n剩餘負重：{rackAsset.RemainOfWeight}KG" +
                               $"\n剩餘空間：{rackAsset.RemainOfHeightU}U";
                        break;
                    case DeviceModelDataExtended deviceAsset:
                        msg += $"\n型號：{deviceAsset.DeviceType}" +
                               $"\n電力：{deviceAsset.information.watt}kMh" +
                               $"\n重量：{deviceAsset.information.weight}KG" +
                               $"\n尺寸：{deviceAsset.information.heightU}U";
                        break;
                }
                Txt.SetText(msg);
            }
            _coroutine = StartCoroutine(Func());
        }

        private void Start() => Container.gameObject.SetActive(false);

        #region Variables
        [SerializeField] private float delaySec = 0.5f;
        
        private RevitModelDataExtended _currentRevitAssetInfo;
        private Coroutine _coroutine;

        private Transform Container => _container ??= transform.Find("Container");
        [NonSerialized] private Transform _container;
        
        private TextMeshProUGUI Txt => _txt ??= GetComponentInChildren<TextMeshProUGUI>();
        [NonSerialized] private TextMeshProUGUI _txt;

        private DeviceAssetManager DeviceAssetManagerInstance =>
            _deviceAssetManager ??= FindFirstObjectByType<DeviceAssetManager>();
        [NonSerialized] private DeviceAssetManager _deviceAssetManager;
        #endregion
    }
}