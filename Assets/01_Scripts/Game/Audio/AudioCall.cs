using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCall : MonoBehaviour
{
    public void ExternalCall(int index)
    {
        AudioManager.Instance.PlaySound(index);
    }
}
