using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionUsingCard : MonoBehaviour
{
    public GameObject[] onClickUseOrNotUseButton;
    Collection collection;

    private void Awake()
    {
        collection = GetComponent<Collection>();
    }

    public void OnClickInteract(int number)
    {
        for (int i = 0; i < onClickUseOrNotUseButton.Length; i++)
        {
            if (i != number)
            {
                onClickUseOrNotUseButton[i].SetActive(false);
                continue;
            }
            else
            {
                collection.CheckCardCount();
            }
        }        
        onClickUseOrNotUseButton[number].SetActive(!onClickUseOrNotUseButton[number].activeSelf);
    }
}
