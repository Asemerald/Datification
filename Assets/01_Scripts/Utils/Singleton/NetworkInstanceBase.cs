using System;
using Unity.Netcode;
using UnityEngine;

namespace Utils
{
    public class NetworkInstanceBase<T> : NetworkBehaviour where T : NetworkBehaviour
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