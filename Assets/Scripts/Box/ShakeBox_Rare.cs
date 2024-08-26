using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShakeBox_Rare : MonoBehaviour
{
    Vector3 targetPosUP = new Vector3(-4, 343, 0);
    Vector3 targetPosDown = new Vector3(-4, -7, 0);
    Vector3 targetPosJump_Left = new Vector3(-240, -7, 5);
    Vector3 targetPosJump_Right = new Vector3(236, -7, 5);

    public void Start()
    {
        transform.DOShakeScale(2).OnComplete(BoxLeft);
    }

    public void BoxLeft()
    {
        transform.DOLocalJump(targetPosJump_Left, 100f, 1, 0.3f).OnComplete(BoxRight);
    }

    public void BoxRight()
    {
        transform.DOLocalJump(targetPosJump_Right, 100f, 1, 0.3f).OnComplete(BoxState);
    }

    public void BoxState()
    {
        transform.DOLocalJump(targetPosDown, 100f, 1, 0.3f).OnComplete(BoxUP);
    }

    public void BoxUP()
    {
        transform.DOLocalMove(targetPosUP, 0.4f).OnComplete(BoxDown);
    }

    public void BoxDown()
    {
        transform.DOLocalMove(targetPosDown, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }

}
