using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionUsingCard : MonoBehaviour
{
    public GameObject[] onClickUseOrNotUseButton;    

    public void OnClickInteract(int number)
    {
        foreach (GameObject GO in onClickUseOrNotUseButton)
        {
            GO.SetActive(false);
        }
        onClickUseOrNotUseButton[number].SetActive(true);
    }
}
