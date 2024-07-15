using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public LayerMask targetLayerMask;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("뭔가 맞았다.");
        if ((targetLayerMask | (1 << other.gameObject.layer)) != targetLayerMask)
        {
            return;
        }

        if (other.TryGetComponent<EnemyTest>(out EnemyTest enemy))
        {
            Debug.Log("적맞았다.");
            enemy.GetHit(damage);
        }


        //if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
        //{
        //    Debug.Log("테스트.");
        //    damagable.GetDamage(damage);
        //}
    }
}
