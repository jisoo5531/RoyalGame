using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCard : MonoBehaviour
{
    public GameObject Box;
    public GameObject Card_Open;

    void Start()
    {
        //Invoke("Chanege", 1.2f);
    }

    public void Chanege()
    {
        Debug.Log("Â¥ÀÜ");
            GameObject.Find("Box").transform.Find("Card_Open").gameObject.SetActive(true);
    }
}
