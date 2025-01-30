using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Event;

namespace UI
{
    public class ConnectionUI : LocalSingleton<ConnectionUI>
    {
        #region Serialized Fields
    
        [Header("Connection UI")]
        [SerializeField] private Button createButton;
        [SerializeField] private TMP_InputField ConnectionCodeInputField;
    
        #endregion
        
        #region Fields
        
        public event EventHandler<EventArgs> OnCreateRoomEvent;
        private string connectionCode = "";
        public event EventHandler<StringEventArgs> OnCodeSubmitEvent;
        
        #endregion

        private void Awake()
        {
            createButton.onClick.AddListener(() =>
            {
                OnCreateRoomEvent?.Invoke(this, EventArgs.Empty);
            });
            
            // Ensure the input field updates text in uppercase without causing an infinite loop.
            ConnectionCodeInputField.onValueChanged.AddListener(OnInputFieldChanged);
            
            // Ensure the input field submits the code when the user presses enter.
            ConnectionCodeInputField.onSubmit.AddListener(OnSubmit);
            
            Hide();
        }
        
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
        /// Check the code then invoke the event to notify the connection code has been submitted.
        /// </summary>
        /// <param name="code">The code to join the server</param>
        private void OnSubmit(string code)
        {
            Debug.Log("Code submitted: " + code);
            // If code is not 6 characters long, return.
            if (code.Length != 6) return;
            
            // Invoke the event to notify the connection code has been submitted with the code as argument.
            var eventArgs = new StringEventArgs(code);
            Debug.Log("Event args: " + eventArgs.String);
            OnCodeSubmitEvent?.Invoke(this, eventArgs);
            
        }

        public string GetCode()
        {
            return connectionCode;
        }
    }
}