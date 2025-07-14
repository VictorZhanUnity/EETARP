using System.Collections.Generic;
using System.Linq;

namespace VictorDev.Common
{
    /// 原API類別功能擴充
    public static class ClassExtended
    {
        /// [Extended] - 首英文字大寫
        public static string ToCapitalizeFirstLetter(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return char.ToUpper(str[0]) + str.Substring(1);
        }

        /// [Extended] - List是否為Null或Empty
        public static bool IsNullOrEmpty<T>(this List<T> list) => list == null || list.Count == 0;
        
        /// [Extended] - 複製一份List
        public static List<T> Copy<T>(this List<T> list) => list.Select(x=>x).ToList();
    }
}