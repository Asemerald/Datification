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

        private string connectionCode = "";
        private EventHandler OnCodeSubmitEvent;
        
        #endregion

        #region Methods
        
        #region Unity Methods

        protected void Awake()
        {
            // Ensure the input field updates text in uppercase without causing an infinite loop.
            ConnectionCodeInputField.onValueChanged.AddListener(OnInputFieldChanged);
            
            // On submit, try to connect to the server.
            ConnectionCodeInputField.onSubmit.AddListener( () =>
            {
                OnSubmit();
            });
            
            // add listener on submit that calls OnSubmit and invokes OnCodeSubmitEvent
            ConnectionCodeInputField.onSubmit.AddListener((string codeText) =>
            {
                // Call the OnSubmit method.
                OnSubmit(codeText);
                
                // Invoke the event.
                OnCodeSubmitEvent?.Invoke(this, EventArgs.Empty);
            });
        }

        private void Start()
        {
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
                
            }
        }
        
        /// <summary>
        /// Bunch of checks to ensure the code is valid then tries to connect to the server.
        /// </summary>
        /// <param name="code">The code to join the server</param>
        private void OnSubmit(string code)
        {
            // If code is not 6 characters long, return.
            if (code.Length != 6) return;
            
            // 
            
        }

        public string GetCode()
        {
            return connectionCode;
        }
        
        #endregion
    }
}