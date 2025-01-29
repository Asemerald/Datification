using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float baseSpeed;
    [SerializeField] private float maxSpeed;
    private float currentSpeed;

    private void Update()
    {
        currentSpeed = baseSpeed;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        transform.Translate(Vector3.forward * (currentSpeed * Time.deltaTime));
    }
}
