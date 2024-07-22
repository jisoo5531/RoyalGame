using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matching_Move : MonoBehaviour
{
    public float speed = 5.0f;
    private Vector3 targetPosition; 
    private Vector3 startPosition; 
    private float journeyLength; 
    private float startTime; 

    void Start()
    {
        SetNewRandomPosition();
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        transform.localPosition = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

        if (transform.localPosition == targetPosition)
        {
            SetNewRandomPosition();
        }
    }

    void SetNewRandomPosition()
    {
        startPosition = transform.localPosition;
        targetPosition = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0);
        journeyLength = Vector3.Distance(startPosition, targetPosition);
        startTime = Time.time;
    }
}
