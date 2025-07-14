using System.Collections.Generic;
using System.Linq;

namespace VictorDev.Common
{
    /// 原API類別功能擴充
    public static class ClassExtended
    {
        #region >>> String <<<
        /// [Extended] - 首英文字大寫
        public static string ToCapitalizeFirstLetter(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return char.ToUpper(str[0]) + str.Substring(1);
        }
        #endregion

        #region >>> List <<<
        /// [Extended] - List是否為Null或Empty
        public static bool IsNullOrEmpty<T>(this List<T> list) => list == null || list.Count == 0;
       
        /// [Extended] - 複製一份List
        public static List<T> Copy<T>(this List<T> list) => list.Select(x=>x).ToList();
        
        /// [Extended] - 以separator隔開，將數組全部列出來
        public static string PrintAll<T>(this List<T> list, string separator =",") => string.Join(separator, list);
        #endregion
    }
}