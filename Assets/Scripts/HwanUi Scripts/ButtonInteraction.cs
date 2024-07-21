using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    #region public º¯¼ö
    public GameObject userInfoUI;
    public GameObject shopUI;
    public GameObject menuUI;
    public GameObject collectionUI;
    #endregion

    private GameObject currentOpenUI;
    private bool isUIOpen = false;


    public void UserInfoClick()
    {
        if(!isUIOpen)
        {
            currentOpenUI.SetActive(false);
            userInfoUI.SetActive(true);
            currentOpenUI = userInfoUI;
            isUIOpen = true;
        }
    }
    
    public void ShopClick()
    {
        if (!isUIOpen)
        {
            currentOpenUI.SetActive(false);
            menuUI.SetActive(true);
            currentOpenUI = menuUI;
            isUIOpen = true;
        }
    }

    public void MenuClick()
    {
        if (!isUIOpen)
        {
            currentOpenUI.SetActive(false);
            userInfoUI.SetActive(true);
            currentOpenUI = userInfoUI;
            isUIOpen = true;
        }
    }

    public void CollectionClick()
    {
        if (!isUIOpen)
        {
            currentOpenUI.SetActive(false);
            collectionUI.SetActive(true);
            currentOpenUI = collectionUI;
            isUIOpen = true;
        }
    }
}
