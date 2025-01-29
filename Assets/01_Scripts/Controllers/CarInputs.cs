using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputs : MonoBehaviour
{
    private enum States
    {
        noActive,
        leftActive,
        rightActive,
        //allActive
    }

    [SerializeField] private States activeState;
    private bool left, right;

    [SerializeField] private Animator anim;
    
    void Update()
    {
        left = Input.GetKey(KeyCode.LeftArrow);
        right = Input.GetKey(KeyCode.RightArrow);

        if (left && right)
        {
            //activeState = States.allActive;
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
}
