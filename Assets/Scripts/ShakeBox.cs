using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShakeBox : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Shake()
    {
        transform.DOShakeRotation(3);
    }
}
