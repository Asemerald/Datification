using System;
using System.Threading.Tasks;
using Network;
using Unity.Services.Relay;
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
        }

        #endregion
        
        private async void OnJoinCodeSubmit_OnCodeSubmitEvent(object sender, StringEventArgs args)
        {
            ConnectionUI.Instance.Hide();
            LoadingUI.Instance.Show();
            switch (await RelayManager.Instance.JoinRelayAsync(args.String))
            {
                case 0: // Success
                    
                    break;
                case 1: // Room does not exist
                    //Create room
                    LoadingUI.Instance.SetLoadingText("Creating room...");
                    await RelayManager.Instance.CreateRelayAsync();
                    LoadingUI.Instance.Hide();
                    break;
                case 2: // Unknown error
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
