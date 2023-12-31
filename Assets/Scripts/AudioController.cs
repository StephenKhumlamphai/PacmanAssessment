using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip audioClip1;
    public AudioClip audioClip2;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayAudioClip(audioClip1);
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (audioSource.clip == audioClip1)
                PlayAudioClip(audioClip2);
            else
                PlayAudioClip(audioClip1);
        }
    }

    private void PlayAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
