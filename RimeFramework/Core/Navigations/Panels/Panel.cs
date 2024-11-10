using UnityEngine;

namespace RimeFramework.Core
{
    /// <summary>
    /// 面板
    /// </summary>
    /// <b> Note: 面板属于IPool也就是池对象，配合Pools使用
    /// <remarks>Author: AstoraGray</remarks>
    public abstract class Panel : MonoBehaviour,IPool
    {
        public abstract string Group { get; } // 所属导航组
        /// <summary>
        /// 显示面板
        /// </summary>
        public virtual void Show()
        {
            gameObject.transform.SetAsLastSibling();
            gameObject.SetActive(true);
        }
        /// <summary>
        /// 隐藏面板
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}