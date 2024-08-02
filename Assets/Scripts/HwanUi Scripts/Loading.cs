using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviourPunCallbacks
{
    public static string nextScene;

    [SerializeField]
    Slider progressBar;

    [SerializeField]
    TMP_Text loadText;


    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    IEnumerator LoadScene()
    {
        //yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        bool isHalfway = false;

        while (!op.isDone)
        {
            timer += Time.deltaTime / 5f;

            if (!isHalfway)
            {
                progressBar.value = Mathf.Lerp(progressBar.value, 0.51f, timer * 2f);
                loadText.text = (int)(progressBar.value * 100) + "%";

                if (progressBar.value >= 0.51f)
                {
                    isHalfway = true;
                    timer = 0f;
                }
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= 1f)
                {
                    progressBar.value = Mathf.Lerp(progressBar.value, 1, timer);
                    loadText.text = (int)(progressBar.value * 100) + "%";

                    if (progressBar.value >= 1.0f)
                    {
                        yield return new WaitForSeconds(0.1f);
                        op.allowSceneActivation = true;
                        yield break;
                    }
                }
            }

            yield return null;
        }
    }

    private void StartGameOnAllClients(string sceneName)
    {
        //PhotonNetwork.LoadLevel(sceneName);
        //SceneManager.LoadScene(sceneName);
    }
}
