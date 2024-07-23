using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    Rigidbody rigid;
    public Vector3 forward;
    public Transform target;
    public Quaternion lookRotationSam;
    public float speed = 1f;

    private float x;
    private float z;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;
        

        //Destroy(gameObject, 4f);
        x = transform.rotation.x;
        z = transform.rotation.z;
    }
    void Update()
    {
        if (target != null)
        {
            // 타겟을 향한 방향 벡터 계산
            Vector3 direction = (target.position - transform.position).normalized;
            // 투사체에 초기 속도 설정
            rigid.velocity = direction * 10f * speed;
        }

        if (target != null)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            //lookRotation.x = lookRotation.z = 0;
         
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 10.0f * Time.deltaTime);
            lookRotationSam = lookRotation;
        }        
        
        //rigid.AddForce(forward * 0.1f, ForceMode.Impulse);

    }
}
