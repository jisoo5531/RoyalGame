using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShakeCard_Jisoo : MonoBehaviour
{
    Vector3 targetPosUP = new Vector3(0, 373, 0);

    private void Start()
    {
        transform.DOLocalMove(targetPosUP, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
}
