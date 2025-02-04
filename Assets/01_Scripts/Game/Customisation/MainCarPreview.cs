using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCarPreview : MonoBehaviour
{
    // -0.25 -3 4
    
    [SerializeField] Vector3 startPosition;
    
    private bool raceStarted = false;
    
    void Start()
    {
        transform.position = startPosition;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        // If i have a parent and race has not started, disable the animation components
        if (transform.parent != null && !raceStarted)
        {
            DisableAnimationComponents();
        }
    }

    private void DisableAnimationComponents()
    {
        if (raceStarted) return;
        raceStarted = true;
        UnityMainThread.wkr.AddJob(() =>
        {
            
            var animationComponent = GetComponent<Animator>();
            animationComponent.enabled = false;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        });
    }
}
