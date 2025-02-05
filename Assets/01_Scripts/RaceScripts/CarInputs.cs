using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarInputs : NetworkBehaviour
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
    public Animator animCar;

    [HideInInspector] public bool secondStage;
    [HideInInspector] public  bool startEnding;

    private CarController controller;
    private float startDragY;

    private void Start()
    {
        if (NetworkManager.Singleton && !NetworkManager.Singleton.IsServer)
        {
            enabled = false;
            return;
        }
        
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
        if (NetworkManager.Singleton)
        {
            if (GameManager.Instance.leftPlayerInput.Value && GameManager.Instance.rightPlayerInput.Value)
            {
                if (activeState != States.allActive)
                {
                    SwitchFX();
                    activeState = States.allActive;
                }
            }
            else if (!GameManager.Instance.leftPlayerInput.Value && GameManager.Instance.rightPlayerInput.Value)
            {
                if (activeState != States.rightActive)
                {
                    SwitchFX();
                    activeState = States.rightActive;
                }
            }
            else if (GameManager.Instance.leftPlayerInput.Value && !GameManager.Instance.rightPlayerInput.Value)
            {
                if (activeState != States.leftActive)
                {
                    SwitchFX();
                    activeState = States.leftActive;
                }
            }
            else
            {
                if (activeState != States.noActive)
                {
                    SwitchFX();
                    activeState = States.noActive;
                }
            }
        
            anim.SetInteger("States", (int)activeState);
            return;
        }

        // Handle offline
        
        if (left && right)
        {
            if (activeState != States.allActive)
            {
                SwitchFX();
                activeState = States.allActive;
            }
        }
        else if (!left && right)
        {
            if (activeState != States.rightActive)
            {
                SwitchFX();
                activeState = States.rightActive;
            }
        }
        else if (left && !right)
        {
            if (activeState != States.leftActive)
            {
                SwitchFX();
                activeState = States.leftActive;
            }
        }
        else
        {
            if (activeState != States.noActive)
            {
                SwitchFX();
                activeState = States.noActive;
            }
        }
        
        anim.SetInteger("States", (int)activeState);
    }

    private void SwitchFX()
    {
        //Audio call
        AudioManager.Instance.PlaySound(8, 0.5f);
        
        if (left)
        {
            controller.EnableLeftFX();
        }
        else
        {
            controller.DisableLeftFX();
        }

        if (right)
        {
            controller.EnableRightFX();
        }
        else
        {
            controller.DisableRightFX();
        }
    }

    public void StartEndingTrigger()
    {
        startEnding = true;
        controller.canLaunch = true;
        animCar.SetTrigger("Ending");
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
