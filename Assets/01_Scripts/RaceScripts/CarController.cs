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
    
    [SerializeField] private float accelMultiplier = 2;
    
    public float currentSpeed;
    private float targetSpeed;
    
    
    [Space(15f)]
    [SerializeField] private float boosterSpeedAdded = 10f;
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

    [Header("FX")] 
    [SerializeField] private ParticleSystem fxAccelLeft;
    [SerializeField] private ParticleSystem fxAccelRight;
    [Space]
    [SerializeField] private ParticleSystem fxWheelLeft;
    [SerializeField] private ParticleSystem fxWheelRight;
    [Space]
    [SerializeField] private ParticleSystem fxBoostLeft; 
    [SerializeField] private ParticleSystem fxBoostRight;
    
    private void Start()
    {
        rampCam.enabled = false;
        rampZone = 0;
        
        canBoost = true;
        targetSpeed = baseSpeed;
        
        //setup FX
        DisableAllFX();
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
        if (inputs.startEnding)
        {
            targetSpeed = 50;
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 1f * Time.deltaTime);
            
            Debug.Log("Change FOV");
        }
        else
        {
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

    public void EnableRightFX()
    {
        fxAccelRight.Play(true);
        fxWheelRight.Play(true);
    }
    public void EnableLeftFX()
    {
        fxAccelLeft.Play(true);
        fxWheelLeft.Play(true);
    }
    
    public void DisableRightFX()
    {
        fxAccelRight.Clear(true);
        fxWheelRight.Clear(true);
        
        fxAccelRight.Stop(true);
        fxWheelRight.Stop(true);
    }
    public void DisableLeftFX()
    {
        fxAccelLeft.Clear(true);
        fxWheelLeft.Clear(true);
        
        fxAccelLeft.Stop(true);
        fxWheelLeft.Stop(true);
    }

    private void DisableAllFX()
    {
        DisableLeftFX();
        DisableRightFX();
        
        fxBoostLeft.Clear(true);
        fxBoostRight.Clear(true);
        
        fxBoostLeft.Stop(true);
        fxBoostRight.Stop(true);
    }
    

    public void Boost()
    {
        if (!canBoost) return;
        
        canBoost = false;
        targetSpeed += boosterSpeedAdded;
        
        //particles
        
        fxBoostLeft.Play();
        fxBoostRight.Play();
        
        inputs.animCar.SetTrigger("Boost");
        
        animatorJ1.SetTrigger("isBoosting");
        animatorJ2.SetTrigger("isBoosting");
        
        StartCoroutine(BoosterCooldown());
    }

    private IEnumerator BoosterCooldown()
    {
        yield return new WaitForSeconds(0.3f);
        canBoost = true;
        yield return new WaitForSeconds(0.7f);
        fxBoostLeft.Stop();
        fxBoostRight.Stop();
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
