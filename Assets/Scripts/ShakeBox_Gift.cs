using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShakeBox_Gift : MonoBehaviour
{

    public void Start()
    {
        transform.DOShakeScale(1, 0.2f, 3, 1);
    }
}
