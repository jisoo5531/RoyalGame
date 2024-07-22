using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MJ_OpenCard : MonoBehaviour
{
    public GameObject Box;
    public GameObject Card_Open;
    public OnClickOpenChest openChest;

    public void Chanege()
    {
        Debug.Log("Â¥ÀÜ");
        //if (openChest.isOpenClick)
        //{
        //   // OpenBox();
        //    openChest.isOpenClick = false;
        //}
        //else
        //{
        //    //CloseBox();
        //    openChest.isOpenClick = true;
        //}
    }
    private void OpenBox()
    {
        Card_Open.SetActive(true);
    }
    private void CloseBox()
    {
        Card_Open.SetActive(false);
    }
}
