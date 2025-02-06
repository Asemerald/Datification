using System;
using System.Collections;
using System.Threading.Tasks;
using Cinemachine;
using Game.Customisation;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            Debug.LogError("ShowCar");
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
                
                StartCoroutine(GoToRaceScene());
            });
        }
        
        [ClientRpc]
        private void ShowCarClientRpc()
        {
            ShowCar();
        }
        
        private void ShowCar()
        {
            UnityMainThread.wkr.AddJobAsync(async () =>
            {
                AudioManager.Instance.PlaySound(4);
                
                //add delay 

                await Task.Delay(1500);
                
                //appel de l'animation pour ouvrir les rideaux
                CustomisationManager.Instance.rideau.GetComponent<Animation>().Play();
            });
        }

        private IEnumerator GoToRaceScene()
        {
            yield return new WaitForSeconds(5);
            
            Debug.LogError("Go");
            // Go to Race Scene
            NetworkManager.Singleton.SceneManager.LoadScene("RaceScene", LoadSceneMode.Single);
        }


    }
    
    
}
