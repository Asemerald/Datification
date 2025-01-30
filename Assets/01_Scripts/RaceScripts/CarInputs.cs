using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool left, right;

    [SerializeField] private Animator anim;

    [HideInInspector] public bool secondStage;
    private bool startEnding;

    private CarController controller;

    private void Start()
    {
        controller = GetComponent<CarController>();
    }

    void Update()
    {
        if (startEnding)
        {
            left = true;
            right = true;
        }
        else
        {
            left = Input.GetKey(KeyCode.LeftArrow);
            right = Input.GetKey(KeyCode.RightArrow);
        }

        
        if (Input.GetKeyUp(KeyCode.UpArrow) && secondStage)
        {
            controller.LaunchRamp();
        }
        
        

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
    }
    
    public void RampAnimationTrigger()
    {
        anim.SetTrigger("Ramp");
        
        //Set animation speed equals to currentSpeed;
        anim.speed = controller.currentSpeed / 100;
        
        controller.SwitchCameras();
        
        secondStage = true;
    }
}
