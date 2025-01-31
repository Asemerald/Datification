using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DirectionButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private CarInputs inputs;

    [SerializeField] private bool leftOrRight;
    private void Start()
    {
        inputs = FindObjectOfType<CarInputs>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (leftOrRight)
        {
            //right
            inputs.right = false;
        }
        else
        {
            //left
            inputs.left = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (leftOrRight)
        {
            //right
            inputs.right = true;
        }
        else
        {
            //left
            inputs.left = true;
        }
    }
}
