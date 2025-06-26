using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Revit;

public class RevitConfig: SingletonMonoBehaviour<RevitConfig>
{
   [SerializeField] private List<ModelAssetIcons> modelAssetIcons;
   
   /// 依設備類別取得ICON
   public static Sprite GetModelAssetIcon(EnumReviteModelKind deviceKind) => Instance.modelAssetIcons.FirstOrDefault(item => item.system.Equals(deviceKind))?.icon;

   [Serializable]
   public class ModelAssetIcons
   {
      public EnumReviteModelKind system;
      public Sprite icon;
   }
}
