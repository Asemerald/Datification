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
        
        #endregion
        
        #region Methods
        
        #region Unity Methods
        
        private void Awake()
        {
            SetLoadingText("Logging in...");
        }

        #endregion
        
        public void SetLoadingText(string text)
        {
            loadingText.text = text;
        }
        
        #endregion
    }
}