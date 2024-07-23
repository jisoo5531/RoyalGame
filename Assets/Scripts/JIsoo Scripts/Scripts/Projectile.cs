using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    Rigidbody rigid;    
    public Vector3 targetPos;    
    public float speed = 1f;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;
        

        //Destroy(gameObject, 4f);        
    }
    void Update()
    {
        if (targetPos != null)
        {
            // 타겟을 향한 방향 벡터 계산
            Vector3 direction = (targetPos - transform.position).normalized;
            // 투사체에 초기 속도 설정
            rigid.velocity = direction * 30f * speed;
        }

        if (targetPos != null)
        {
            Vector3 dir = (targetPos - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(dir);            
         
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 10.0f * Time.deltaTime);            
        }                        
    }
}
