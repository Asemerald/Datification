using Unity.Netcode;
using UnityEngine;

namespace Utils
{
    public class PersistentNetworkSingleton<T> : BaseNetworkSingleton<T> where T : NetworkBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                return BaseInstance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = Instance;
                DontDestroyOnLoad(this);
            }
            else
            {
#if UNITY_EDITOR
                if (SHOW_DEBUG)
                {
                    Debug.Log($"This scene already have a instance to {typeof(T)}. Deleting duplicates.");
                }
#endif
                Destroy(gameObject);
            }
        }
    }
}