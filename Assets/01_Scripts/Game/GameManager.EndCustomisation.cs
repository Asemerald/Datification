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
                // Find objects with car tag and make them child of CustomisationManager.Instance.mainCarParent
                
                CustomisationManager.Instance.mainCarParent.SetActive(true);
                
                var cars = GameObject.FindGameObjectsWithTag("Car");
                foreach (var car in cars)
                {
                    car.transform.SetParent(CustomisationManager.Instance.mainCarParent.transform);
                }
                
                CustomisationManager.Instance.showRoomCamera.gameObject.SetActive(true);
                CustomisationManager.Instance.rideau.SetActive(false);
            });
        }


    }
    
    
}
