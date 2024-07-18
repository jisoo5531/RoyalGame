using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShakeBox : MonoBehaviour
{
    Vector3 targetPosUP = new Vector3(0, 165, 0);
    Vector3 targetPosDown = new Vector3(0, 0, 0);

    public void Start()
    {
        transform.DOShakeRotation(3).OnComplete(BoxUP);
    }

    public void BoxUP()
    {
        transform.DOLocalMove(targetPosUP, 0.5f).OnComplete(BoxDown);
    }

    public void BoxDown()
    {
        transform.DOLocalMove(targetPosDown, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }

}
