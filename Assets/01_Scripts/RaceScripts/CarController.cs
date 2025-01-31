using System.Collections;
using Cinemachine;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private CarInputs inputs;

    [Space(15f)] 
    [Header("Car settings")] 
    [SerializeField] private float maxSpeed = 100;
    [SerializeField] private float baseSpeed = 30;
    private float accelSpeed;
    [SerializeField] private float accelMultiplier = 2;
    [SerializeField] private float decelMultiplier = 3;
    
    [HideInInspector] public float currentSpeed;
    private float targetSpeed;
    
    
    [Space(15f)]
    [SerializeField] private float boosterSpeedAdded = 10f;
    [SerializeField] private ParticleSystem boostPs;
    private bool canBoost;

    [Header("Cameras")] 
    [SerializeField] private CinemachineVirtualCamera mainCam;
    [SerializeField] private CinemachineVirtualCamera rampCam;
    
    [Header("Ramp")] 
    public int rampZone;
    public bool canLaunch;

    [Header("Characters")] 
    [SerializeField] private Animator animatorJ1;
    [SerializeField] private Animator animatorJ2;

    
    private void Start()
    {
        rampCam.enabled = false;
        rampZone = 0;
        
        canBoost = true;
    }

    private void Update()
    {
        if (!inputs.secondStage)
        {
            RaceUpdate();
        }
        else
        {
            CheckRampZone();
        }
        
    }

    private void RaceUpdate()
    {
        accelSpeed = baseSpeed + 10;

        if (inputs.activeState == CarInputs.States.noActive)
        {
            targetSpeed = baseSpeed;
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, decelMultiplier * Time.deltaTime);
        }
        else
        {
            targetSpeed = accelSpeed;
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, accelMultiplier * Time.deltaTime);
        }

        targetSpeed = Mathf.Clamp(targetSpeed, 0, maxSpeed);
        
        transform.Translate(Vector3.forward * (currentSpeed * Time.deltaTime));
    }

    private void CheckRampZone()
    {
        if (rampZone == 4 && canLaunch)
        {
            Debug.Log("Lancement raté");
            DisplayZone("Lancement raté !");
            canLaunch = false;
            TriggerEndAnimation();
        }
    }

    public void Boost()
    {
        if (!canBoost) return;
        
        canBoost = false;
        baseSpeed += boosterSpeedAdded;
        boostPs.Play();
        
        inputs.animCar.SetTrigger("Boost");
        
        animatorJ1.SetTrigger("isBoosting");
        animatorJ2.SetTrigger("isBoosting");
        
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
        if (!canLaunch) return;
        
        switch (rampZone)
        {
            //Partager cette valeur sur le serveur et
            // 1. vérifier lequel des deux joueurs à la meilleure valeur
            // 2. faire la moyenne des deux valeurs
            
            case 1 : 
                Debug.Log("Lancement moyen");
                RaceManager.Instance.typeOfLaunchString = "Lancement moyen !";
                canLaunch = false;
                TriggerEndAnimation();
                break;
            case 2 : 
                Debug.Log("Bon Lancement");
                DisplayZone("Bon Lancement !");
                canLaunch = false;
                TriggerEndAnimation();
                break;
            case 3 : 
                Debug.Log("Lancement parfait");
                DisplayZone("Lancement parfait !");
                canLaunch = false;
                TriggerEndAnimation();
                break;
            default :
                break;
        }
    }

    private void DisplayZone(string displayedText)
    {
        RaceManager.Instance.typeOfLaunchString = displayedText;
    }

    private void TriggerEndAnimation()
    {
        animatorJ1.SetBool("raceOver",true);
        animatorJ2.SetBool("raceOver",true);
    }
}
