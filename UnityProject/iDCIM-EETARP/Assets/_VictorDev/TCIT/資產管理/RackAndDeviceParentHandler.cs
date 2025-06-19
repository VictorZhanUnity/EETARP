using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using VictorDev.Common;
using Debug = VictorDev.Common.Debug;

namespace VictorDev.TCIT
{
    /// 處理機櫃與其設備模型之Paret架構
    /// <param> 利用Collider是否交互重疊，判斷是否為該機櫃的設備</param>
    public class RackAndDeviceParentHandler : MonoBehaviour
    {
        [Header("[設定] - 機櫃模型關鍵字")] [SerializeField]
        private List<string> rackKeywords = new() { "DCR" };

        [Button]
        private void SetRackAndModelsParent()
        {
            Debug.Log("SetRackAndModelsParent...", this, EmojiEnum.Robot);
            Racks.ForEach(targetRack =>
            {
                List<GameObject> modelInBound = ObjectHelper.GetModelInBound(targetRack, ModelsWithNotRack);
                modelInBound.ForEach(obj => obj.transform.parent = targetRack.transform);
            });
            Debug.Log("SetRackAndModelsParent... OK!", this, EmojiEnum.Done);
        }

        #region Variables

        /// 場景上所有DCR機櫃物件
        private List<GameObject> Racks
        {
            get
            {
                Collider[] colliders = FindObjectsByType<Collider>(FindObjectsSortMode.None); // 找到所有Collider
                List<GameObject> result = colliders.Where(col =>
                        rackKeywords.Any(word => col.name.Contains(word, StringComparison.OrdinalIgnoreCase)))
                    .Select(col => col.gameObject).ToList();
                return result;
            }
        }

        /// 場景上所有非機櫃模型
        private List<GameObject> ModelsWithNotRack
        {
            get
            {
                Collider[] colliders = FindObjectsByType<Collider>(FindObjectsSortMode.None); // 找到所有Collider
                List<GameObject> modelsWithNotRack = colliders.Where(col =>
                        rackKeywords.Any(word => col.name.Contains(word, StringComparison.OrdinalIgnoreCase)) == false)
                    .Select(col => col.gameObject).ToList();
                return modelsWithNotRack;
            }
        }
        #endregion
    }
}