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
        
        public void Hide()
        {
            BaseInstance.gameObject.SetActive(false);
        }
        
        public void Show()
        {
            BaseInstance.gameObject.SetActive(true);
        }
    }
}