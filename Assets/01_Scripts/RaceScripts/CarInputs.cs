using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarInputs : MonoBehaviour
{
    public enum States
    {
        noActive,
        leftActive,
        rightActive,
        allActive
    }

    public States activeState;
    [HideInInspector] public bool left, right;

    [SerializeField] private Animator anim;

    [HideInInspector] public bool secondStage;
    private bool startEnding;

    private CarController controller;

    [Header("Inputs")]
    [SerializeField] private Button leftButton, rightButton;
    private float startDragY;

    private void Start()
    {
        controller = GetComponent<CarController>();
    }

    void Update()
    {
        if (RaceManager.Instance.currentRaceStep == RaceManager.RaceStep.BeforeStart || RaceManager.Instance.currentRaceStep == RaceManager.RaceStep.StartRace)
            return;
        
        CheckDrag();
        
        if (startEnding)
        {
            left = true;
            right = true;
        }
        /*else
        {
            left = Input.GetKey(KeyCode.LeftArrow);
            right = Input.GetKey(KeyCode.RightArrow);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) && secondStage)
        {
            controller.LaunchRamp();
        }*/
        
        

        if (left && right)
        {
            activeState = States.allActive;
        }
        else if (!left && right)
        {
            activeState = States.rightActive;
        }
        else if (left && !right)
        {
            activeState = States.leftActive;
        }
        else
        {
            activeState = States.noActive;
        }
        
        anim.SetInteger("States", (int)activeState);
    }

    public void StartEndingTrigger()
    {
        startEnding = true;
        controller.canLaunch = true;
    }
    
    public void RampAnimationTrigger()
    {
        anim.SetTrigger("Ramp");
        
        //Set animation speed equals to currentSpeed;
        anim.speed = controller.currentSpeed / 100;
        
        controller.SwitchCameras();
        
        secondStage = true;
    }

    private void CheckDrag()
    {
        if (Input.touchCount == 1) // One finger for dragging
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
            
            
            if (touch.phase == TouchPhase.Began)
            {
                startDragY = touchPosition.y;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (touchPosition.y > startDragY && secondStage)
                {
                    Debug.Log("Launch !");
                    controller.LaunchRamp();
                }
            }
        }
    }
}
