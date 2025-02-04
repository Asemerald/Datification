using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCarPreview : MonoBehaviour
{
    // -0.25 -3 4
    
    [SerializeField] Vector3 startPosition;

    
    void Start()
    {
        transform.position = startPosition;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        
    }

    public void DisableAnimationComponents()
    {
        UnityMainThread.wkr.AddJob(() =>
        {
            var animationComponent = GetComponent<Animator>();
            animationComponent.enabled = false;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        });
    }
}
