using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Revit;

namespace VictorDev.TCIT
{
    /// Revit設定管理
    public class RevitConfig : SingletonMonoBehaviour<RevitConfig>
    {
        [SerializeField] private List<ModelAssetIcons> modelAssetIcons;

        /// 依設備類別取得ICON
        public static Sprite GetModelAssetIcon(EnumRevitModelKind deviceKind) =>
            Instance.modelAssetIcons.FirstOrDefault(item => item.system == deviceKind)?.icon;

        [Serializable]
        public class ModelAssetIcons
        {
            public EnumRevitModelKind system;
            public Sprite icon;
        }
    }
}