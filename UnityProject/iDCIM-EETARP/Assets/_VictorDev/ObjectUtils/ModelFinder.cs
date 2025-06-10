using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using VictorDev.Common;
using VictorDev.MaterialUtils;

namespace VictorDev.ObjectUtils
{
    [Serializable]
    public class ModelFinder:MonoBehaviour
    {
        [Button]
        public void FindTargetObjects() => findedModels = ModelMaterialHandler.FindTargetObjects(objKeyWords);

        [Button]
        public void AddColliderToObjects() => ObjectHelper.AddColliderToObjects(findedModels, new BoxCollider());

        [Button]
        public void RemoveColliderFromObjects() => ObjectHelper.RemoveColliderFromObjects(findedModels);
        
        [Button]
        public void SelectObjects() => Selection.objects = findedModels.Select(t=>t.gameObject).ToArray();

        public bool IsOn
        {
            set
            {
                if(value) ToShow();
                else ToHide();
            }
        }
        
        public void ToShow(bool isRestoreMaterial=true) => ModelMaterialHandler.ReplaceMaterialWithExclude(FindedModels, null);

        public void ToHide() => ModelMaterialHandler.RestoreOriginalMaterials();
        
        #region Variables
        [Header("[Name關鍵字]")]
        [SerializeField] List<string> objKeyWords;
        
        [Header(">>> 搜尋到的模型")]
        [SerializeField] List<Transform> findedModels;
        
        /// 搜尋到的模型
        public HashSet<Transform> FindedModels => _findedModelsHashSet ??= findedModels.ToHashSet();

        private HashSet<Transform> _findedModelsHashSet;

        #endregion
    }
}