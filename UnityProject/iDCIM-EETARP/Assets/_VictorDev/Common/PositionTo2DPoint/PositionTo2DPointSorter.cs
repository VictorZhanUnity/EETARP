using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using VictorDev.Common;

namespace _VictorDEV.Common
{
    /// 對PositionTo2DPoint進行前後排序
    public class PositionTo2DPointSorter : MonoBehaviour
    {
        [SerializeField] private List<PositionTo2DPoint> posTo2DPointList;

        private void Update()
        {
            // 根据攝影機距离对Landmark进行排序并调整Sibling Index
            posTo2DPointList.Sort((a, b) => b.DistanceFromCamera.CompareTo(a.DistanceFromCamera));
            for (int i = 0; i < posTo2DPointList.Count; i++)
            {
                posTo2DPointList[i].transform.SetSiblingIndex(i);
            }
        }
        
        [Button]
        private void GetLandmarksFromThisContainer()
        {
            posTo2DPointList = GetComponentsInChildren<PositionTo2DPoint>(true).ToList();
        }
        
        [Button]
        private void Clear() => posTo2DPointList.Clear();
    }
}