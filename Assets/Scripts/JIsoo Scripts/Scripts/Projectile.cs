using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviourPunCallbacks
{
    Rigidbody rigid;
    public Vector3 targetPos;
    public float speed = 13f;
    private Vector3 originalRotate;
    private Vector3 originalPos;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        originalRotate = transform.localEulerAngles;
        originalPos = transform.position;
        Debug.Log(originalRotate);
        Invoke("LifeTime", 1.7f);
    }

    private void LifeTime()
    {
        if (gameObject != null)
        {
            photonView.RPC("DestoryBow", RpcTarget.All);
        }
    }

    [PunRPC]
    private void DestoryBow()
    {
        Destroy(gameObject, 4f);
    }

    private void FixedUpdate()
    {
        if (targetPos != null)
        {
            Vector3 direction = (targetPos - transform.position).normalized;
            //direction.y = originalPos.y;
            //Vector3 dir = (targetPos - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            lookRotation.x = originalRotate.y;
            lookRotation.y = originalRotate.x;
            // transform.rotation = lookRotation;
            transform.position += direction * speed * 30f * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20f);
            //rigid.velocity = direction * 30f * speed;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 10.0f * Time.deltaTime);

            // transform.rotation = Quaternion.Euler(originalRotate.x, originalRotate.y, transform.localEulerAngles.y);
        }
    }
}
