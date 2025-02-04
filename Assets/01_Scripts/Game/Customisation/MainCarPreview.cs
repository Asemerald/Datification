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
    }

    private void Update()
    {
        
    }
}
