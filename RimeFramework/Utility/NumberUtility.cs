namespace RimeFramework.Utility
{
    public static class NumberUtility
    {
        public static int GetSign(this float y)
        {
            if (y > 0)
            {
                return 1;
            }
            if (y < 0)
            {
                return -1;
            }
            return 0;
        }
    }
}