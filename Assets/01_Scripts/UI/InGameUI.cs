using System;
using Network;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class InGameUI : InstanceBase<InGameUI>
    {
        #region SerializeField

        [Header("InGame UI")]
        [SerializeField] private Button StartButton; 
        

        #endregion
        
        #region Fields

        public GameObject EGameObject;
        
        #endregion
    
        #region Methods
    
        #region Unity Methods
    
        private void Start()
        {
            EGameObject = gameObject;

            StartButton?.onClick.AddListener(OnStartButtonClicked);

            if (StartButton != null)
            {
                UnityMainThread.wkr.AddJob(() =>
                {
                    StartButton.gameObject.SetActive(false);
                });
            }
            
            Hide();
        }

        private void OnEnable()
        {
            
        }

        #endregion
        
        public void OnStartButtonClicked()
        {
            GameManager.Instance.StartGame();
        }
        
        public void ShowStartButton()
        {
           UnityMainThread.wkr.AddJob(() =>
           {
               StartButton.gameObject.SetActive(true);
           });
        }
        
        public void Show()
        {
            UnityMainThread.wkr.AddJob(() =>
            {
                EGameObject.SetActive(true);
            });
        }
        
        public void Hide()
        {
            UnityMainThread.wkr.AddJob(() =>
            {
                EGameObject.SetActive(false);
            });
        }
    
        #endregion
    }
}
