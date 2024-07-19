using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

#region 박환의
public class TargetFollowUnit : MonoBehaviour
{
    #region public 변수
    public Transform target = null;

    public float speed;
    public float range;
    public Collider targetCollider;
    public bool isAttack = false;
    public bool isMove = false;
    #endregion

    #region private 변수
    int targetIndex;
    Vector3[] path;
    Vector3 currentWaypoint;
    GridController controller;
    #endregion

    private void Awake()
    {
        controller = FindAnyObjectByType<GridController>();
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
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
            if (transform.position == currentWaypoint)
            {
                targetIndex++;

                if (targetIndex >= path.Length)
                {
                    yield break;
                }

                currentWaypoint = path[targetIndex];
            }

            if (isAttack)
            {
                yield break;
            }

            Vector3 direction = (currentWaypoint - transform.position).normalized;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Vector3 eulerAngles = targetRotation.eulerAngles;
                eulerAngles.x = 0;
                eulerAngles.z = 0;
                targetRotation = Quaternion.Euler(eulerAngles);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 7f);
            }

            if (!Physics.CheckSphere(transform.position + direction * controller.nodeRadius, controller.nodeRadius, controller.unwalkableMask))
            {
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            }
            else
            {
                Vector3 avoidanceDirection = Vector3.Cross(direction, Vector3.up).normalized;
                if (!Physics.CheckSphere(transform.position + avoidanceDirection * controller.nodeRadius, controller.nodeRadius, controller.unwalkableMask))
                {
                    transform.position += avoidanceDirection * speed * Time.deltaTime;
                }
                else
                {
                    transform.position -= avoidanceDirection * speed * Time.deltaTime;
                }
            }

            yield return null;
        }
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
#endregion