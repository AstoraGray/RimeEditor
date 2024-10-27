namespace RimeFramework.Utility
{
    public static class StringUtility
    {
        public static string Extract(this string str,string strExt)
        {
            int index = str.IndexOf(strExt);

            if (index >= 0)
            {
                return str.Substring(index + strExt.Length);
            }
            else
            {
                return "未找到指定标记";
            }
        }
    }
}