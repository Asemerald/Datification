using System;
using System.Collections;
using System.Threading.Tasks;
using Cinemachine;
using Game.Customisation;
using Unity.Netcode;
using UnityEngine;
using Utils;

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
            
            
            UnityMainThread.wkr.AddJob(() =>
            {
                Debug.Log("ShowCarServerRpc");
                // Find objects with car tag and make them child of CustomisationManager.Instance.mainCarParent
                var cars = GameObject.FindGameObjectsWithTag("Car");
                foreach (var halfCars in cars)
                {
                    halfCars.transform.SetParent(ServerBehaviour.Instance.maincar.gameObject.transform);
                    
                    // reset position and rotation
                    halfCars.transform.localPosition = Vector3.zero;
                    halfCars.transform.localRotation = Quaternion.identity;
                    
                
                }
                
                StartCoroutine(SpawnRaceCarCall());
            });
        }
        
        [ClientRpc]
        private void ShowCarClientRpc()
        {
            ShowCar();
        }
        
        private async void ShowCar()
        {
           await UnityMainThread.wkr.AddJobAsync(async () =>
           {
               CustomisationManager.Instance.showRoomCamera.gameObject.SetActive(true);
               CustomisationManager.Instance.rideau.SetActive(false);

               await Task.Delay(5000);
               
               
               SpawnRace();

           });
        }

        private IEnumerator SpawnRaceCarCall()
        {
            yield return new WaitForSeconds(5);
            ServerBehaviour.Instance.SpawnRaceCar();
        }


    }
    
    
}
