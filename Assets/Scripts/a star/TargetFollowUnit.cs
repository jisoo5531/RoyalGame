using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

#region 박환의
public class TargetFollowUnit : MonoBehaviour
{
    public Transform target;
    float speed = 3;
    Vector3[] path;
    int targetIndex;
    const float stopDistance = 11f;
    const float obstacleCheckDistance = 1f;
    const float obstacleAvoidanceDistance = 1f;
    GridController controller;

    private void Awake()
    {
        controller = FindAnyObjectByType<GridController>();
    }

    void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
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
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (targetIndex < path.Length)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length - 1)
                    {
                        // 목표 지점 앞에서 멈추는 지점 계산
                        Vector3 directionToTarget = (target.position - transform.position).normalized;
                        currentWaypoint = target.position - directionToTarget * stopDistance;
                    }
                    else
                    {
                        currentWaypoint = path[targetIndex];
                    }
                }
            }
            else
            {
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                Vector3 stopPosition = target.position - directionToTarget * stopDistance;

                if (Vector3.Distance(transform.position, stopPosition) < 0.1f)
                {
                    yield break;
                }
                else
                {
                    currentWaypoint = stopPosition;
                }
            }

            Vector3 direction = (currentWaypoint - transform.position);
            direction.y = 0;

            // 장애물 감지
            Ray ray = new Ray(transform.position, transform.forward);
            print("a");
            Debug.DrawRay(transform.position, transform.forward * 300, Color.red);
            if (Physics.Raycast(ray, obstacleCheckDistance, controller.unwalkableMask))
            {
                // 장애물 회피
                Vector3 avoidanceDirection = Vector3.Cross(transform.forward, Vector3.up).normalized;
                Vector3 avoidancePosition = transform.position + avoidanceDirection * obstacleAvoidanceDistance;

                // 회피할 위치로 이동
                transform.position = Vector3.MoveTowards(transform.position, avoidancePosition, speed * Time.deltaTime);
            }
            else
            {
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

                Vector3 adjustedPosition = path[i];
                if (i == path.Length - 1)
                {
                    Vector3 directionToTarget = (target.position - transform.position).normalized;
                    adjustedPosition = target.position - directionToTarget * stopDistance;
                }

                Gizmos.DrawCube(adjustedPosition, Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, adjustedPosition);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], adjustedPosition);
                }
            }
        }
    }
}
#endregion