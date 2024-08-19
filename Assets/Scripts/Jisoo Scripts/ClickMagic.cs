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
    public Vector3 spawnPos;

    // Update is called once per frame
    private void Update()
    {
        spawnPos.y = -1;
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    if (Physics.Raycast(ray, out RaycastHit hit))
        //    {
        //        GameObject FireRange =  Instantiate(fireballRange, hit.point, fireballRange.transform.rotation);
        //        Destroy(FireRange, 1f);
        //        spawnPos = hit.point;
        //        spawnPos.y = -1;
        //        SpawnFireBall();
        //    }
        //}
    }

    public void SpawnFireBall(GameObject fireball)
    {
        fireball.transform.GetChild(0).transform.position = startTrans.position;

        Projectile fireballProjectile = fireball.transform.GetChild(0).gameObject.AddComponent<Projectile>();
        //fireballProjectile.target = spawnPos;
    }
}
