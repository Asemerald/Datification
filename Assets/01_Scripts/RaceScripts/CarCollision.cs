using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CarCollision : MonoBehaviour
{
    private RaceManager raceManager;

    private void Start()
    {
        if (NetworkManager.Singleton && !NetworkManager.Singleton.IsServer)
        {
            enabled = false;
        }
        
        raceManager = RaceManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Booster"))
        {
            GetComponentInParent<CarController>().Boost();
        }

        if (other.gameObject.CompareTag("RampTrigger"))
        {
            GetComponentInParent<CarInputs>().RampAnimationTrigger();
            raceManager.CoroutineEndOfRace();
        }

        if (other.gameObject.CompareTag("MiddleTrigger"))
        {
            GetComponentInParent<CarInputs>().StartEndingTrigger();
            raceManager.ApproachingSlide();
        }

        if (other.gameObject.CompareTag("RampZone"))
        {
            if (GetComponentInParent<CarController>().canLaunch)
            {
                GetComponentInParent<CarController>().rampZone++;
            }
            
        }
    }
}
