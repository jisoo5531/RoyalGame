using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLobby : MonoBehaviour
{
    public AudioClip defaultBGM;
    public AudioClip CollectionBGM;
    private AudioSource audioSource;

    public GameObject targetShop;
    public GameObject targetCollection;
    public GameObject targetLobby;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (defaultBGM != null)
        {
            audioSource.clip = defaultBGM;
            audioSource.Play();
        }
    }

    void Update()
    {
        if (targetCollection.activeInHierarchy)
        {
            ChangeBGM(CollectionBGM);
        }
        else if (!targetLobby.activeInHierarchy)
        {
            audioSource.Stop();
        }
        else
        {
            ChangeBGM(defaultBGM);
        }
    }

    void ChangeBGM(AudioClip newClip)
    {
        if (newClip != null && audioSource.clip != newClip)
        {
            audioSource.Stop();
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
}