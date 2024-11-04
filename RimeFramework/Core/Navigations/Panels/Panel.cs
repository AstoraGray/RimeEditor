using UnityEngine;

namespace RimeFramework.Core
{
    public abstract class Panel : MonoBehaviour,IPool
    {
        public abstract string Group { get; }
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}