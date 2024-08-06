using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviourPunCallbacks
{
    Rigidbody rigid;    
    public Vector3 targetPos;    
    public float speed = 1f;
    private Vector3 originalRotate;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        photonView.RPC("DestoryBow", RpcTarget.All);
        originalRotate = transform.localEulerAngles;
        Debug.Log(transform.localRotation);
        Debug.Log(transform.transform.localEulerAngles);
        Debug.Log(transform.transform.eulerAngles);
    }
    [PunRPC]
    private void DestoryBow()
    {
        Destroy(gameObject, 4f);
    }

    void Update()
    {
        if (targetPos != null)
        {
            Vector3 direction = (targetPos - transform.position).normalized;
            rigid.velocity = direction * 30f * speed;
        }

        if (targetPos != null)
        {
            Vector3 dir = (targetPos - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(dir);            
         
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 10.0f * Time.deltaTime);
            transform.rotation = Quaternion.Euler(originalRotate.x, originalRotate.y, transform.rotation.z);
        }                        
    }
}
