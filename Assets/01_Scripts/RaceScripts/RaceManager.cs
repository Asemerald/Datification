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
     private bool debugOn;
     private float trafficLightAnimDuration;
     
     
     public RaceStep currentRaceStep;
     
     [Header("Elements")]
     [SerializeField] private CarController carController;
     [SerializeField] private GameObject trafficLights;
     
     [Space,Header("Timings")]
     [SerializeField] private float trafficLightsEndTime = 1.5f;
     [SerializeField] private float raceDuration;
     
     
     //Singleton Okazou
     private void Awake()
     {
          if(Instance != null && Instance != this) Destroy(this);
          Instance = this;
     }
     private void Start()
     {
          //Race Step Initialisation
          currentRaceStep = RaceStep.BeforeStart;

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
                         StartCoroutine(DuringRace());
                         currentStepIndex++;
                    }
                    break;
               case RaceStep.ApproachingJump:
                    if (currentStepIndex == 3)
                    {
                         StartCoroutine(ApproachingSlide());
                         currentStepIndex++;
                    }
                    break;
               case RaceStep.EndOfRace:
                    if (currentStepIndex == 4)
                    {
                         StartCoroutine(EndOfRace());
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
     IEnumerator DuringRace()
     {
          DebugCurrentRaceStep();
          yield return new WaitForSeconds(raceDuration);
          currentRaceStep = RaceStep.ApproachingJump;
     }
     IEnumerator ApproachingSlide()
     {
          DebugCurrentRaceStep();
          //Give slide control to players + Wait for end of jump
          yield return new WaitForSeconds(1f);
          currentRaceStep = RaceStep.EndOfRace;
     }
     IEnumerator EndOfRace()
     {
          DebugCurrentRaceStep();
          //trigger Score Apparition then trigger exit button
          yield return new WaitForSeconds(1f);
          
     }

     #endregion

     private void DebugCurrentRaceStep()
     {
          if (debugOn) Debug.Log(currentRaceStep);
     }
}
