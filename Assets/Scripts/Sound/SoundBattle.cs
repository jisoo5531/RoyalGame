using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBattle : MonoBehaviour
{
    public AudioClip defaultBGM;
    public AudioClip thiryBGM;
    public AudioClip sixtyBGM;
    public AudioClip winBGM;
    public AudioClip loseBGM;
    public AudioClip suddenDeath;
    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        StartCoroutine(PlayNextClipWhenFinished());
    }
    private IEnumerator PlayNextClipWhenFinished()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        audioSource.clip = defaultBGM;
        audioSource.Play();
    }

    public void CheckTime(int time, bool isSuddenDeath)
    {
        if (!isSuddenDeath)
        {
            if (time <= 30)
            {
                ChangeBGM(thiryBGM);
            }
            else if (time <= 60)
            {
                ChangeBGM(sixtyBGM);
            }
        }
        else
        {
            ChangeBGM(suddenDeath);
        }
    }

    public void ResultBgm(bool isWin)
    {
        if (audioSource.clip != null)
        {
            audioSource.Stop();
            if(isWin)
            {
                audioSource.clip = winBGM;
            }
            else
            {
                audioSource.clip = loseBGM;
            }
            audioSource.Play();
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
