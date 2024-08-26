using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDestroy : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        Invoke("DestroyDelay", 0.8f);
    }

    private void DestroyDelay()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
