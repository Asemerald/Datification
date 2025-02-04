using System.Collections;
using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;

public class DebugUi : MonoBehaviour
{

    [Header("Game Settings")] 
    [SerializeField] private TMP_Text rightSideText;
    
    private void Update()
    {
        if (GameManager.Instance == null) return;
        rightSideText.text = $"Right side: {GameManager.Instance.hasRightCar}";
    }
}
