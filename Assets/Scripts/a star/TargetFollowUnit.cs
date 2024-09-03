using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TargetFollowUnit : MonoBehaviourPunCallbacks
{
    #region public 변수
    public Transform target = null;

    public float speed;
    public Collider targetCollider;
    public bool isAttack = false;
    #endregion

    #region private 변수
    int targetIndex;
    Vector3[] path;
    Vector3 currentWaypoint;
    UnitCanvasInfo unitInfoCanvas;
    #endregion

    private void Awake()
    {
        unitInfoCanvas = GetComponent<UnitCanvasInfo>();
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (this == null)
        {
            StopCoroutine("FollowPath");
            return;
        }

        if (pathSuccessful)
        {
            for (int i = 0; i < newPath.Length; i++)
            {
                newPath[i].y = transform.position.y;
            }
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        currentWaypoint = path[0];
        while (true)
        {
            if (Vector3.Distance(transform.position, currentWaypoint) < 1.5)
            {
                targetIndex++;

                if (targetIndex < path.Length)
                {
                    currentWaypoint = path[targetIndex];
                }
            }

            if (isAttack)
            {
                yield break;
            }

            Vector3 direction = (currentWaypoint - transform.position);
            direction.y = 0;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider != targetCollider && hitCollider.gameObject.layer == 11)
                {
                    Vector3 avoidDirection = transform.position - hitCollider.transform.position;
                    direction += avoidDirection.normalized * 0.5f;
                }
            }

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Vector3 eulerAngles = targetRotation.eulerAngles;
                eulerAngles.x = 0;
                eulerAngles.z = 0;
                targetRotation = Quaternion.Euler(eulerAngles);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 7f);

                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("CanvasRotate", RpcTarget.Others, transform.localEulerAngles.y);
                    unitInfoCanvas.unitCanvas.transform.localEulerAngles = new Vector3(0, -transform.localEulerAngles.y, 0);
                }
                else
                {
                    photonView.RPC("CanvasRotate", RpcTarget.Others, transform.localEulerAngles.y - 180);
                    unitInfoCanvas.unitCanvas.transform.localEulerAngles = new Vector3(0, -transform.localEulerAngles.y + 180, 0);
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    [PunRPC]
    private void CanvasRotate(float rotateY)
    {
        unitInfoCanvas.unitCanvas.transform.localEulerAngles = new Vector3(0, -rotateY + 180, 0);
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}