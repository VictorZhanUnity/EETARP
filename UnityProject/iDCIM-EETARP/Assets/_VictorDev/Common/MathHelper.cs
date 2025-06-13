using UnityEngine;

namespace VictorDev.MathUtils
{
    /// 數學工具
    public static class MathHelper
    {
        /// [字串]轉換成小數點後N位
        public static string ToDotNumberString(float value, int n = 1) =>
            value.ToString((n > 0) ? $"0.{new string('#', n)}" : "");

        /// [float值]轉換成小數點後N位
        public static float ToDotNumberFloat(float value, int n = 1) =>
            Mathf.Round(value * Mathf.Pow(10, n)) / Mathf.Pow(10, n);
        
        /// [float值]轉換成0~1的百分比數值
        public static float ToPercent01(float value, float maxValue = 100, int n = 1) =>
            ToDotNumberFloat(value / maxValue, n);
    }
}