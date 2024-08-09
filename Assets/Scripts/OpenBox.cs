using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBox : MonoBehaviour
{
    public GameObject Box_Close;
    public GameObject Box_Open;

    void Start()
    {
        Box_Close = GameObject.Find("Box_Close");
        Box_Open = GameObject.Find("Box_Open");
    }

    private void Update()
    {
        Chanege();
    }

    private void Chanege()
    {
        if (!Box_Close.activeSelf)
        {
            GameObject.Find("Box").transform.Find("Box_Open").gameObject.SetActive(true);
        }
    }

}
