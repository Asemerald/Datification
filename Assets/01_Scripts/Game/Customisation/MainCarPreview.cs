using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCarPreview : MonoBehaviour
{
    // -0.25 -3 4
    
    [SerializeField] Vector3 startPosition;

    private bool changed;
    void Start()
    {
        transform.position = startPosition;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "RaceScene" && !changed)
        {
            changed = true;
            DisableAnimationComponents();
        }

    }

    public void DisableAnimationComponents()
    {
        UnityMainThread.wkr.AddJob(() =>
        {
            var animationComponent = GetComponent<Animator>();
            animationComponent.enabled = false;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            
            if (NetworkManager.Singleton && NetworkManager.Singleton.IsServer)
            {
                // find car with raceCartas
                var cars = GameObject.FindGameObjectWithTag("raceCar").GetComponent<NetworkObject>();
                
                GetComponent<NetworkObject>().TrySetParent(cars, false);
                
            }
        });
    }
}
