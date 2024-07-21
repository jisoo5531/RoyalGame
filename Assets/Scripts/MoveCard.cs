using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveCard : MonoBehaviour
{
    public GameObject Box;
    public GameObject Card;


    void Start()
    {
        Invoke("CardActive", 0.7f);
    }

    public void CardActive()
    {
        GameObject.Find("Box").transform.Find("Card").gameObject.SetActive(true);
    }
}
