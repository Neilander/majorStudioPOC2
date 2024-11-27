using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bananaScript : MonoBehaviour
{

    public AnimationCurve movementCurve; // 控制运动进度的曲线
    public float g = 9.8f; // 重力加速度

    public float h;
    public float t;
    public Vector3 targetScale = new Vector3(0.8f, 0.8f, 0.8f); // 目标缩放比例
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(MoveInParabola(transform.position, transform.position + new Vector3(-4, 5), initialV, 9.8f, new Vector3(0.8f,0.8f,0.8f)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThrowObject(Vector3 end)
    {
        Vector3 start = transform.position;
        Vector3 midPoint = (start + end) / 2;
        midPoint.y += h;
        StartCoroutine(MoveInParabola(start, midPoint, end, t, targetScale));
    }

    /// <summary>
    /// 启动抛物线运动。
    /// </summary>
    /// <param name="start">起始点</param>
    /// <param name="end">终点</param>
    /// <param name="totalTime">运动所需的总时间</param>
    /// <param name="height">中点高度</param>
    public void ThrowObject(Vector3 start, Vector3 end, float totalTime, float height, Vector3 scale)
    {
        Vector3 midPoint = (start + end) / 2;
        midPoint.y += height;
        StartCoroutine(MoveInParabola(start, midPoint, end, totalTime, scale));
    }

    private IEnumerator MoveInParabola(Vector3 start, Vector3 midPoint, Vector3 end, float totalTime, Vector3 targetScale)
    {
        float elapsedTime = 0f;
        Vector3 initialScale = transform.localScale;

        while (elapsedTime < totalTime)
        {
            float normalizedTime = elapsedTime / totalTime;
            float curveProgress = movementCurve.Evaluate(normalizedTime);
            Vector3 position = CalculateBezierPoint(curveProgress, start, midPoint, end);
            transform.position = position;

            // 动态调整缩放比例
            transform.localScale = Vector3.Lerp(initialScale, targetScale, normalizedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        transform.localScale = targetScale; // 确保最终缩放比例准确
        doBanana();
    }

    /// <summary>
    /// 计算二次贝塞尔曲线上的点。
    /// </summary>
    /// <param name="t">曲线进度 [0, 1]</param>
    /// <param name="p0">起点</param>
    /// <param name="p1">中点</param>
    /// <param name="p2">终点</param>
    /// <returns>曲线上的点</returns>
    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // 二次贝塞尔曲线公式
        return (1 - t) * (1 - t) * p0 +
               2 * (1 - t) * t * p1 +
               t * t * p2;
    }


    public float radius = 5f; // 检测的圆形范围半径
    void doBanana()
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("animalTag"))
            {
                Debug.Log("Detected an object with animalTag: " + collider.gameObject.name);
            }
            else if (collider.CompareTag("ballTag"))
            {
                Debug.Log("Detected an object with ballTag: " + collider.gameObject.name);
            }
        }
    }
    // 绘制检测半径（仅在 Scene 视图中可见）
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
