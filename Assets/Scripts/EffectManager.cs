using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{    
    public static EffectManager Instance;
    
    public GameObject deathEffect;

    private void Awake()
    {
        Instance = this;
    }
}
