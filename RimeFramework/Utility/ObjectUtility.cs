using System;

namespace RimeFramework.Utility
{
    public static class ObjectUtility
    {
        public static string GetKey(this Object obj)
        {
            return $"{obj.GetType()}-{obj.GetHashCode()}";
        }
    }
}