using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.MaterialUtils;

namespace VictorDev.ObjectUtils
{
    /// ModelFinder的集合處理
    [Serializable]
    public class ModelFinderSet : MonoBehaviour
    {
        [SerializeField] List<ModelFinder> modelFinders = new List<ModelFinder>();

        public bool IsOn
        {
            set
            {
                if (value) ToShow();
                else ToHide();
            }
        }

        public void ToShow()
        {
            HashSet<Transform> models = modelFinders.SelectMany(finder => finder.FindedModels).ToHashSet();
            ModelMaterialHandler.ReplaceMaterialWithExclude(models);
        }

        public void ToHide() => ModelMaterialHandler.RestoreOriginalMaterials();
    }
}