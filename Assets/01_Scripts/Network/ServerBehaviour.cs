using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Utils;

public class ServerBehaviour : LocalNetworkSingleton<ServerBehaviour>
{
    #region Serialize Fields
    
    [Header("To Instantiate")]
    [SerializeField] private NetworkObject GameManager;
    
    #endregion

    private void Awake()
    {
        if (!IsServer) return;
        
        //var gameManager = NetworkManager.SpawnManager.InstantiateAndSpawn(GameManager);
    }
}
