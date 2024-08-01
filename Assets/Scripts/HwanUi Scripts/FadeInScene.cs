using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInScene : MonoBehaviour
{
    public void LobbyScene()
    {
        //Loading.InitScene("Lobby", false);
        Loading.instance.InitScene("Lobby", false);
        SceneManager.LoadScene("Loading");
        //Loading.LoadScene("Lobby", false);
    }
}
