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
    float range = 3.5f;
    float baseRange = 1.3f;
    int targetIndex;
    bool istrue = false;
    Transform firstTarget;
    Collider targetCollider;

    void Start()
    {
        firstTarget = target;
        // AdjustRange();
        targetCollider = target.GetComponent<Collider>();
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    private void Update()
    {
        if (firstTarget != target)
        {
           // AdjustRange();
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            firstTarget = target;
        }
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
    void AdjustRange()
    {

        if (targetCollider != null)
        {
            float maxColliderDimension = GetMaxColliderDimension(targetCollider);
            range = baseRange * maxColliderDimension;
            print(maxColliderDimension + ",  " + range);
        }
    }
    float GetMaxColliderDimension(Collider collider)
    {
        if (collider is BoxCollider)
        {
            BoxCollider boxCollider = (BoxCollider)collider;
            Vector3 size = boxCollider.size;
            return Mathf.Max(size.x, size.y, size.z);
        }
        else if (collider is CapsuleCollider)
        {
            CapsuleCollider capsuleCollider = (CapsuleCollider)collider;
            float radius = capsuleCollider.radius;
            float height = capsuleCollider.height;
            return Mathf.Max(radius * 7f, height);
        }
        return 1f;
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

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
            Vector3 closestPointOnTarget = targetCollider.ClosestPoint(transform.position);

            // 타겟의 가장 가까운 점으로의 방향을 계산
            Vector3 directionToTarget = (closestPointOnTarget - transform.position).normalized;

            // 타겟의 콜라이더로부터 `range`만큼 떨어진 지점 계산
            Vector3 targetPosition = closestPointOnTarget - directionToTarget * range;

            // 타겟 위치까지의 거리 계산
            float distanceToTargetPosition = Vector3.Distance(transform.position, targetPosition);

            if (distanceToTargetPosition < 0.1f)
            {
                istrue = true;
                yield break;
            }

            if (!istrue)
            {
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