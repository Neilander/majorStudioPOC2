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

    [Header("开头掉落")]
    public AnimationCurve dropCurve;
    public float dropT; // 移动的持续时间
    public float height;

    [Header("测试用")]
    public int testStartIndex;
    public int testEndIndex;
    public float testT;
    public float testh;
    public bool testBtn;

    public float initialYVperUnit = 3f;
    public float baseYV = 2f;
    float gravity = 9.8f;


    //private baseAnimalScript toCatch;
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
        //StartCoroutine(MoveCoroutine(points[startIndex], points[endIndex], t, h));
        StartCoroutine(MoveInParabola(animalManager.Instance.returnAnimalWithIndex(startIndex, true), animalManager.Instance.returnAnimalWithIndex(endIndex, false), baseYV+initialYVperUnit*(endIndex-startIndex), gravity, animalManager.Instance.returnAnimalBasedOnIndex(endIndex)));
    }

    
    public void doInitialDrop(Vector3 toPos, baseAnimalScript toAnimal)
    {
        // 计算初始位置：目标位置上方 height 的位置
        Vector3 startPosition = new Vector3(toPos.x, toPos.y + height, toPos.z);

        // 将对象设置到初始位置，并激活
        transform.position = startPosition;
        gameObject.SetActive(true);
        

        // 启动协程进行移动
        StartCoroutine(MoveToPosition(toPos, dropCurve, dropT, toAnimal));
    }

    public IEnumerator MoveToPosition(Vector3 toPos, AnimationCurve curve, float duration, baseAnimalScript toCatch = null)
    {
        float elapsedTime = 0f; // 已经过的时间
        Vector3 startPosition = transform.position; // 初始位置

        while (elapsedTime < duration)
        {
            // 计算当前进度（归一化时间 [0, 1]）
            float normalizedTime = elapsedTime / duration;

            // 根据曲线计算当前位置的 Y 值
            float curveValue = curve.Evaluate(normalizedTime);
            float currentY = Mathf.Lerp(startPosition.y, toPos.y, curveValue);

            // 更新位置（保持 X 和 Z 不变）
            transform.position = new Vector3(toPos.x, currentY, toPos.z);

            // 增加经过的时间
            elapsedTime += Time.deltaTime;

            // 等待下一帧
            yield return null;
        }

        // 确保最后的位置完全对齐
        transform.position = toPos;
        if (toCatch != null)
            toCatch.TakeBall(this);
    }


    /// <summary>
    /// 启动抛物线运动。
    /// </summary>
    /// <param name="start">起始点</param>
    /// <param name="end">终点</param>
    /// <param name="initialYVelocity">初始y方向速度</param>
    /// <param name="gravity">重力加速度</param>
    /// <returns></returns>
    public IEnumerator MoveInParabola(Vector3 start, Vector3 end, float initialYVelocity, float gravity, baseAnimalScript toCatch)
    {
        // 计算竖直方向的运动时间
        float totalTime = CalculateParabolaTime(start.y, end.y, initialYVelocity, gravity);
        if (totalTime <= 0)
        {
            Debug.LogError("Invalid time calculated for parabola motion.");
            yield break;
        }

        // 计算水平方向的速度
        Vector3 horizontalVelocity = new Vector3(
            (end.x - start.x) / totalTime,
            0,
            (end.z - start.z) / totalTime
        );

        // 初始化运动状态
        float elapsedTime = 0f;
        Vector3 currentPosition = start;

        while (elapsedTime < totalTime)
        {
            // 当前时间下的竖直方向位移
            float currentYVelocity = initialYVelocity - gravity * elapsedTime;
            float currentY = start.y + initialYVelocity * elapsedTime - 0.5f * gravity * elapsedTime * elapsedTime;

            // 当前时间下的水平位移
            Vector3 currentHorizontal = start + horizontalVelocity * elapsedTime;

            // 更新物体位置
            currentPosition = new Vector3(currentHorizontal.x, currentY, currentHorizontal.z);
            transform.position = currentPosition;

            // 增加时间
            elapsedTime += Time.deltaTime;

            // 等待下一帧
            yield return null;
        }

        // 确保最终位置准确
        transform.position = end;
        toCatch.TakeBall(this);
        isMoving = false;
    }

    /// <summary>
    /// 计算抛物线运动的总时间。
    /// </summary>
    /// <param name="startY">起始y坐标</param>
    /// <param name="endY">终点y坐标</param>
    /// <param name="initialYVelocity">初始y方向速度</param>
    /// <param name="gravity">重力加速度</param>
    /// <returns>总时间</returns>
    private float CalculateParabolaTime(float startY, float endY, float initialYVelocity, float gravity)
    {
        // 根据二次方程计算时间
        float a = -0.5f * gravity;
        float b = initialYVelocity;
        float c = startY - endY;

        // 使用求根公式计算时间
        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            Debug.LogError("No valid solution for parabola time.");
            return -1f;
        }

        // 两个解中取更大的正值
        float t1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
        float t2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);

        return Mathf.Max(t1, t2);
    }


}
