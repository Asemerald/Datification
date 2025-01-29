using System;
using System.Threading.Tasks;
using UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Vivox;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

namespace Network
{
    public class RelayManager : PersistentNetworkSingleton<RelayManager>
    {
        #region Serialize Fields
        
        //
        
        #endregion
        
        #region Fields
        
        public string JoinCode { get; private set; }
        public event EventHandler<EventArgs> OnRelayFullEvent;
        
        #endregion

        #region Methods

        
        #region Unity Methods

        protected override void OnAwake()
        {
            base.OnAwake();
            NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
            {
                if (!IsServer) return;
                if (NetworkManager.Singleton.ConnectedClientsList.Count > 1)
                {
                    OnRelayFullEvent?.Invoke(this, EventArgs.Empty);
                }
            };
        }
        
        void Start()
        {
        }

        private void Update()
        {
        }

        #endregion
        
        /// <summary>
        /// Crée le serveur
        /// </summary>
        /// <returns>
        /// <para>0 = Success</para>
        /// <para>1 = Error</para>
        /// </returns>
        public async Task<int> CreateRelayAsync()
        {
            try
            {
                // Create a Relay allocation
                LoadingUI.Instance.SetLoadingText("Creating Relay...");
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);

                // Get the join code
                LoadingUI.Instance.SetLoadingText("Getting Join Code...");
                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                JoinCode = joinCode;
                Debug.Log($"Relay created with join code: {joinCode}");

                string host = allocation.RelayServer.IpV4; 
                ushort port = (ushort)allocation.RelayServer.Port; 
                byte[] joinAllocationId = allocation.AllocationIdBytes; 
                byte[] connectionData = allocation.ConnectionData;
                byte[] hostConnectionData = allocation.ConnectionData;
                byte[] key = allocation.Key;
                bool isSecure = false;
            
                foreach (var endpoint in allocation.ServerEndpoints)
                {
                    if (endpoint.ConnectionType == "dtls")
                    {
                        host = endpoint.Host;
                        port = (ushort)endpoint.Port;
                        isSecure = endpoint.Secure;
                    }
                }
            
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(host,
                    port,
                    joinAllocationId,
                    connectionData,
                    hostConnectionData,
                    key,
                    isSecure));
                
                NetworkManager.Singleton.StartHost();
                
                LoadingUI.Instance.SetLoadingText("Joining Voice Channel...");
                await VivoxManager.Instance.JoinChannelAsync(joinCode);
                
                return 0;
            }
            catch (RelayServiceException e)
            {
                Debug.LogError($"Failed to create Relay: {e}");
                //TODO Reload the scene or something
                return 1;
            }
        }
        
        
        #endregion
        
        /// <summary>
        /// Attempts to join a relay using the provided join code.
        /// </summary>
        /// <param name="joinCode">The relay room code.</param>
        /// <returns>
        /// <para>0 = Success</para>
        /// <para>1 = Error</para>
        /// </returns>

        public async Task<int> JoinRelayAsync(string joinCode)
        {
            try
            {
                Debug.Log($"Attempting to join Relay with code: {joinCode}");

                // Join an existing Relay allocation
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

                string host = joinAllocation.RelayServer.IpV4;
                ushort port = (ushort)joinAllocation.RelayServer.Port;
                byte[] joinAllocationId = joinAllocation.AllocationIdBytes;
                byte[] connectionData = joinAllocation.ConnectionData;
                byte[] hostConnectionData = joinAllocation.HostConnectionData;
                byte[] key = joinAllocation.Key;
                bool isSecure = false;

                foreach (var endpoint in joinAllocation.ServerEndpoints)
                {
                    if (endpoint.ConnectionType == "dtls")
                    {
                        host = endpoint.Host;
                        port = (ushort)endpoint.Port;
                        isSecure = endpoint.Secure;
                    }
                }

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(host,
                    port, joinAllocationId, connectionData, hostConnectionData, key, isSecure));
                NetworkManager.Singleton.StartClient();
                
                await VivoxManager.Instance.JoinChannelAsync(joinCode);
                
                return 0;
            }
            catch (RelayServiceException e)
            {
                // Handle not found by creating a new Relay
                switch (e.Reason)
                {
                    case RelayExceptionReason.JoinCodeNotFound:
                        //TODO do something 
                        return 1;

                    case RelayExceptionReason.AllocationNotFound:
                        return 1;

                    // Handle other specific error codes as needed
                    default:
                        Debug.LogError($"Failed to join Relay: {e}");
                        return 1;
                }
            }

        }
        
    }
}