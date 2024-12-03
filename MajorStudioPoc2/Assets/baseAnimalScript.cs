using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class baseAnimalScript : MonoBehaviour
{
    internal bool ifReady = true;
    public List<Sprite> displaySprites;
    public SpriteRenderer renderer;
    public float flipDuration = 0.5f;
    internal bool ifHaveBall;
    internal bool ifJustInteract;
    internal int curRestTurn;
    internal ballScript ball;
    private Vector3 originalScale;


    
    [Header("动物基础数值")]
    public int restTurn;
    public int selfIndex;
    public int baseIndexAdd;
    public float throwTime;
    public float throwHeight;
    public Transform AcceptPos;
    public Transform throwPos;
    public int interactionScore = 50;
    public explainType type;

    //这是展示用的
    private int curState = 0;
    public TextMeshProUGUI text;

    
    public animalSceneState Scene_curState;
    public bool canBeDrag = false;

    
    // Start is called before the first frame update
    void Start()
    {

        animalManager.Instance.registerAnimal(selfIndex, this, out text);
        originalScale = transform.localScale; // 记录原始缩放
        curState = -1;
        ChangeDisplay(0);
        //StartState(animalSceneState.inShop);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        
        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    public void StartState(animalSceneState newState)
    {
        EndState(Scene_curState);
        switch (newState)
        {
            case animalSceneState.inShop:
                canBeDrag = true;
                break;

            case animalSceneState.inShow:
                canBeDrag = false;
                animalManager.Instance.reportReachShow_Animal(this);
                break;
        }
        Scene_curState = newState;
    }

    void EndState(animalSceneState lastState)
    {
        switch (lastState)
        {
            case animalSceneState.inShop:
                break;

            case animalSceneState.inShow:
                doShowEnd();
                break;
        }
    }

    void UpdateState()
    {
        switch (Scene_curState)
        {
            case animalSceneState.inShop:
                HandleInShopState();
                
                break;

            case animalSceneState.inShow:
                //Debug.Log("什么情况？");
                break;
        }
    }

    void doShowEnd()
    {
        curRestTurn = 0;
        ifJustInteract = false;
        ifHaveBall = false;
        ifReady = true;
        ChangeDisplay(0);
        ChangeRestCount(-1);
        Invoke("reportFlipFinish", 1f);
    }

    void reportFlipFinish()
    {
        animalManager.Instance.reportFlipEnd(this);
    }

    #region drag module

    void HandleInShopState()
    {

        if (Scene_curState == animalSceneState.inShow)
            return;
        if (Input.GetMouseButtonDown(0)&& canBeDrag) // 鼠标左键按下
        {
            // 将鼠标屏幕坐标转换为世界坐标
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 从鼠标位置发出 2D 射线
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);

            // 检测射线是否命中
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject) // 检查命中的是否是自己
                {
                    OnMouseDragStart(hit.point); // 开始拖动
                }
            }
        }

        if (Input.GetMouseButton(0) && isDragging) // 鼠标左键按住且正在拖动
        {
            OnMouseDragObj();
        }

        if (Input.GetMouseButtonUp(0)) // 鼠标左键松开
        {
            OnMouseRelease(); // 停止拖动
        }
    }

    // 是否正在拖动
    private bool isDragging = false;
    // 拖动起始点的偏移
    private Vector3 dragOffset;

    void OnMouseDragStart(Vector3 hitPoint)
    {
        isDragging = true;
        dragOffset = transform.position - hitPoint; // 记录点击点与对象中心的偏移
        Debug.Log($"{gameObject.name}: Start dragging!");
        canBeDrag = false;
        animalManager.Instance.reportStartDrag();

    }

    
    void OnMouseDragObj()
    {
        // 获取鼠标屏幕坐标并转换为世界坐标
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // 将 Z 坐标设置为 0（适用于 2D 平面）
        mouseWorldPosition.z = 0;

        // 更新拖动对象的位置，考虑偏移
        transform.position = mouseWorldPosition + dragOffset;
    }

    void OnMouseRelease()
    {
        Debug.Log("至少到这里了");
        if (isDragging) // 仅在拖动状态下执行释放逻辑
        {
            isDragging = false;
            Debug.Log($"{gameObject.name}: Stop dragging!");
            //canBeDrag = true;
            animalManager.Instance.handleMoveToPosAfterDrag(selfIndex, this);
        }
    }

    public void setDragable() { canBeDrag = true; }
    public void setNotDragable() { canBeDrag = false; }
    #endregion

    #region moveModule
    [Header("moveModule")]
    public float moveDuration = 1.0f; // 移动所需时间
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // 自定义移动曲线

    public float leaveDur = 1.5f;
    public AnimationCurve leaveCurve;

    private bool isMoving = false; // 是否正在移动
    private Vector3 startPosition; // 起始位置
    private Vector3 targetPosition; // 目标位置
    private float elapsedTime = 0; // 累计时间
    private Action onMoveComplete; // 移动完成后的回调
    private bool ifLeave = false;

    /// <summary>
    /// 开始平滑移动到指定位置
    /// </summary>
    /// <param name="destination">目标位置 (Vector2)，内部自动转换为 Z=0 的 Vector3</param>
    /// <param name="onComplete">可选的回调，移动完成后执行</param>
    public void MoveTo(Vector2 destination, Action onComplete = null)
    {
        startPosition = transform.position; // 记录起始位置
        targetPosition = new Vector3(destination.x, destination.y, 0); // 转换目标位置为 Vector3
        onMoveComplete = onComplete; // 存储回调
        elapsedTime = 0; // 重置计时
        isMoving = true; // 开始移动
        ifLeave = false;
    }

    public void MoveToForDrag(Vector2 destination)
    {
        startPosition = transform.position; // 记录起始位置
        targetPosition = new Vector3(destination.x, destination.y, 0); // 转换目标位置为 Vector3
        onMoveComplete = () => { canBeDrag = true; };
        elapsedTime = 0; // 重置计时
        isMoving = true; // 开始移动
        ifLeave = false;
    }

    public void leaveTo(Vector2 destination)
    {
        startPosition = transform.position; // 记录起始位置
        targetPosition = new Vector3(destination.x, destination.y, 0); // 转换目标位置为 Vector3
        elapsedTime = 0; // 重置计时
        isMoving = true; // 开始移动
        ifLeave = true;
        canBeDrag = false;
        StartSway();
        
    }

    /// <summary>
    /// 平滑移动到目标点
    /// </summary>
    private void MoveTowardsTarget()
    {
        if (!isMoving) return;


        // 增加时间
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / (ifLeave?leaveDur: moveDuration)); // 归一化时间 [0, 1]

        // 根据曲线计算插值
        float curveValue = ifLeave? leaveCurve.Evaluate(t):  moveCurve.Evaluate(t);
        transform.position = Vector3.Lerp(startPosition, targetPosition, curveValue);

        // 检测是否完成移动
        if (t >= 1.0f)
        {
            transform.position = targetPosition; // 确保完全对齐
            isMoving = false; // 停止移动

            // 执行回调（如果存在）
            onMoveComplete?.Invoke();
        }
    }

    

    #endregion

    private bool IsMouseOverSprite(Vector3 mousePosition, SpriteRenderer spriteRenderer)
    {
        // 获取 Sprite 的边界
        Bounds bounds = spriteRenderer.bounds;

        // 仅检查 2D 平面上的位置（忽略 Z 轴）
        return bounds.Contains(new Vector3(mousePosition.x, mousePosition.y, bounds.center.z));
    }

    

    public void DoTurnStart()
    {
        Debug.Log(name+"开始了");   
        //如果有球
        if (ifHaveBall)
        {
            InterActWithBall();
        }
        else if(!ifReady)
        {
            //如果无球并且在休息状态，就休息减1
            DoRest();
        }
        //否则就什么都不做

    }

    public void DoTurnEnd()
    {
        if (ifJustInteract)
        {

            ChangeDisplay(2);
            ChangeRestCount(restTurn);
            ifJustInteract = false;
        }
        else if (curRestTurn <= 0)
        {
            recover();
        }
    }

    public virtual void TakeBall(ballScript b)
    {
        
        if (ifHaveBall || (!ifReady))
        {

            b.doDrop();
        }
        else
        {
            ball = b;
            b.gameObject.SetActive(false);
            ifHaveBall = true;
        }
    }

    public virtual void TakeBanana(int n)
    {
        if (animalManager.Instance.ifShowEnd)
            return;
        curRestTurn = Mathf.Max(curRestTurn - n, 0);
        if (curRestTurn == 0)
        {
            recover();
        }
        else
        {
            ChangeRestCount(curRestTurn);
        }
    }

    public virtual void InterActWithBall()
    {
        ball.gameObject.SetActive(true);
        ball.MoveBall(selfIndex, selfIndex+baseIndexAdd);
        ChangeDisplay(1);
        ifJustInteract = true;
        ifHaveBall = false;
        animalManager.Instance.changeScore(interactionScore);
        ifReady = false;
    }

    public virtual void DoRest()
    {
        curRestTurn = Mathf.Max(curRestTurn - 1, 0);
        ChangeRestCount(curRestTurn);
    }

    public virtual void DoDrop()
    {
        Debug.Log("球掉了");
    }

    private void recover()
    {
        ChangeDisplay(0);
        ChangeRestCount(-1);
        ifReady = true;
    }

    

    /// <summary>
    /// 传入小于0的数字会让休息数字关闭
    /// </summary>
    /// <param name="turn"></param>
    internal void ChangeRestCount(int turn)
    {
        if (selfIndex >= 6)
            return;
        curRestTurn = turn;
        if (text == null)
            animalManager.Instance.registerAnimal(selfIndex, this, out text);

        
        if (turn >= 0)
        {
            text.text = curRestTurn.ToString();
            if (!text.gameObject.activeInHierarchy)
                text.gameObject.SetActive(true);

        }
        else
        {
            text.gameObject.SetActive(false);
        }
        

    }

    /// <summary>
    /// 0代表充能态，1代表动作态，2代表休息态
    /// </summary>
    /// <param name="toState"></param>
    public virtual void ChangeDisplay(int toState)
    {


        if (curState == toState)
            return;

        curState = toState;
        if (toState >= 0 && toState < displaySprites.Count)
        {
            StartCoroutine(FlipSprite(toState));
        }
        else
        {
            Debug.LogWarning("Invalid state index: " + toState);
        }

        if (toState == 2)
            renderer.color = Color.gray;
        else
            renderer.color = Color.white;
    }

    private IEnumerator FlipSprite(int toState)
    {
        float halfDuration = flipDuration / 2f; // 翻转一半的时间
        Vector3 scale = transform.localScale;

        // 第一阶段：逐渐缩小到 0
        float elapsedTime = 0f;
        while (elapsedTime < halfDuration)
        {
            float t = elapsedTime / halfDuration;
            transform.localScale = Vector3.Lerp(originalScale, new Vector3(0, originalScale.y, originalScale.z), t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保缩放为 0
        transform.localScale = new Vector3(0, originalScale.y, originalScale.z);

        // 更改 Sprite
        renderer.sprite = displaySprites[toState];

        // 第二阶段：逐渐恢复到原始缩放
        elapsedTime = 0f;
        while (elapsedTime < halfDuration)
        {
            float t = elapsedTime / halfDuration;
            transform.localScale = Vector3.Lerp(new Vector3(0, originalScale.y, originalScale.z), originalScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保缩放恢复原值
        transform.localScale = originalScale;
    }

    #region swayModule

    public float swayAngle = 15f; // 最大摆动角度
    public float swaySpeed = 2f;  // 摇晃速度

    private Coroutine swayCoroutine;

    /// <summary>
    /// 开始左右摇晃
    /// </summary>
    public void StartSway()
    {
        if (swayCoroutine == null)
        {
            swayCoroutine = StartCoroutine(Sway());
        }
    }

    /// <summary>
    /// 停止左右摇晃
    /// </summary>
    public void StopSway()
    {
        if (swayCoroutine != null)
        {
            StopCoroutine(swayCoroutine);
            swayCoroutine = null;
            transform.rotation = Quaternion.identity; // 恢复为初始角度
        }
    }

    /// <summary>
    /// 左右摇晃的协程
    /// </summary>
    private IEnumerator Sway()
    {
        float timer = 0f;

        while (true)
        {
            timer += Time.deltaTime * swaySpeed;
            float angle = Mathf.Sin(timer) * swayAngle; // 使用正弦函数计算当前角度
            transform.rotation = Quaternion.Euler(0, 0, angle); // 应用摆动角度（Z 轴）
            yield return null; // 等待下一帧
        }
    }

    #endregion


    #region shakeModule

    public float shakeAmount = 1f; // 左右晃动的距离
    public float shakeSpeed = 2f;  // 晃动的速度
    public float shakeDuration = 2f; // 晃动持续时间
    private Coroutine shakeCoroutine;

    /// <summary>
    /// 启动晃动
    /// </summary>
    public void StartShaking()
    {
        // 如果有正在运行的协程，先停止
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }

        // 启动新的晃动协程
        shakeCoroutine = StartCoroutine(Shake(null));
    }

    public void StartShakingForEndShow()
    {
        // 如果有正在运行的协程，先停止
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }

        // 启动新的晃动协程
        shakeCoroutine = StartCoroutine(Shake(() => { StartState(animalSceneState.empty); }));
    }

    /// <summary>
    /// 晃动的协程
    /// </summary>
    private IEnumerator Shake(Action endAction)
    {
        float timer = 0f;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.localPosition; // 记录初始位置

        while (elapsedTime < shakeDuration)
        {
            timer += Time.deltaTime * shakeSpeed;
            elapsedTime += Time.deltaTime;

            float offset = Mathf.Sin(timer) * shakeAmount; // 使用正弦函数计算左右偏移量
            transform.localPosition = new Vector3(startPosition.x + offset, startPosition.y, startPosition.z); // 应用偏移

            yield return null; // 等待下一帧
        }

        // 恢复到初始位置
        transform.localPosition = startPosition;

        // 结束晃动
        shakeCoroutine = null;
        endAction?.Invoke();
    }
    #endregion

    
}


public enum animalSceneState
{
    empty,
    inShop,
    inShow
}