using System;
using System.Collections;
using Unity.Services.Vivox;
using UnityEngine;
using Utils;

namespace Network
{
    public class VivoxManager : PersistentNetworkSingleton<VivoxManager>
    {
        #region Serialize Fields
        
        //
        
        #endregion
        
        #region Fields
        
        private bool _hasJoinedChannel = false;
        private string channelName = "default";
        
        #endregion
        
        #region Methods
        
        #region Unity Methods
        
        //
        
        #endregion
        
        public async void LoginToVivoxAsync()
        {
            LoginOptions options = new LoginOptions();
            options.DisplayName = Unity.Netcode.NetworkManager.Singleton.LocalClientId == 0 ? "Tom-Tom" : "Nana";
            options.EnableTTS = true;
            await VivoxService.Instance.LoginAsync(options);
        }
        
        public async void JoinChannelAsync(string channelName)
        {
            try
            {
                this.channelName = channelName;
                //Leave any existing channel
                await VivoxService.Instance.LeaveAllChannelsAsync();
                
                await VivoxService.Instance.JoinPositionalChannelAsync(channelName, ChatCapability.AudioOnly, new Channel3DProperties(
                    audibleDistance: 32,
                    conversationalDistance: 1,
                    audioFadeModel: AudioFadeModel.InverseByDistance,
                    audioFadeIntensityByDistanceaudio: 1
                ));
                
                Debug.Log($"Joined channel: {channelName}");
                
                _hasJoinedChannel = true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to join channel {channelName}: {e}");
                StartCoroutine(RetryConnection());
                Debug.LogWarning("Retrying connection in 1 second...");
            }
        }
        
        private IEnumerator RetryConnection()
        {
            yield return new WaitForSeconds(1);
            JoinChannelAsync(channelName);
        }
        
        #endregion
    }
}