using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSAnimationEnd : MonoBehaviour
{
    public void AnimationEnd()
    {
        TimeManager.Instance.GameStart();
        this.gameObject.SetActive(false);
    }
}
