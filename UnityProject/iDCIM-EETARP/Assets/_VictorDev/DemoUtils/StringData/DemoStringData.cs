using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using Debug = VictorDev.Common.Debug;

namespace VictorDev.DemoUtils
{
    /// 供StringAutoJumper使用
    [CreateAssetMenu(fileName = "DemoStringData", menuName = "VictorDev/SO/Demo String Data")]
    public class DemoStringData : ScriptableObject
    {
        public List<string> stringValues;

        [Foldout("[原始字串，項目之間用symbol隔開]")] [SerializeField, TextArea(1, 50)]
        private string sourceString;

        [Foldout("[Symbol轉換設定]")] [SerializeField]
        private string symbol = ",";

        [Button]
        private void ConvertSourceToStringValues()
        {
            sourceString = sourceString.Trim();
            
            symbol = symbol.Trim();
            stringValues.Clear();
            stringValues = sourceString.Replace("\r", "").Replace("\n", "").Split(symbol).ToList();
        }
        
        /// 取得隨機資料項
        public string GetRandomStringValue()
        {
            if (stringValues.Count == 0)
            {
                Debug.Log($"stringValues count is 0", this, EmojiEnum.Error);
                return string.Empty;
            }
            else
            {
                return stringValues[Random.Range(0, stringValues.Count)];
            }
        }
    }
}