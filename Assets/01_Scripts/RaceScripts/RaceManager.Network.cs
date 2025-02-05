using Unity.Netcode;
using UnityEngine;

public partial class RaceManager
{
    public bool clientConnected = false;
    
    [ServerRpc (RequireOwnership = false)]
    private void StartRaceServerRpc()
    {
        StartRaceClientRpc();
    }
    
    [ClientRpc]
    private void StartRaceClientRpc()
    {
        clientConnected = true;
        
        // get the gameobject with tag car
        var car = GameObject.FindGameObjectWithTag("Car");
        if (car != null)
        {
            CarController controller = car.AddComponent<CarController>();
            carController = controller;
            carController.enabled = true;
        }
    }
    
    private void CheckClientConnection()
    {
        switch (NetworkManager.Singleton.IsHost)
        {
            case true:
                ServerBehaviour.Instance.SpawnRaceCar();
                break;
            case false:
                Debug.LogWarning("Client connected");
                StartRaceServerRpc();
                break;
        }
    }
    
}
