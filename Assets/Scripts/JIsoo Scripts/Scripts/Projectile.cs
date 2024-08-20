using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviourPunCallbacks
{
    Rigidbody rigid;
    public GameObject target;
    public float speed = 2f;
    public Vector3 dir;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        dir = transform.forward;

        if (photonView.IsMine)
        {
            Invoke("LifeTime", 1.8f);
        }
    }
    private void LifeTime()
    {
        if (gameObject != null && photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (target != null && photonView.IsMine)
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
