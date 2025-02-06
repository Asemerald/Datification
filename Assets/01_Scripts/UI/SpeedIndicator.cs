using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class SpeedIndicator : NetworkBehaviour
{

    [SerializeField] private Image speedIndicatorImage;
    
    private float currentSpeed;
    private float _maxSpeed = 100;
    private float _minSpeed = 0;
    
    [SerializeField] private float acceleration = 1f ;

    [SerializeField] private CarController controller;
    
    private enum AccelerationState
    {
        Normal,
        Boost,
        Obstacle
    }
    [SerializeField] private AccelerationState playerAccelerationState;

    [SerializeField] private TMP_Text speedText;
    
    private bool carSpawned = false;
    
    private void Start()
    {
        StartCoroutine(TryToGetCarController());
    }
    
    private IEnumerator TryToGetCarController()
    {
        // check if there is a car controller in the scene, if not wait for 1 second and try again
        while (controller == null)
        {
            controller = FindObjectOfType<CarController>();
            yield return new WaitForSeconds(0.5f);
        }
        
        carSpawned = true;
    }
    
    private void Update()
    {
        if (!carSpawned) return;
        
        //playerAccelerationState = AccelerationState.Normal;
        if (NetworkManager.Singleton)
        {
            if (controller.speedBeforeRampNetVar.Value == 0)
            {
                currentSpeed = controller.currentSpeed;
            
                // set the netvar if in network
                if (NetworkManager.Singleton)
                {
                    currentSpeed = controller.currentSpeedNetVar.Value;
                }
            }
            else
            {
                currentSpeed = controller.speedBeforeRamp;
            
                // set the netvar if in network
                if (NetworkManager.Singleton)
                {
                    currentSpeed = controller.speedBeforeRampNetVar.Value;
                }
            }
        }
        else
        {
            if (controller.speedBeforeRamp == 0)
            {
                currentSpeed = controller.currentSpeed;
            
                // set the netvar if in network
                if (NetworkManager.Singleton)
                {
                    currentSpeed = controller.currentSpeedNetVar.Value;
                }
            }
            else
            {
                currentSpeed = controller.speedBeforeRamp;
            
                // set the netvar if in network
                if (NetworkManager.Singleton)
                {
                    currentSpeed = controller.speedBeforeRampNetVar.Value;
                }
            }
        }
         
        speedText.text = $"{Mathf.RoundToInt((currentSpeed * 2.5f))}km/h"; 
        
    
        UpdateIndicatorPosition();
    }
    private void FixedUpdate()
    {
       /* switch (playerAccelerationState)
        {
            case AccelerationState.Normal:
                Accelerate();
                break;
            case AccelerationState.Boost: 
                //Boost 
                break;
            case AccelerationState.Obstacle: 
                //Decelerate 
                break;
        }*/
    }

    private void Accelerate()
    {
        currentSpeed += acceleration;
        currentSpeed = Mathf.Clamp(currentSpeed, _minSpeed, _maxSpeed);
        
        Debug.Log(currentSpeed);
    }
    private void Decelerate()
    {
        currentSpeed -= acceleration;
        currentSpeed = Mathf.Clamp(currentSpeed, _minSpeed, _maxSpeed);
        
        Debug.Log(currentSpeed);
    }

    private void UpdateIndicatorPosition()
    {
        float newPos  = Mathf.Lerp(-355, 355, currentSpeed/100);

        speedIndicatorImage.rectTransform.anchoredPosition = new Vector2(newPos, speedIndicatorImage.rectTransform.anchoredPosition.y);
    }
}

