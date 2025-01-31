using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollision : MonoBehaviour
{
    private RaceManager raceManager;

    private void Start()
    {
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
            GetComponentInParent<CarController>().rampZone++;
        }
    }
}
