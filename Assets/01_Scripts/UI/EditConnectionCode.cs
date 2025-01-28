using System;
using TMPro;
using UnityEngine;
using Utils;

namespace UI
{
    public class EditConnectionCode : LocalSingleton<EditConnectionCode>
    {
        
        #region Serialized Fields
        [Header("UI Elements")]
        [SerializeField] private TMP_InputField ConnectionCodeInputField;
        
        #endregion
        
        #region Fields
        
        public event EventHandler OnCodeChanged;
        private string connectionCode = "";
        
        #endregion

        #region Methods
        
        #region Unity Methods

        protected void Awake()
        {
            // Ensure the input field updates text in uppercase without causing an infinite loop.
            ConnectionCodeInputField.onValueChanged.AddListener(OnInputFieldChanged);
            //On click on mobile, the keyboard will appear
            ConnectionCodeInputField.onSelect.AddListener((string value) =>
            {
                //Spawns the keyboard
                TouchScreenKeyboard.Open(value);
            });
        }

        private void Start()
        {
            OnCodeChanged += EditConnectionCode_OnCodeChanged;
        }
        
        #endregion

        private void OnInputFieldChanged(string newValue)
        {
            // Convert input to uppercase.
            string upperValue = newValue.ToUpper();

            // Update the connection code only if there's a difference.
            if (connectionCode != upperValue)
            {
                connectionCode = upperValue;

                // Update the input field text if necessary to reflect the transformation.
                ConnectionCodeInputField.text = upperValue;

                // Fire the event to notify listeners.
                OnCodeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void EditConnectionCode_OnCodeChanged(object sender, EventArgs e)
        {
            // Handle the event if needed.
        }

        public string GetCode()
        {
            return connectionCode;
        }
        
        #endregion
    }
}