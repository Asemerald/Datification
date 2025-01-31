using System;
using Game;
using Network;
using TMPro;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class InGameUI : InstanceBase<InGameUI>
    {
        #region SerializeField
        
        [Header("InGame UI")]
        [SerializeField] private Button startButton; 
        [SerializeField] private Button nextButton;
        

        #endregion
        
        #region Fields

        public GameObject EGameObject;
        
        #endregion
    
        #region Methods
    
        #region Unity Methods
    
        private void Start()
        {
            EGameObject = gameObject;
            
            ShowStartButton(false);
            ShowNextButton(false);
            
            Hide();
        }

        private void OnEnable()
        {
            
        }

        #endregion

        public void ShowNextButton(bool show)
        {
            UnityMainThread.wkr.AddJob(() =>
            {
                nextButton.gameObject.SetActive(show);
            });
        }
        
        public void OnNextButtonClicked()
        {
            GameManager.Instance.NextCustomStage();
        }
        
        public void OnStartButtonClicked()
        {
            GameManager.Instance.StartGame();
            ShowStartButton(false);
        }
        
        public void ShowStartButton(bool show)
        {
           UnityMainThread.wkr.AddJob(() =>
           {
               startButton.gameObject.SetActive(show);
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
