using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundClick : MonoBehaviour
{
    public AudioClip clickOn;
    public AudioClip clickOff;
    private AudioSource audioSource;


    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlayClickSound(int soundIndex)
    {
        if (soundIndex == 1)
        {
            audioSource.clip = clickOn;
        }
        else if (soundIndex == 2)
        {
            audioSource.clip = clickOff;
        }

        audioSource.Play();
    }
}
