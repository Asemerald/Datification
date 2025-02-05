using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public partial class RaceManager : NetworkInstanceBase<RaceManager>
{
     public enum RaceStep
     {
          BeforeStart,
          StartRace,
          DuringRace,
          ApproachingJump,
          EndOfRace
     }
     private int currentStepIndex;
     private float trafficLightAnimDuration;

     [SerializeField] private TMP_Text finalScoreText;
     [HideInInspector] public string typeOfLaunchString;
     [SerializeField] private AudioSource audioSource;
     
     
     public RaceStep currentRaceStep;
     
     [Header("Elements")]
     [SerializeField] private CarController carController;
     [SerializeField] private GameObject trafficLights;
     [SerializeField] private GameObject slideIndicator;
     [SerializeField] private GameObject finalScorePanel;
     [SerializeField] private GameObject endRaceButton;
     [SerializeField] private GameObject controlTouchScreenRight;
     [SerializeField] private GameObject controlTouchScreenLeft;
     
     [Space,Header("Timings")]
     [SerializeField] private float trafficLightsEndTime = .5f;
     [SerializeField] private float finalJumpDuration = 2f;
     
     [Space,Header("Debug")]
     [SerializeField] private bool debugOn;
     
     private void Start()
     {
         // Check if there is a network manager, if not return  
         if (!NetworkManager.Singleton)
         {
              Debug.LogWarning("Offline");
             StartCoroutine(WaitForInitialize());
             clientConnected = true;
             return;
         }
             
         
          // Check if the client is connected
          clientConnected = false;
          CheckClientConnection();
          
          // Wait for the client to connect
          StartCoroutine(WaitForInitialize());
     }

     private IEnumerator WaitForInitialize()
     {
          while (!clientConnected)  // Wait until Client connect
               yield return null;
          
          Debug.LogError("Client Connected");
          
          //Race Initialisation
          currentRaceStep = RaceStep.BeforeStart;
          trafficLights.SetActive(false);
          slideIndicator.SetActive(false);
          finalScorePanel.SetActive(false);
          endRaceButton.SetActive(false);
          controlTouchScreenRight.SetActive(false);
          controlTouchScreenLeft.SetActive(false);
          

          //Gets the duration of the traffic light animation to give the car control at the right timing
          trafficLightAnimDuration = trafficLights.GetComponent<Animation>().clip.length;
          
     }
     private void Update()
     {
          if (!clientConnected) return;
          
          //Triggers the coroutine of the current step
          switch (currentRaceStep)
          {
               case RaceStep.BeforeStart:
                    if (currentStepIndex == 0)
                    {
                         StartCoroutine(BeforeStart());
                         currentStepIndex++;
                    }
                    break;
               case RaceStep.StartRace:
                    if (currentStepIndex == 1)
                    {
                         StartCoroutine(StartRace());
                         currentStepIndex++;
                    }
                    break;
               case RaceStep.DuringRace:
                    if (currentStepIndex == 2)
                    {
                         currentStepIndex++;
                    }
                    break;
               case RaceStep.ApproachingJump:
                    if (currentStepIndex == 3)
                    {
                         currentStepIndex++;
                    }
                    break;
               case RaceStep.EndOfRace:
                    if (currentStepIndex == 4)
                    {
                         currentStepIndex++;
                    }
                    break;
          }
     }

     #region Steps logic Details

     IEnumerator BeforeStart()
     {
          //Audio
          audioSource.Play();
          
          DebugCurrentRaceStep();
          carController.enabled = false;
          
          /*APPARITION INDICATION TOUCH SCREEN J1 LEFT - A CHANGER EN FONCTION DU JOUEUR!
          controlTouchScreenLeft.SetActive(true);
          yield return new WaitForSeconds(2f);
          controlTouchScreenLeft.SetActive(false);*/
          
          //APPARITION INDICATION TOUCH SCREEN J2 RIGHT 
          controlTouchScreenRight.SetActive(true);
          yield return new WaitForSeconds(2f);
          controlTouchScreenRight.SetActive(false);
          
          currentRaceStep = RaceStep.StartRace;
     }
     IEnumerator StartRace()
     {
          DebugCurrentRaceStep();
          
          trafficLights.SetActive(true);
          yield return new WaitForSeconds(trafficLightAnimDuration-trafficLightsEndTime);
          currentRaceStep = RaceStep.DuringRace;
          
          if (!NetworkManager.Singleton || NetworkManager.Singleton.IsHost)
          {
               carController.enabled = true;
          }
     }
     private void DuringRace()
     {
          DebugCurrentRaceStep();
     }
     public void ApproachingSlide()
     {
          DebugCurrentRaceStep();
          slideIndicator.SetActive(true);
     }
     public void CoroutineEndOfRace()
     {
          StartCoroutine(EndOfRace());
     }
     IEnumerator EndOfRace()
     {
          DebugCurrentRaceStep();
          slideIndicator.SetActive(false);
          yield return new WaitForSeconds(finalJumpDuration);
          finalScoreText.text = $"{typeOfLaunchString} \r\n Score : {ScoreFormula()}m";
          finalScorePanel.SetActive(true);
          endRaceButton.SetActive(true);
          
          //Audio
          audioSource.Stop();

     }

     #endregion

     private void DebugCurrentRaceStep()
     {
          if (debugOn) Debug.Log(currentRaceStep);
     }

     private int ScoreFormula()
     {
          //customisation
          float customizationMult = 1;

          //race speed 
          float raceSpeed = Mathf.RoundToInt(carController.speedBeforeRamp) * 2.5f;
          

          //ramp timing
          float rampScore = 0;
          switch (carController.rampZone)
          {
               case 1 :
                    rampScore = 1.5f;
                    break;
               case 2 : 
                    rampScore = 2;
                    break;
               case 3 : 
                    rampScore = 3;
                    break;
               default :
                    rampScore = 1;
                    break;
          }
          
          return Mathf.RoundToInt(customizationMult * raceSpeed * rampScore);
     }
     public void RestartScene()
     {
          SceneManager.LoadScene(0);
     }
}
