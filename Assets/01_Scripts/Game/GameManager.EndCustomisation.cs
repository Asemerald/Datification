using System;
using Cinemachine;
using Game.Customisation;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public partial class GameManager
    {
        
        
        private void CheckEndCustomisation()
        {
            if (hostFinishedCustomisation.Value && clientFinishedCustomisation.Value)
            {
                ShowCarServerRpc();
            }
        }
        
        [ServerRpc]
        private void ShowCarServerRpc()
        {
            ShowCarClientRpc();
        }
        
        [ClientRpc]
        private void ShowCarClientRpc()
        {
            ShowCar();
        }
        
        private void ShowCar()
        {
            UnityMainThread.wkr.AddJob(() =>
            {
                CustomisationManager.Instance.showRoomCamera.gameObject.SetActive(true);
                CustomisationManager.Instance.showRoomCamera.Priority = 20;
                CustomisationManager.Instance.rideau.SetActive(false);
            });
        }


    }
    
    
}
