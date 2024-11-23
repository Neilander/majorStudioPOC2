using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballScript : MonoBehaviour
{
    public List<Transform> points; // List to hold the transforms for the path
    public AnimationCurve yCurve; // Animation curve for y-axis movement
    public float baseY; // Base height
    private bool isMoving = false; // Flag to indicate if the ball is currently moving
    private int toIndex;

    [Header("测试用")]
    public int testStartIndex;
    public int testEndIndex;
    public float testT;
    public float testh; 
    public bool testBtn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (testBtn)
        {
            /*
            MoveBall(testStartIndex, testEndIndex, testT, testh);
            testBtn = false;*/
            if (animalManager.Instance != null)
                animalManager.Instance.ballToIndex(this, 0);

            testBtn = false;
        } 
    }

    public void MoveBall(int startIndex, int endIndex, float t, float h)
    {
        // Validate indices
        if (points == null || points.Count == 0)
        {
            Debug.LogError("Points list is empty or null.");
            return;
        }

        if (startIndex < 0 || startIndex >= points.Count)
        {
            Debug.LogError($"Start index {startIndex} is out of range.");
            return;
        }

        if (endIndex < 0 || endIndex >= points.Count)
        {
            Debug.LogError($"End index {endIndex} is out of range.");
            return;
        }

        if (isMoving)
        {
            Debug.LogWarning("Already moving. Wait until current movement is finished.");
            return;
        }

        isMoving = true; // Set the flag to indicate movement has started
        toIndex = endIndex;
        StartCoroutine(MoveCoroutine(points[startIndex], points[endIndex], t, h));
    }

    private IEnumerator MoveCoroutine(Transform startPoint, Transform endPoint, float duration, float height)
    {
        // Set the initial position to the start point
        transform.position = startPoint.position;

        float elapsedTime = 0f; // Track elapsed time

        Vector3 startPosition = startPoint.position;
        Vector3 endPosition = endPoint.position;

        while (elapsedTime < duration)
        {
            // Calculate the normalized time [0, 1]
            float normalizedTime = elapsedTime / duration;

            // Lerp the x position
            float newX = Mathf.Lerp(startPosition.x, endPosition.x, normalizedTime);

            // Calculate the y position using the animation curve
            float curveValue = yCurve.Evaluate(normalizedTime);
            float newY = Mathf.Lerp(baseY, baseY + height, curveValue);

            // Update the position of the ball
            transform.position = new Vector3(newX, newY, transform.position.z);

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the ball reaches the end position
        transform.position = endPosition;

        isMoving = false; // Reset the flag

        if (animalManager.Instance != null)
            animalManager.Instance.ballToIndex(this, toIndex);
    }
}
