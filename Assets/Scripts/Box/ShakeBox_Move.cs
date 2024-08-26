using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShakeBox_Move : MonoBehaviour
{
    Vector3 targetPosUP = new Vector3(0, 85, 0);
    Vector3 targetPosDown = new Vector3(0, 0, 0);
    Vector3 targetPosJump_Left = new Vector3(-240, -7, 5);

    public void Start()
    {
        transform.DOShakeScale(0.7f, 0.2f, 3, 1).OnComplete(BoxJumpDown);
    }

    public void BoxJumpUP()
    {
        transform.DOLocalJump(targetPosUP, 100f, 1, 0.3f);
    }

    public void BoxJumpDown()
    {
        transform.DOLocalJump(targetPosDown, 100f, 1, 0.3f);
    }

    public void BoxLeft()
    {
        transform.DOLocalJump(targetPosJump_Left, 100f, 1, 0.3f).OnComplete(BoxRight);
    }

    public void BoxRight()
    {
        transform.DOLocalJump(targetPosUP, 100f, 1, 0.3f).OnComplete(BoxState);
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
