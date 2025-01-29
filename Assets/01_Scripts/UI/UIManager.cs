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
        }

        #endregion
        
        private async void OnJoinCodeSubmit_OnCodeSubmitEvent(object sender, StringEventArgs args)
        {
            ConnectionUI.Instance.Hide();
            LoadingUI.Instance.Show();
            switch (await RelayManager.Instance.JoinRelayAsync(args.String))
            {
                case 0: // Success
                    LoadingUI.Instance.Hide();
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
        
        #endregion
    }
}
