using System.Collections;
using System.Threading.Tasks;
using Game;
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
     public NetworkVariable<int> typeOfLaunchStringNetVar = new NetworkVariable<int>(0);
     [SerializeField] private AudioSource audioSource;
     
     
     public RaceStep currentRaceStep;
     
     [Header("Elements")]
     [SerializeField] private CarController carController;
     [SerializeField] private GameObject trafficLights;
     [SerializeField] private GameObject slideIndicator;
     [SerializeField] private GameObject finalScorePanel;
     [SerializeField] private GameObject endRaceButton;
     [SerializeField] private GameObject speedIndicator;
     [SerializeField] private Animator controlTouchScreenRight;
     [SerializeField] private Animator controlTouchScreenLeft;
     
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
          
          UnityMainThread.wkr.AddJobAsync(async() =>
          {
               if (GameManager.Instance.hasRightCar)
               {
                    controlTouchScreenRight.gameObject.SetActive(true);
                    controlTouchScreenRight.enabled = true;
                    controlTouchScreenLeft.gameObject.SetActive(false);
                    
               }
               else
               {
                    controlTouchScreenLeft.gameObject.SetActive(true);
                    controlTouchScreenLeft.enabled = true;
                    controlTouchScreenRight.gameObject.SetActive(false);
               }
               
               await Task.Delay(2000);
               
               controlTouchScreenRight.gameObject.SetActive(false);
               controlTouchScreenLeft.gameObject.SetActive(false);
               
               currentRaceStep = RaceStep.StartRace;
          });
          
          
          yield return null;
     }
     IEnumerator StartRace()
     {
          DebugCurrentRaceStep();
          
          trafficLights.SetActive(true);
          yield return new WaitForSeconds(trafficLightAnimDuration-trafficLightsEndTime);
          currentRaceStep = RaceStep.DuringRace;
          
          /*if (!NetworkManager.Singleton || NetworkManager.Singleton.IsHost)
          {
               carController.enabled = true;
          }*/
          
          carController.enabled = true;
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
          if (!NetworkManager.Singleton)
          {
               finalScoreText.text = $"{typeOfLaunchString} \r\n Score : {ScoreFormula()}m";
          }
          else
          {
               switch (typeOfLaunchStringNetVar.Value)
               {
                    case 1:
                         finalScoreText.text = $"Pas mal ! \r\n Score : {ScoreFormula()}m";
                         break;
                    case 2:
                         finalScoreText.text = $"Très bien ! \r\n Score : {ScoreFormula()}m";
                         break;
                    case 3:
                         finalScoreText.text = $"Impressionnant ! \r\n Score : {ScoreFormula()}m";
                         break;
                    default:
                         finalScoreText.text = $"À revoir... \r\n Score : {ScoreFormula()}m";
                         break;
               }
          }
          finalScorePanel.SetActive(true);
          endRaceButton.SetActive(true);
          speedIndicator.SetActive(false);
          
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
          float raceSpeed;

          if (NetworkManager.Singleton)
          {
               //race speed 
               raceSpeed = Mathf.RoundToInt(carController.speedBeforeRampNetVar.Value) * 2.5f;
          }
          else
          {
               //race speed 
               raceSpeed = Mathf.RoundToInt(carController.speedBeforeRamp) * 2.5f;
          }
          

          //ramp timing
          float rampScore = 0;
          if (NetworkManager.Singleton)
          {
               int betterRampZone;
               // make betterrampzon the max value between rampzoneclient and rampzoneserver
               if (carController.rampScoreClient.Value == 3 || carController.rampScoreHost.Value == 3)
               {
                    betterRampZone = 3;
               }
               else if(carController.rampScoreClient.Value == 2 || carController.rampScoreHost.Value == 2)
               {
                    betterRampZone = 2;
               }
               else if (carController.rampScoreClient.Value == 1 || carController.rampScoreHost.Value == 1)
               {
                    betterRampZone = 1;
               }
               else if (carController.rampScoreClient.Value == 4 || carController.rampScoreHost.Value == 4)
               {
                    betterRampZone = 4;
               }
               else
               {
                    betterRampZone = 0;
               }
               
               switch (betterRampZone)
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
          }
          else
          {
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
          }
          
          return Mathf.RoundToInt(customizationMult * raceSpeed * rampScore);
     }
     public void RestartScene()
     {
          //NetworkManager.Singleton.SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
     }
}
