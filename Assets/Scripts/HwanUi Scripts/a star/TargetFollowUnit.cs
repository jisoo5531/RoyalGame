using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

#region ¹ÚÈ¯ÀÇ
public class TargetFollowUnit : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float range;
    Vector3[] path;
    int targetIndex;
    public CharacterController targetCollider;
    Vector3 currentWaypoint;
    public bool isAttack = false;
    public bool isMove = false;

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

                if (targetIndex < path.Length)
                {
                    currentWaypoint = path[targetIndex];
                }
            }
            //Vector3 closestPointOnTarget = targetCollider.ClosestPoint(transform.position);

            //Vector3 directionToTarget = (closestPointOnTarget - transform.position).normalized;

            //Vector3 targetPosition = closestPointOnTarget - directionToTarget * range;

            //float distanceToTargetPosition = Vector3.Distance(transform.position, targetPosition);

            //if (distanceToTargetPosition < 0.1f)
            //{
            //    yield break;
            //}
            if(isAttack)
            {
                yield break;
            }

            Vector3 direction = (currentWaypoint - transform.position);
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
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
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