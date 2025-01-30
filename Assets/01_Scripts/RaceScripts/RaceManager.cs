using System.Collections;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
     private RaceManager Instance;
     
     private enum RaceStep
     {
          BeforeStart,
          StartRace,
          DuringRace,
          ApproachingJump,
          EndOfRace
     }

     private int currentStepIndex;
     
     [SerializeField] private RaceStep currentRaceStep;

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
     }
     private void Update()
     {
          //Triggers the coroutine of the current step
          switch (currentRaceStep)
          {
               case RaceStep.BeforeStart:
                    if (currentStepIndex == 0)
                    {
                         //Start logique
                         currentStepIndex++;
                    }
                    break;
               case RaceStep.StartRace:
                    if (currentStepIndex == 1)
                    {
                         //Start logique
                         currentStepIndex++;
                    }
                    break;
               case RaceStep.DuringRace:
                    if (currentStepIndex == 2)
                    {
                         //Start lgique
                         currentStepIndex++;
                    }
                    break;
               case RaceStep.ApproachingJump:
                    if (currentStepIndex == 3)
                    {
                         //Start lgique
                         currentStepIndex++;
                    }
                    break;
               case RaceStep.EndOfRace:
                    if (currentStepIndex == 4)
                    {
                         //Start lgique
                         currentStepIndex++;
                    }
                    break;
          }
     }

     #region Steps logic Details

     IEnumerator BeforeStart()
     {
          //StopCar + Wait for Loading
          yield return new WaitForSeconds(1f);
          currentRaceStep = RaceStep.StartRace;
     }
     IEnumerator StartRace()
     {
          //Start Traffic Lights + Give car controle
          yield return new WaitForSeconds(1f);
          currentRaceStep = RaceStep.StartRace;
     }
     IEnumerator DuringRace()
     {
          //Wait for Slide
          yield return new WaitForSeconds(1f);
          currentRaceStep = RaceStep.ApproachingJump;
     }
     IEnumerator ApproachingSlide()
     {
          //Give slide control to players + Wait for end of jump
          yield return new WaitForSeconds(1f);
          currentRaceStep = RaceStep.EndOfRace;
     }
     IEnumerator EndOfRace()
     {
          //trigger Score Apparition then trigger exit button
          yield return new WaitForSeconds(1f);
     }

     #endregion
}
