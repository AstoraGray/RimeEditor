using System;

namespace RimeFramework.Core
{
    /// <summary>
    /// 监控值
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <remarks>Author: AstoraGray</remarks>
    public struct ObsValue<T> where T : struct
    {
        public T Value
        {
            set
            {
                bool isChanged = !_value.Equals(value);
                _value = value;
                if (isChanged)
                {
                    OnValueChanged?.Invoke(value);
                }
            }
            get => _value;
        }

        public event Action<T> OnValueChanged;
        
        private T _value;
    }
}