using System.Collections;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
     public static RaceManager Instance;
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
     
     
     public RaceStep currentRaceStep;
     
     [Header("Elements")]
     [SerializeField] private CarController carController;
     [SerializeField] private GameObject trafficLights;
     [SerializeField] private GameObject slideIndicator;
     [SerializeField] private GameObject finalScorePanel;
     [SerializeField] private GameObject endRaceButton;
     
     [Space,Header("Timings")]
     [SerializeField] private float trafficLightsEndTime = .5f;
     [SerializeField] private float finalJumpDuration = 2f;
     
     [Space,Header("Debug")]
     [SerializeField] private bool debugOn;
     
     //Singleton Okazou
     private void Awake()
     {
          if(Instance != null && Instance != this) Destroy(this);
          Instance = this;
     }
     private void Start()
     {
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
          DebugCurrentRaceStep();
          carController.enabled = false;
          yield return new WaitForSeconds(1f);
          currentRaceStep = RaceStep.StartRace;
     }
     IEnumerator StartRace()
     {
          DebugCurrentRaceStep();
          
          trafficLights.SetActive(true);
          yield return new WaitForSeconds(trafficLightAnimDuration-trafficLightsEndTime);
          carController.enabled = true;
          currentRaceStep = RaceStep.DuringRace;
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
          finalScorePanel.SetActive(true);
          endRaceButton.SetActive(true);
     }

     #endregion

     private void DebugCurrentRaceStep()
     {
          if (debugOn) Debug.Log(currentRaceStep);
     }
}
