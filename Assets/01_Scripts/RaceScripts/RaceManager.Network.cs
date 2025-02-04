using Unity.Netcode;
using UnityEngine;

public partial class RaceManager
{
    private bool clientConnected = false;
    
    [ServerRpc (RequireOwnership = false)]
    private void StartRaceServerRpc()
    {
        StartRaceClientRpc();
    }
    
    [ClientRpc]
    private void StartRaceClientRpc()
    {
        clientConnected = true;
    }
    
    private void CheckClientConnection()
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            Debug.LogWarning("Client connected");
            StartRaceServerRpc();
        }
    }
    
}
