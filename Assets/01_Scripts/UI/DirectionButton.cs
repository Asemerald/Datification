using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class DirectionButton : NetworkBehaviour, IPointerUpHandler, IPointerDownHandler
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
            if (NetworkManager.Singleton)
            {
                GameManager.Instance.SetInputServerRpc(false, true);
                return;
            }
            
            //right
            inputs.right = false;
        }
        else
        {
            if (NetworkManager.Singleton)
            {
                GameManager.Instance.SetInputServerRpc(false, false);
                return;
            }
            
            //left
            inputs.left = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (leftOrRight)
        {
            if (NetworkManager.Singleton)
            {
                GameManager.Instance.SetInputServerRpc(true, true);
                return;
            }
            
            //right
            inputs.right = true;
        }
        else
        {
            if (NetworkManager.Singleton)
            {
                GameManager.Instance.SetInputServerRpc(true, false);
                return;
            }
            
            //left
            inputs.left = true;
        }
    }
}

