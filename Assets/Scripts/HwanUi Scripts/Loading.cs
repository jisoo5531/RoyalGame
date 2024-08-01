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
    public static Loading instance;
    public static string nextScene = string.Empty;

    [SerializeField]
    Slider progressBar;

    [SerializeField]
    TMP_Text loadText;

    public static bool isBattle;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (nextScene != string.Empty)
        {
            StartCoroutine(LoadScene());
        }
    }

    public static void LoadNextScene(string sceneName)
    {

    }

    public static void LoadScene(string sceneName, bool isGameStart)
    {
        //nextScene = sceneName;
        //isBattle = isGameStart;
        SceneManager.LoadScene("Loading");
    }

    public void InitScene(string sceneName, bool isGameStart)
    {
        nextScene = sceneName;
        isBattle = isGameStart;
    }

    IEnumerator LoadScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);

        op.allowSceneActivation = false;

        float timer = 0.0f;
        bool isHalfway = false;
        while (!op.isDone)
        {
            yield return null;

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
                        yield return new WaitForSeconds(0.3f);
                        op.allowSceneActivation = true;
                        //if (isBattle)
                        //{
                        //    if (PhotonNetwork.IsMasterClient)
                        //    {
                        //        StartGameOnAllClients(nextScene);
                        //    }
                        //}
                        yield break;
                    }
                }
            }
        }
    }

    private void StartGameOnAllClients(string sceneName)
    {
        //PhotonNetwork.LoadLevel(sceneName);
        //SceneManager.LoadScene(sceneName);
    }
}
