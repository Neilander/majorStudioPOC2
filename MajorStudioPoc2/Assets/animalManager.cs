using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class animalManager : MonoBehaviour
{
    public static animalManager Instance { get; private set; }

    public baseAnimalScript[] allAnimals;
    public baseAnimalScript[] shopAnimals;
    public Transform rightPos;
    public Transform leftpos;
    public List<Transform> basePos;
    public bool ifShowEnd = false;
    public List<TextMeshProUGUI> texts;

    public List<Vector3> gameStartPos;
    public List<Vector3> inShopSixPos;
    public List<Vector3> shopPos;


    private scoreScript curDisplay;
    public bananaLeft curLeft;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 如果已经存在一个实例，销毁当前对象
        }
        else
        {
            Instance = this; // 设置为单例实例
            DontDestroyOnLoad(gameObject); // 保持跨场景不销毁
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void turnStart()
    {
        foreach (baseAnimalScript an in allAnimals)
        {
            if (an != null)
                an.DoTurnStart();
        }
    }

    public void turnEnd()
    {
        foreach (baseAnimalScript an in allAnimals)
        {
            if (an != null)
                an.DoTurnEnd();
        }
    }

    public void registerAnimal(int n, baseAnimalScript an, out TextMeshProUGUI coolText)
    {
        if (n < 6)
        {
            if (allAnimals == null)
                allAnimals = new baseAnimalScript[6];
            allAnimals[n] = an;
            coolText = texts[n];
        }
        else
        {
            if (shopAnimals == null || (shopAnimals.Length<6))
                shopAnimals = new baseAnimalScript[6];
            shopAnimals[n-6] = an;
            coolText = null;

        }

        
    }

    public void ballToIndex(ballScript ball, int index)
    {
        Debug.Log("来了");
        if (index < allAnimals.Length && (allAnimals[index] != null))
        {
            allAnimals[index].TakeBall(ball);
        }
    }

    public baseAnimalScript returnFirstAnimal()
    {
        foreach (baseAnimalScript an in allAnimals)
        {
            if (an != null)
                return an;
        }
        return null;
    }

    public bool returnAnimalWithIndex(int i, bool ifThrowPos, out Vector3 pos)
    {
        if (allAnimals[i] == null)
        {
            pos = Vector3.zero;
            return false;
        }
        pos = ifThrowPos ? allAnimals[i].throwPos.position : allAnimals[i].AcceptPos.position;
        return true;
    }

    public bool returnAnimalBasedOnIndex(int n, out baseAnimalScript animal)
    {
        if (allAnimals[n] == null)
        {
            animal = null;
            return false;
        }
        animal = allAnimals[n];
        return true;
    }

    public Vector3 returnEndIndex(bool ifRight)
    {
        if (ifRight)
            return rightPos.position;
        else
            return leftpos.position;
    }

    public Vector3 returnAnimalBasicPos(int n)
    {
        int k = Mathf.Clamp(n, 0, 5);
        return basePos[k].position;
    }

    public void reportScoreText(scoreScript score)
    {
        curDisplay = score;
    }

    public void changeScore(int n)
    {
        curDisplay.ChangeScore(n);
    }

    public void reportText(TextMeshProUGUI t, int n)
    {
        texts[n] = t;
        Debug.Log("report！" + (texts[n] == null));
    }

    public void reportLeftRightPos(Transform t, bool ifLeft)
    {
        if (ifLeft)
            leftpos = t;
        else
            rightPos = t;
    }

    public void reportBasePos(Transform t, int n)
    {
        basePos[n] = t;
    }

    public void reportBananaText(bananaLeft left)
    {
        curLeft = left;
    }

    public void reportShopPos(List<Vector3> shopPoses, Transform basePos)
    {
        shopPos = shopPoses;
        shopFather = basePos;
    }

    /*
     * 关于移动交互的
     */
    public float detectionRadius;
    public float shopDetectionRadius;
    private Transform shopFather;
    private int currentMovingIndex;

    public void reportMove(int index)
    {
        currentMovingIndex = index;
    }

    /// <summary>
    /// 检测鼠标是否在某个点的指定半径内
    /// </summary>
    /// <param name="n">返回点的索引，如果在 inShopSixPos 中，返回对应索引；如果在 shopPos 中，返回索引 + 6</param>
    /// <returns>如果鼠标在任意点的半径内返回 true，否则返回 false</returns>
    public bool detectMouseInCircle(out int n)
    {
        n = -1; // 初始化输出参数

        // 获取鼠标在 z = 0 平面的位置
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); // 将屏幕坐标转换为世界坐标
        mousePosition.z = 0; // 将 z 轴固定为 0，转换为 2D 检测

        // 遍历 inShopSixPos
        for (int i = 0; i < inShopSixPos.Count; i++)
        {
            float distance = Vector2.Distance(new Vector2(mousePosition.x, mousePosition.y), new Vector2(inShopSixPos[i].x, inShopSixPos[i].y));
            if (distance <= detectionRadius)
            {
                n = i; // 返回索引
                return true;
            }
        }

        // 遍历 shopPos
        for (int i = 0; i < shopPos.Count; i++)
        {
            float distance = Vector2.Distance(new Vector2(mousePosition.x, mousePosition.y), new Vector2(shopPos[i].x, shopPos[i].y));
            if (distance <= shopDetectionRadius)
            {
                n = i + 6; // 返回索引 + 6
                return true;
            }
        }

        return false; // 如果没有在任何点的范围内
    }

    public void handleMoveToPosAfterDrag(int n, baseAnimalScript caller)
    {
        int probN;

        //移动要注意3个，位置，index，和coolText,最后该在存储里的位置
        if (detectMouseInCircle(out probN))
        {
            baseAnimalScript toPosAn = (probN < 6) ? allAnimals[probN] : shopAnimals[probN - 6];

            if (toPosAn != null)
            {
                toPosAn.selfIndex = n;
                toPosAn.text = caller.text;
                toPosAn.MoveToForDrag((n < 6) ? inShopSixPos[n] : shopPos[n - 6]);
                ((n < 6) ? allAnimals : shopAnimals)[(n < 6)?n:(n-6)] = toPosAn;
                toPosAn.transform.SetParent((n < 6) ? null : shopFather);
                
            }
            caller.selfIndex = probN;
            caller.text = (probN < 6) ? texts[probN] : null;
            caller.MoveToForDrag((probN < 6) ? inShopSixPos[probN] : shopPos[probN - 6]);
            ((probN < 6) ? allAnimals : shopAnimals)[(probN < 6) ? probN : (probN - 6)] = caller;
            caller.transform.SetParent((probN < 6) ? null : shopFather);
        }
        else
        {
            Debug.Log("回老位置");
            caller.MoveToForDrag((n < 6) ? inShopSixPos[n] : shopPos[n - 6]);
        }
    }

    private void OnDrawGizmos()
    {
        // 绘制 inShopSixPos 的圆
        Gizmos.color = Color.green; // 设置颜色
        if (inShopSixPos != null)
        {
            foreach (Vector3 position in inShopSixPos)
            {
                Gizmos.DrawWireSphere(position, detectionRadius); // 绘制圆
            }
        }

        // 绘制 shopPos 的圆
        Gizmos.color = Color.blue; // 更改颜色
        if (shopPos != null)
        {
            foreach (Vector3 position in shopPos)
            {
                Gizmos.DrawWireSphere(position, shopDetectionRadius); // 绘制圆
            }
        }
    }
}