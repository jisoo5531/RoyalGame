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
        Invoke("LifeTime", 1.4f);
    }

    private void Start()
    {
        //if (targetPos != null)
        //{
        //    UpdateDirection();
        //   // Vector3 direction = (targetPos - transform.position).normalized;
        //    //Quaternion lookRotation = Quaternion.LookRotation(direction);
        //    //transform.rotation = lookRotation;

        //    //rigid.velocity = direction * speed;
        //}
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

    private void Update()
    {
        if (target != null)
        {
            UpdateDirection();
            //Vector3 direction = (targetPos - transform.position).normalized;
            //direction.y = originalPos.y;
            //Vector3 dir = (targetPos - transform.position).normalized;
            // Quaternion lookRotation = Quaternion.LookRotation(direction);
            // lookRotation.x = originalRotate.y;
            //lookRotation.y = originalRotate.x;
            // transform.rotation = lookRotation;
            //transform.position += t * speed * 30f * Time.deltaTime
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 10.0f * Time.deltaTime);

            //rigid.velocity = direction * 30f * speed;
            //transform.position += direction * speed * 30f * Time.deltaTime;
            //Quaternion qua = Quaternion.Euler(originalPos.x, originalPos.y)
            //transform.rotation = Quaternion.Euler(-90, 0, lookRotation.eulerAngles.z);
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-90f, 0, lookRotation.z), Time.deltaTime * 20f);
            // transform.rotation = Quaternion.Euler(originalRotate.x, originalRotate.y, transform.localEulerAngles.y);
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
