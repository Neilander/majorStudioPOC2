using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class moveUiControl : MonoBehaviour
{
    #region moveModule
    [Header("moveModule")]
    public float moveDuration = 1.0f; // 移动所需时间
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // 自定义移动曲线

    public List<UnityEvent> onMoveReachs; // 可在 Inspector 中设置的 UnityEvent
    public int triggerIndex = 0;
    private bool isMoving = false; // 是否正在移动
    private Vector2 startPosition; // 起始位置
    private Vector2 targetPosition; // 目标位置
    private float elapsedTime = 0; // 累计时间

    private RectTransform rectTransform; // UI 的 RectTransform

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); // 获取 RectTransform
    }

    void Update()
    {
        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    /// <summary>
    /// 开始平滑移动到指定位置
    /// </summary>
    /// <param name="destination">目标位置 (Vector2)</param>
    public void MoveTo(Vector2 destination, int i = 0)
    {
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform 未找到，确保此脚本附加在 UI 对象上！");
            return;
        }

        startPosition = rectTransform.anchoredPosition; // 记录起始位置
        targetPosition = destination; // 设置目标位置
        elapsedTime = 0; // 重置计时
        isMoving = true; // 开始移动
        triggerIndex = i;
    }

    /// <summary>
    /// 平滑移动到目标点
    /// </summary>
    private void MoveTowardsTarget()
    {
        if (!isMoving) return;

        // 增加时间
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / moveDuration); // 归一化时间 [0, 1]

        // 根据曲线计算插值
        float curveValue = moveCurve.Evaluate(t);
        rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, curveValue);

        // 检测是否完成移动
        if (t >= 1.0f)
        {
            rectTransform.anchoredPosition = targetPosition; // 确保完全对齐
            isMoving = false; // 停止移动
            onMoveReachs[triggerIndex]?.Invoke();
        }
    }

    #endregion
}
