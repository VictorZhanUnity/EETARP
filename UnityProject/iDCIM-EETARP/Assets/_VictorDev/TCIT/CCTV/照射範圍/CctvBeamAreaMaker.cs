using System;
using System.Linq;
using NaughtyAttributes;
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
                    Instantiate(beamAreaPrefab, cctv);
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

        private void Awake() => gameObject.SetActive(false);
    }
}