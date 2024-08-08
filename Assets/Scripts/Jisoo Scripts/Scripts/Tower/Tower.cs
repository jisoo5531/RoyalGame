using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviourPunCallbacks
{
    public int HP { get; set; }
    public int maxHP { get; set; }

    public bool isNotOnCannon { get; set; }

    public GameObject onTopUnit;
}
