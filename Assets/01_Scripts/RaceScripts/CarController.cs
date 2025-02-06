using System.Collections;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class CarController : NetworkBehaviour
{
    [SerializeField] private CarInputs inputs;

    [Space(15f)] 
    [Header("Car settings")] 
    [SerializeField] private float maxSpeed = 100;
    [SerializeField] private float baseSpeed = 30;
    
    [SerializeField] private float accelMultiplier = 2;
    
    public float currentSpeed;
    private float targetSpeed;

    public float speedBeforeRamp;
    
    
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
        //Spawn 
        if (NetworkManager.Singleton)
        {
            SpawnServerRpc();
        }
        
        rampCam.Priority = 5;
        rampZone = 0;
        
        canBoost = true;
        targetSpeed = baseSpeed;
        
        //setup FX
        DisableAllFX();

        
        if (NetworkManager.Singleton && !NetworkManager.Singleton.IsServer)
        {
            return;
        }
        
        // transform position = 0 1 -250
        transform.position = new Vector3(0, 1, -250);
    }
    
    [ServerRpc]
    private void SpawnServerRpc()
    {
        NetworkObject.Spawn();
        
        // Spawn all childrens
        /*foreach (NetworkObject child in transform)
        {
            child.Spawn();
        }*/
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
        if (NetworkManager.Singleton && !NetworkManager.Singleton.IsServer)
        {
            return;
        }
        
        if (inputs.startEnding)
        {
            targetSpeed = 50;
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 1f * Time.deltaTime);
            
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
        
        //Audio call
        AudioManager.Instance.PlaySound(9, 0.5f);
        
        // return if online and client
        if (NetworkManager.Singleton && !NetworkManager.Singleton.IsServer)
        {
            return;
        }
        
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
        rampCam.Priority = 11;
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
                RaceManager.Instance.typeOfLaunchString = "Lancement moyen !";
                canLaunch = false;
                TriggerEndAnimation();
                break;
            case 2 : 
                DisplayZone("Bon Lancement !");
                canLaunch = false;
                TriggerEndAnimation();
                break;
            case 3 : 
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
