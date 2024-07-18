using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager instance;
    public static EffectManager m_Instance { get; }
    
    public GameObject deathEffect;

    private void Awake()
    {
        instance = this;
    }
}
