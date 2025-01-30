using System;
using System.Collections;
using System.Collections.Generic;
using Game.Customisation;
using Prefabs;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

public class GameManager : NetworkInstanceBase<GameManager>
{
    
    #region Fields
    
    public LevelsScriptable currentLevel;
    public NetworkVariable<string> levelName = new("Level 1");
    
    #endregion
    
    #region Methods

    #region Unity Methods

    public override void OnNetworkSpawn()
    {
        InitGame();
    }

    #endregion
    private void InitGame()
    {
        if (IsServer)
        {
            currentLevel = CustomisationManager.Instance.SelectRandomLevel();
            levelName.Value = currentLevel.name;
        }
    }

    private void GetDataFromServer()
    {
        if (IsClient)
        {
            CustomisationManager.Instance.GetLevelByName(levelName.Value);
        }
    }
    
    public void StartGame()
    {
        if (IsServer)
        {
            StartGameServerRpc();
        }
    }
    
    [ServerRpc]
    private void StartGameServerRpc()
    {
        StartGameClientRpc();
    }
    
    [ClientRpc]
    private void StartGameClientRpc()
    {
        if (IsServer && IsHost) return;
        GetDataFromServer();
    }
    
    #endregion

}
