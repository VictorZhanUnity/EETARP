using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.ColorUtils;
using VictorDev.MaterialUtils;
using VictorDev.Revit;
using Random = UnityEngine.Random;

namespace VictorDev.TCIT.StorageAssetUtils
{
    /// HUD_機櫃條件過濾顯示
    public class RackResourcesFilter : MonoBehaviour, DeviceAssetManager.IRackModelAssetList
    {
        [Header("[設定] - 機櫃過濾顏色符合條件優劣")] public List<ColorHelper.ColorLevel> colorLevels = new()
        {
            new(60f, ColorHelper.HexToColor(0x00FF00)),
            new(80f, ColorHelper.HexToColor(0xFFCE00)),
            new(100f, ColorHelper.HexToColor(0xFF0000)),
        };

        /// 接收所有機櫃資料
        public void ReceiveRackModelAssetList(List<RackModelDataExtended> rackData) =>_rackModelDataList = rackData;

        /// 接收目前選擇的設備資料
        public void ReceiveSelectedRevitAssetData(RevitModelDataExtended revitAssetData)
        {
            if (revitAssetData is DeviceModelDataExtended deviceModelData)
            {
                _selectedDeviceData = deviceModelData;
                gameObject.SetActive(_selectedDeviceData != null);
                ToFilterRack();
            }
        }

        #region 條件過濾機櫃顯示處理

        /// 過濾機櫃
        private void ToFilterRack()
        {
            if (_selectedDeviceData == null) return;
            _rackModelDataList.ForEach(rack =>
            {
                bool isSuitable = rack.IsDeviceSuitable(_selectedDeviceData);
                ChangeRackHeight(rack, isSuitable);
                ChangeRackColor(rack, isSuitable);
            });
        }

        /// 依照是否符合條件而調整機櫃大小
        private void ChangeRackHeight(RackModelDataExtended rackData, bool isSuitable)
        {
            DOTween.Kill(rackData.Model);
            rackData.Model
                .DOScaleY(isSuitable ? 1 : minScale, tweenDuration * (isSuitable ? 1 : 0.5f))
                .SetEase(isSuitable ? easeOut : easeIn)
                .SetDelay(Random.Range(0f, tweenDuration)).SetAutoKill(true);
        }

        /// 依照是否符合條件而調整機櫃顏色與是否透明
        private void ChangeRackColor(RackModelDataExtended rackData, bool isSuitable)
        {
            int rackMaterialIndex = rackData.Model.name.Contains("ATEN") ? 0 : 0;
            int rackHoleIndex = rackData.Model.name.Contains("ATEN") ? 3 : 5;

            if (rackData.Model.name.Contains("DAMAC", StringComparison.OrdinalIgnoreCase))
            {
                rackMaterialIndex = 2;
                rackHoleIndex = -1;
            }

            Material[] mats = rackData.Model.GetComponent<MeshRenderer>().materials;

            for (int i = 0; i < mats.Length; i++)
            {
                Color color = mats[i].color;
                if (i == rackMaterialIndex) //當目前mat為機櫃外殼時處理
                {
                    if (isSuitable == false)
                        color = rackData.Model.name.Contains("ATEN")
                            ? ColorHelper.HexToColor(0x333333)
                            : ColorHelper.HexToColor(0xF4F1ED);
                    else
                    {
                        //根據filter選項來取得剩餘資源百分八
                        List<float> filterUsagePercentList = new List<float>();
                        if (ToggleWatt.isOn) filterUsagePercentList.Add(rackData.UsagePercentOfWatt);
                        if (ToggleWeight.isOn) filterUsagePercentList.Add(rackData.UsagePercentOfWeight);
                        if (ToggleRackUnit.isOn) filterUsagePercentList.Add(rackData.UsagePercentOfHeightU);

                        float percentage = filterUsagePercentList.Sum(value => value) / filterUsagePercentList.Count;
                        if (!ToggleWatt.isOn && !ToggleWeight.isOn && !ToggleRackUnit.isOn)
                        {
                            color = rackData.Model.name.Contains("ATEN")
                                ? ColorHelper.HexToColor(0x333333)
                                : ColorHelper.HexToColor(0xF4F1ED);
                        }
                        else
                        {
                            color = ColorHelper.GetColorFromPercentage(percentage, colorLevels);
                        }
                    }
                }

                color.a = isSuitable ? 0 : 1;
                if (rackMaterialIndex >= 0 && isSuitable || i == rackHoleIndex)
                    MaterialHelper.SetTransparentMode(mats[i]);
                else MaterialHelper.SetOpaqueMode(mats[i]);

                DOTween.Kill(mats[i]);
                mats[i].DOColor(color, tweenDuration).SetEase(isSuitable ? easeOut : easeIn).SetAutoKill(true);
            }
        }

        #endregion

        /// 重置所有機櫃樣式
        public void ResetRacks()
        {
            _rackModelDataList?.ForEach(rack =>
            {
                ChangeRackHeight(rack, true);
                ChangeRackColor(rack, false);
            });
        }

        #region Initialize

        private void Awake() => gameObject.SetActive(false);

        private void OnEnable()
        {
            ToggleWatt.onValueChanged.AddListener(_ => ToFilterRack());
            ToggleWeight.onValueChanged.AddListener(_ => ToFilterRack());
            ToggleRackUnit.onValueChanged.AddListener(_ => ToFilterRack());
        }

        private void OnDisable()
        {
            ToggleWatt.onValueChanged.RemoveListener(_ => ToFilterRack());
            ToggleWeight.onValueChanged.RemoveListener(_ => ToFilterRack());
            ToggleRackUnit.onValueChanged.RemoveListener(_ => ToFilterRack());
            ResetRacks();
        }

        #endregion

        #region Variables

        [Foldout("[設定] - Dotween參數")]
        [SerializeField] private float minScale = 0.02f, tweenDuration = 0.3f;
        [Foldout("[設定] - Dotween參數")]
        [SerializeField] private Ease easeOut = Ease.OutQuad, easeIn = Ease.InQuad;

        /// [資料項] - 所有機櫃資料
        [NonSerialized] private List<RackModelDataExtended> _rackModelDataList;

        /// 目前選擇的設備資料
        private DeviceModelDataExtended _selectedDeviceData;

        private Toggle ToggleWatt => _toggleWatt ??= transform.Find("Container/ToggleWatt").GetComponent<Toggle>();
        private Toggle ToggleWeight =>
            _toggleWeight ??= transform.Find("Container/ToggleWeight").GetComponent<Toggle>();
        private Toggle ToggleRackUnit =>
            _toggleRackUnit ??= transform.Find("Container/ToggleRackUnit").GetComponent<Toggle>();
        [NonSerialized] private Toggle _toggleWatt, _toggleWeight, _toggleRackUnit;

        #endregion

       
    }
}