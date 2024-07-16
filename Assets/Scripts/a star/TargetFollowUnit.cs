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
    float range;
    float baseRange = 1.3f;
    int targetIndex;
    bool istrue = false;
    Transform firstTarget;

    void Start()
    {
        firstTarget = target;
        AdjustRange();
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    private void Update()
    {
        if (firstTarget != target)
        {
            AdjustRange();
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
        Collider targetCollider = target.GetComponent<Collider>();
        //if (targetCollider != null)
        //{
        //    float targetSize = Mathf.Max(targetCollider.bounds.size.x, targetCollider.bounds.size.z);
        //    print(targetSize);
        //    range = targetSize;
        //}
        //else
        //{
        //    range = baseRange;
        //}
        if (targetCollider != null)
        {
            // Collider 타입에 따라 크기를 계산
            float maxColliderDimension = GetMaxColliderDimension(targetCollider);

            // 조정된 범위 계산
            range = baseRange * maxColliderDimension;
            print(range);
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
            return Mathf.Max(radius * 2, height);
        }
        else
        {
            Debug.LogWarning("Collider type not supported for range adjustment.");
            return 1.0f; // 기본 값 반환
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        //float stoppingDistance = 8f;

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
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            Vector3 targetPosition = target.position - directionToTarget * range;

            // targetPosition과 현재 위치의 거리 계산
            float distanceToTargetPosition = Vector3.Distance(transform.position, targetPosition);

            // targetPosition에 도달하면 멈춤
            if (distanceToTargetPosition < 0.1f)
            {
                istrue = true;
                yield break;
            }

            //if (Vector3.Distance(transform.position, target.position) < stoppingDistance)
            //{
            //    istrue = true;
            //    yield break;
            //}

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