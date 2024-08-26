using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{    
    public static EffectManager instance;
    
    public GameObject deathEffect;

    private void Awake()
    {
        instance = this;
    }
}
