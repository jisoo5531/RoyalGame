using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDestroy : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        photonView.RPC("DestroyObj", RpcTarget.All);
    }

    [PunRPC]
    public void DestroyObj()
    {
        Destroy(gameObject, 0.8f);
    }
}
