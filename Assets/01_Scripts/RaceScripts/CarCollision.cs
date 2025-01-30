using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Booster"))
        {
            GetComponentInParent<CarController>().Boost();
        }

        if (other.gameObject.CompareTag("RampTrigger"))
        {
            GetComponentInParent<CarInputs>().RampAnimationTrigger();
            
        }

        if (other.gameObject.CompareTag("MiddleTrigger"))
        {
            GetComponentInParent<CarInputs>().StartEndingTrigger();
        }

        if (other.gameObject.CompareTag("RampZone"))
        {
            GetComponentInParent<CarController>().rampZone++;
        }
    }
}
