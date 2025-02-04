using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Utils;

public class ServerBehaviour : NetworkInstanceBase<ServerBehaviour>
{
    #region Serialize Fields
    
    [Header("To Instantiate")]
    [SerializeField] private NetworkObject GameManager;
    [SerializeField] private NetworkObject mainCarPrefab;
    
    #endregion

    public NetworkObject maincar;

    protected void Start()
    {
        Application.targetFrameRate = 120;
    }

    public void SpawnGameManager_OnRelayJoined()
    {
        if (!IsServer) return;
        
        Debug.Log("ServerBehaviour Awake");
        var gameManager = NetworkManager.SpawnManager.InstantiateAndSpawn(GameManager);
        maincar = NetworkManager.SpawnManager.InstantiateAndSpawn(mainCarPrefab);
    }
    
    
}
