using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Tower : MonoBehaviourPunCallbacks
{
    public int HP { get; set; }
    public int maxHP { get; set; }

    public float range { get; set; }

    public int damage { get; set; }

    public bool isNotOnCannon { get; set; }

    public GameObject onTopUnit;
    protected DeffenseUnit deffenseUnit;

    protected virtual void InitData() { }
}

