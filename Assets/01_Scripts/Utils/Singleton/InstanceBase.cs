using System;
using UnityEngine;

namespace Utils
{
    public class InstanceBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        // A script to inherit from to make a singleton

        public static T Instance { get; private set; }
        
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            
        }
    }
}