using System;
using System.Threading.Tasks;
using Game;
using Network;
using Utils;
using Utils.Event;
using Unity.Netcode;
using UnityEngine;


namespace UI
{
    public class UIManager : NetworkInstanceBase<UIManager>
    {

        #region Fields

        private bool uiDisabled = false;

        #endregion
        
        
        # region Methods
        
        #region Unity Methods

        private void Start()
        {
            
            ConnectionPanelUI.Instance.OnCodeSubmitEvent += OnJoinCodeSubmit_OnCodeSubmitEvent;
            ConnectionPanelUI.Instance.OnCreateRoomEvent += OnCreateButtonClicked_OnCreateRoomEvent;
            Authentificate.Instance.OnAuthentificateSuccess += DeactivateLoadingScreen_OnAuthentificateSuccess;
            NetworkManager.Singleton.OnClientConnectedCallback += SingletonOnOnClientConnectedCallback;
        }

        private void Update()
        {
            if (!NetworkManager.Singleton.IsHost) return;
            if (NetworkManager.Singleton.ConnectedClients.Count == 2 && !uiDisabled)
            {
                SingletonOnOnClientConnectedCallback();
                uiDisabled = true;
            }
        }

        private void SingletonOnOnClientConnectedCallback(ulong obj = 0)
        {
            if (!NetworkManager.Singleton.IsHost) return;
            if (NetworkManager.Singleton.ConnectedClientsList.Count < 2)
            {
                ServerBehaviour.Instance.SpawnGameManager_OnRelayJoined();
                return;
            }
            GameManager.Instance.ClientJoinedServerRpc();
            DeactivateLoadingScreen_OnRelayFullEvent(this, EventArgs.Empty);
            uiDisabled = true;

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
                    InGameUI.Instance.Show();
                    LoadingUI.Instance.Hide();
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
            uiDisabled = true;
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
