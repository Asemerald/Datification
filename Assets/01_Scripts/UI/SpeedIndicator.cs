using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class SpeedIndicator : MonoBehaviour
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
    
    private void Update()
    {
        //playerAccelerationState = AccelerationState.Normal;
         currentSpeed = controller.currentSpeed;
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

