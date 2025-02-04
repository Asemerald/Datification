using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class AudioManager : InstanceBase<AudioManager>
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;

    public void PlaySound(int index, float volume)
    {
        audioSource.PlayOneShot(audioClips[index], volume);
    }
}
