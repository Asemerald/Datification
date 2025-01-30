using System;
using Network;
using TMPro;
using UnityEngine;
using Utils;

namespace UI
{
    public class InGameUI : LocalSingleton<InGameUI>
    {
        #region SerializeField

        [Header("InGame UI")]
        [SerializeField] private GameObject inGameUI; // TODO : Add InGame UI elements
        

        #endregion
    
        #region Methods
    
        #region Unity Methods
    
        private void Awake()
        {
            Hide();
        }

        private void OnEnable()
        {
            
        }

        #endregion
        
        
    
        #endregion
    }
}
