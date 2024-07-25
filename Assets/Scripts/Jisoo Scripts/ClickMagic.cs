using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMagic : MonoBehaviour
{
    public GameObject explosion;
    public GameObject fireballRange;
    public GameObject fireBallPrefab;
    public Transform startTrans;
    public Transform targetTrans;
    public Vector3 clickPos;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject FireRange =  Instantiate(fireballRange, hit.point, fireballRange.transform.rotation);
                Destroy(FireRange, 1f);
                clickPos = hit.point;
                clickPos.y = -1;
                SpawnFireBall();
            }
        }
    }

    private void SpawnFireBall()
    {        
        GameObject fireball = Instantiate(fireBallPrefab, clickPos, fireBallPrefab.transform.rotation);
        fireball.transform.GetChild(0).transform.position = startTrans.position;
        Projectile fireballProjectile = fireball.transform.GetChild(0).gameObject.AddComponent<Projectile>();
        fireballProjectile.targetPos = clickPos;
    }
}
