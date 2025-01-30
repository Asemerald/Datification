using System;
using TMPro;
using UnityEngine;
using Utils;

namespace UI
{
    public class LoadingUI : LocalSingleton<LoadingUI>
    {
        #region Serialized Fields
        
        [Header("Loading UI")]
        [SerializeField] private TMP_Text loadingText;
        [SerializeField] private TMP_Text loadingDetailsText;
        [SerializeField] private TMP_Text joinCodeText;
        
        
        #endregion

        #region Fields

        private GameObject EGameObject;

        #endregion
        
        #region Methods
        
        #region Unity Methods
        
        private void Start()
        {
            EGameObject = gameObject;
            OnDisable();
            SetLoadingText("Signing in...");
        }

        private void OnDisable()
        {
            SetLoadingText("");
            SetJoinCodeText("");
            SetLoadingDetailsText("");
        }

        #endregion
        
        public void SetLoadingText(string text)
        {
            loadingText.text = text;
        }
        
        public void SetJoinCodeText(string text)
        {
            joinCodeText.text = text;
        }
        
        public void SetLoadingDetailsText(string text)
        {
            loadingDetailsText.text = text;
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