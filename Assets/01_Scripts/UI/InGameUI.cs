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
        [SerializeField] private TMP_Text joinCodeText;

        private bool loadingFinished;

        #endregion
    
        #region Methods
    
        #region Unity Methods
    
        private void Awake()
        {
            RelayManager.Instance.OnRelayFullEvent += DeactivateCodeText_OnRelayFullEvent;
            
            Hide();
        }

        private void OnEnable()
        {
            joinCodeText.text = "Join Code: " + RelayManager.Instance.JoinCode;
        }

        #endregion
        
        private void DeactivateCodeText_OnRelayFullEvent(object sender, System.EventArgs e)
        {
            joinCodeText.gameObject.SetActive(false);
        }
    
        #endregion
    }
}
