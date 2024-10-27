using UnityEngine;

namespace RimeFramework.Utility
{
    public static class ColorUtility
    {
        public static string ToHex(this Color color)
        {
            // 将颜色的RGB值转换为0-255范围的整数
            int r = Mathf.RoundToInt(color.r * 255);
            int g = Mathf.RoundToInt(color.g * 255);
            int b = Mathf.RoundToInt(color.b * 255);
            int a = Mathf.RoundToInt(color.a * 255);
        
            // 格式化为十六进制字符串
            return $"#{r:X2}{g:X2}{b:X2}{a:X2}";
        }
    }
}