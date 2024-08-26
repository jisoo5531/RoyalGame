using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveCard : MonoBehaviour
{
    public GameObject Box;
    public GameObject Card;

    public void CardActive()
    {
        Card.SetActive(true);
        Card.GetComponent<OpenChestMoveCard>().MoveCard();
    }
}
