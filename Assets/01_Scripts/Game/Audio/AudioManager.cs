using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class AudioManager : InstanceBase<AudioManager>
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int index, float volume = 1)
    {
        //audioSource.PlayOneShot(audioClips[index], volume);
    }
}
