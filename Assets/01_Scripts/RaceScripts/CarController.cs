using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float baseSpeed;
    [SerializeField] private float maxSpeed;
    private float currentSpeed;

    [SerializeField] private float boosterSpeedAdded = 10f;
    private bool canBoost;
    [SerializeField] private ParticleSystem boostPs;

    private void Start()
    {
        currentSpeed = baseSpeed;
        canBoost = true;
    }

    private void Update()
    {
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        transform.Translate(Vector3.forward * (currentSpeed * Time.deltaTime));
    }

    public void Boost()
    {
        if (!canBoost) return;
        
        Debug.Log("Boost");
        canBoost = false;
        currentSpeed += boosterSpeedAdded;
        boostPs.Play();
            
        StartCoroutine(BoosterCooldown());
    }

    private IEnumerator BoosterCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canBoost = true;
    }
}
