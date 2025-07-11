using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        /// 從物件名稱取得DevicePath
        public static string GetDevicePath(Transform target)
        {
            Match match = Regex.Match(target.name.Trim(), @"\[(.*?)\]");
            return match.Success ? match.Groups[1].Value: string.Empty;
        }
        
        /// 從物件名稱取得DeviceName
        public static string GetDeviceName(Transform target)
        {
           string devicePath = GetDevicePath(target);
           return devicePath.Split(":")[1].Trim();
        }
    }
}