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
    public class InGameUI : LocalSingleton<InGameUI>
    {
        #region SerializeField

        [Header("InGame UI")]
        [SerializeField] private Button StartButton; 
        

        #endregion
        
        #region Fields

        private GameObject EGameObject;
        
        #endregion
    
        #region Methods
    
        #region Unity Methods
    
        private void Start()
        {
            EGameObject = gameObject;

            StartButton?.onClick.AddListener(OnStartButtonClicked);

            if (StartButton != null) StartButton.gameObject.SetActive(false);
            
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
            StartButton.gameObject.SetActive(true);
        }
        
        public void Show()
        {
            EGameObject.SetActive(true);
        }
        
        public void Hide()
        {
            EGameObject.SetActive(false);
        }
    
        #endregion
    }
}
