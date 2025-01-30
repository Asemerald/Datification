using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private CarInputs inputs;

    [Space(15f)] 
    [Header("Car settings")] 
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accelMultiplier = 2;
    [SerializeField] private float decelMultiplier = 3;
    [HideInInspector] public float currentSpeed;
    private float targetSpeed;
    private float baseSpeed;
    
    [Space(15f)]
    [SerializeField] private float boosterSpeedAdded = 10f;
    [SerializeField] private ParticleSystem boostPs;
    private bool canBoost;

    [Header("Cameras")] 
    [SerializeField] private CinemachineVirtualCamera mainCam;
    [SerializeField] private CinemachineVirtualCamera rampCam;
    
    [Header("Ramp")] 
    public int rampZone;

    
    private void Start()
    {
        rampCam.enabled = false;
        
        canBoost = true;
        baseSpeed = maxSpeed;
    }

    private void Update()
    {
        if (!inputs.secondStage)
        {
            RaceUpdate();
        }
        
    }

    private void RaceUpdate()
    {
        targetSpeed = maxSpeed;
        
        if (inputs.activeState == CarInputs.States.allActive)
        {
            maxSpeed += Time.deltaTime * accelMultiplier;
        }
        else if (inputs.activeState == CarInputs.States.noActive)
        {
            maxSpeed -= Time.deltaTime * decelMultiplier;
        }

        maxSpeed = Mathf.Clamp(maxSpeed, baseSpeed, 100);

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 5 * Time.deltaTime);
        
        transform.Translate(Vector3.forward * (currentSpeed * Time.deltaTime));
    }

    public void Boost()
    {
        if (!canBoost) return;
        
        canBoost = false;
        maxSpeed += boosterSpeedAdded;
        targetSpeed += boosterSpeedAdded;
        boostPs.Play();
            
        StartCoroutine(BoosterCooldown());
    }

    private IEnumerator BoosterCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canBoost = true;
    }

    public void SwitchCameras()
    {
        rampCam.enabled = true;
    }
    
    
    public void LaunchRamp()
    {
        
    }
}
