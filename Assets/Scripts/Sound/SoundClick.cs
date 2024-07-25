using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundClick : MonoBehaviour
{
    public AudioClip clickOn; // On 클릭 사운드
    public AudioClip clickOff; // Off 번째 클릭 사운드
    private AudioSource audioSource;


    void Start()
    {
        // AudioSource 컴포넌트를 추가하거나 가져옵니다.
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlayClickSound(int soundIndex)
    {
        // 사운드 선택
        if (soundIndex == 1)
        {
            audioSource.clip = clickOn;
        }
        else if (soundIndex == 2)
        {
            audioSource.clip = clickOff;
        }

        // 사운드 재생
        audioSource.Play();
    }
}
