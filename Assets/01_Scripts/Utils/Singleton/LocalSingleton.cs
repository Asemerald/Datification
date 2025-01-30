using UnityEngine;

namespace Utils
{
    public class LocalSingleton<T> : BaseSingleton<T> where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                return BaseInstance;
            }
        }
        
        public virtual void Hide()
        {
            BaseInstance.gameObject.SetActive(false);
        }
        
        public virtual void Show()
        {
            BaseInstance.gameObject.SetActive(true);
        }
    }
}