using System;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using VictorDev.Common;
using VictorDev.ObjectUtils;

namespace VictorDev.TCIT
{
    /// 批次設定新增CCTV照射範圍
    public class CctvBeamAreaMaker : MonoBehaviour
    {
        [SerializeField] private GameObject beamAreaPrefab;
        [SerializeField] private ModelFinder modelFinder;
        
        [Button]
        private void AddBeamAreaToCctvs()
        {
            modelFinder.FindedModels.ToList().ForEach(cctv =>
            {
                if (cctv.childCount == 0)
                {
                    PrefabUtility.InstantiatePrefab(beamAreaPrefab, cctv);
                }
            });
        }

        [Button]
        private void RemoveBeamAreaFromCctvs()
        {
            modelFinder.FindedModels.ToList().ForEach(cctv =>
            {
                if (cctv.childCount > 0)
                {
                    ObjectHelper.DestoryObject(cctv.GetChild(0));
                }
            });
        }

        [Button]
        private void BeamAreaOn() => SetBeamArea(true);
        [Button]
        private void BeamAreaOff() => SetBeamArea(false);
        private void SetBeamArea(bool isOn)
        {
            modelFinder.FindedModels.ToList().ForEach(cctv =>
            {
                cctv.GetChild(0)?.gameObject.SetActive(isOn);
            });
        }
        
        private void Awake() => gameObject.SetActive(false);
    }
}