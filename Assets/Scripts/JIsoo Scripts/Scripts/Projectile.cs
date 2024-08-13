using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviourPunCallbacks
{
    Rigidbody rigid;
    public GameObject target;
    public float speed = 1f;
    private Vector3 originalRotate;
    private Vector3 originalPos;
    public Vector3 dir;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        originalRotate = transform.localEulerAngles;
        originalPos = transform.position;
        dir = transform.forward;
        Invoke("LifeTime", 2f);
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
        Destroy(gameObject);
    }

    private void Update()
    {
        if (target != null)
        {
            UpdateDirection();
        }
    }
    private void UpdateDirection()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        rigid.velocity = direction * 30f * speed;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 10.0f * Time.deltaTime);
    }
}
