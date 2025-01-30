using System;
using System.Threading.Tasks;
using Network;
using Utils;
using Utils.Event;
using Unity.Netcode;


namespace UI
{
    public class UIManager : LocalNetworkSingleton<UIManager>
    {
        # region Methods
        
        #region Unity Methods

        private void Start()
        {
            
            ConnectionPanelUI.Instance.OnCodeSubmitEvent += OnJoinCodeSubmit_OnCodeSubmitEvent;
            ConnectionPanelUI.Instance.OnCreateRoomEvent += OnCreateButtonClicked_OnCreateRoomEvent;
            Authentificate.Instance.OnAuthentificateSuccess += DeactivateLoadingScreen_OnAuthentificateSuccess;
            NetworkManager.Singleton.OnConnectionEvent += ((manager, data) =>
            {
                if (!IsHost) return;
                if (NetworkManager.Singleton.ConnectedClientsList.Count < 2) return;
                DeactivateLoadingScreen_OnRelayFullEvent(this, EventArgs.Empty);

            } );
        }

        #endregion
        
        private async void OnJoinCodeSubmit_OnCodeSubmitEvent(object sender, StringEventArgs args)
        {
            ConnectionPanelUI.Instance.Hide();
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
                    LoadingUI.Instance.SetLoadingDetailsText("Please try again");
                    await WaitDelay.Instance.WaitFor(2).ContinueWith(_ =>
                    {
                        LoadingUI.Instance.Hide();
                        ConnectionPanelUI.Instance.Show();
                    });
                    break;
            }
        }
        
        private async void OnCreateButtonClicked_OnCreateRoomEvent(object sender, EventArgs args)
        {
            ConnectionPanelUI.Instance.Hide();
            LoadingUI.Instance.Show();
            switch (await RelayManager.Instance.CreateRelayAsync())
            {
                case 0: // Success
                    if (!IsServer) break;
                    
                    LoadingUI.Instance.SetJoinCodeText("Join Code: " + RelayManager.Instance.JoinCode);
                    LoadingUI.Instance.SetLoadingText("Room created");
                    LoadingUI.Instance.SetLoadingDetailsText("Waiting for player");
                    break;
                case 1: // Unknown error
                    LoadingUI.Instance.SetLoadingText("An error occurred");
                    LoadingUI.Instance.SetLoadingDetailsText("Please try again");
                    await WaitDelay.Instance.WaitFor(2).ContinueWith(_ =>
                    {
                        LoadingUI.Instance.Hide();
                        ConnectionPanelUI.Instance.Show();
                    });
                    break;
            }
        }
        
        private void DeactivateLoadingScreen_OnAuthentificateSuccess(object sender, EventArgs args)
        {
            LoadingUI.Instance.Hide();
            ConnectionPanelUI.Instance.Show();
        }
        
        private async void DeactivateLoadingScreen_OnRelayFullEvent(object sender, EventArgs args)
        {
            LoadingUI.Instance.SetLoadingText("Player Connected");
            LoadingUI.Instance.SetLoadingDetailsText("Starting game...");
            await WaitDelay.Instance.WaitFor(2).ContinueWith(_ =>
            {
                LoadingUI.Instance.Hide();
                InGameUI.Instance.Show();
                InGameUI.Instance.ShowStartButton();
            });
        }
        
        #endregion
    }
}
