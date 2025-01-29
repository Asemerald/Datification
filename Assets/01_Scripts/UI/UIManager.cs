using System;
using System.Threading.Tasks;
using Network;
using Unity.Services.Relay;
using UnityEngine;
using Utils;
using Utils.Event;

namespace UI
{
    public class UIManager : LocalSingleton<UIManager>
    {
        # region Methods
        
        #region Unity Methods

        private void Awake()
        {
            EditConnectionCode.Instance.OnCodeSubmitEvent += OnJoinCodeSubmit_OnCodeSubmitEvent;
            ConnectionUI.Instance.OnCreateRoomEvent += OnCreateButtonClicked_OnCreateRoomEvent;
            Authentificate.Instance.OnAuthentificateSuccess += DeactivateLoadingScreen_OnAuthentificateSuccess;
        }

        #endregion
        
        private async void OnJoinCodeSubmit_OnCodeSubmitEvent(object sender, StringEventArgs args)
        {
            ConnectionUI.Instance.Hide();
            LoadingUI.Instance.Show();
            var returnCode = await RelayManager.Instance.JoinRelayAsync(args.String);
            switch (returnCode)
            {
                case 0: // Success
                    LoadingUI.Instance.Hide();
                    InGameUI.Instance.Show();
                    break;
                case 1: // Error
                    LoadingUI.Instance.SetLoadingText("An error occurred");
                    await WaitDelay.Instance.WaitFor(2).ContinueWith(_ =>
                    {
                        LoadingUI.Instance.Hide();
                        ConnectionUI.Instance.Show();
                    });
                    break;
            }
        }
        
        private async void OnCreateButtonClicked_OnCreateRoomEvent(object sender, EventArgs args)
        {
            ConnectionUI.Instance.Hide();
            LoadingUI.Instance.Show();
            switch (await RelayManager.Instance.CreateRelayAsync())
            {
                case 0: // Success
                    LoadingUI.Instance.Hide();
                    InGameUI.Instance.Show();
                    break;
                case 1: // Unknown error
                    LoadingUI.Instance.SetLoadingText("An error occurred");
                    await WaitDelay.Instance.WaitFor(2).ContinueWith(_ =>
                    {
                        LoadingUI.Instance.Hide();
                        ConnectionUI.Instance.Show();
                    });
                    break;
            }
        }
        
        private void DeactivateLoadingScreen_OnAuthentificateSuccess(object sender, EventArgs args)
        {
            LoadingUI.Instance.Hide();
            ConnectionUI.Instance.Show();
        }
        
        #endregion
    }
}
