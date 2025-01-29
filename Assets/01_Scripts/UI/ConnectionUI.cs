using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class ConnectionUI : LocalSingleton<ConnectionUI>
    {
        #region Serialized Fields
    
        [Header("Connection UI")]
        [SerializeField] private Button createButton;
    
        #endregion
        
        #region Fields
        
        public event EventHandler<EventArgs> OnCreateRoomEvent;
        
        #endregion

        private void Awake()
        {
            createButton.onClick.AddListener(() =>
            {
                OnCreateRoomEvent?.Invoke(this, EventArgs.Empty);
            });
            
            Hide();
        }
    }
}