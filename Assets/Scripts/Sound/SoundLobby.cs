using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLobby : MonoBehaviour
{
    public AudioClip defaultBGM;  // 기본 BGM
    public AudioClip ShopBGM;      // 새로운 상황에 맞는 BGM
    public AudioClip CollectionBGM;
    private AudioSource audioSource;

    // 특정 오브젝트를 할당합니다.
    public GameObject targetShop;
    public GameObject targetCollection;
    public GameObject targetLobby;

    void Start()
    {
        // AudioSource 컴포넌트를 가져옵니다.
        audioSource = GetComponent<AudioSource>();

        // 기본 BGM을 재생합니다.
        if (defaultBGM != null)
        {
            audioSource.clip = defaultBGM;
            audioSource.Play();
        }
    }

    void Update()
    {
        // 특정 오브젝트가 활성화되었는지 확인합니다.
        if (targetShop.activeInHierarchy)
        {
            ChangeBGM(ShopBGM);
        }
        else if (targetCollection.activeInHierarchy)
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

    // BGM을 변경하는 함수
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