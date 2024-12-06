using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class rollAnimal : MonoBehaviour
{
    public animalPrefabsInfo infos;
    public moveUiControl rightUi;
    public moveUiControl leftUi;
    public moveUiControl downUi;

    public Vector2 rightUiUpAnchorPos;
    public Vector2 rightUiDownAnchorPos;

    public Vector2 leftUiLeftAnchorPos;
    public Vector2 leftUiRightAnchorPos;

    public Vector2 downUiDownAnchorPos;
    public Vector2 downUiUpAnchorPos;

    public Transform rollBasePos;
    public float gap;
    public int rollTime;

    public int rollTimePerTurn = 5;
    private int leftRollTime = 5;
    public TextMeshProUGUI rollText;

    public int maxTurn;
    private int curTurn;
    public TextMeshProUGUI turnText;


    private bool ifGameStart = false;

    // Start is called before the first frame update
    void Start()
    {
        animalManager.Instance.registerShopManger(this);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!ifGameStart)
        {
            inShopStart_whenGameStart();
            ifGameStart = true;
        }

        
            

        
    }

    public void rollAnimals()
    {
        if (leftRollTime < 1)
            return;
        leftRollTime -= 1;
        rollText.text = "Roll (" + (rollTimePerTurn - leftRollTime).ToString() + "/"+rollTimePerTurn+")";
        // 存储所有计算的动物位置
        List<Vector3> calculatedPositions = new List<Vector3>();

        // 判断奇偶数
        bool isOdd = rollTime % 2 != 0;

        // 计算起始位置
        float offset = isOdd ? 0 : gap / 2; // 奇数从中心开始，偶数偏移一半间距

        // 计算所有动物的位置
        for (int i = 0; i < rollTime; i++)
        {
            // 奇数分布公式
            int index = i - rollTime / 2;
            float posX = rollBasePos.position.x + index * gap + (isOdd ? 0 : offset);
            Vector3 position = new Vector3(posX, rollBasePos.position.y, rollBasePos.position.z);

            // 将计算好的位置添加到列表
            calculatedPositions.Add(position);
        }

        // 将计算的所有位置汇报给 AnimalManager
        animalManager.Instance.reportShopPos(calculatedPositions, rollBasePos);

        // 清理之前的子对象
        foreach (Transform child in rollBasePos)
        {
            Destroy(child.gameObject);
        }

        int n = 0;
        // 根据计算好的位置生成动物
        foreach (Vector3 position in calculatedPositions)
        {
            // 获取一个随机动物Prefab（假设infos提供了Prefab数组）
            GameObject animalPrefab = infos.GetRandomAnimal(); // infos需要实现此方法

            // 实例化动物
            if (animalPrefab != null)
            {
                GameObject newAnimal = Instantiate(animalPrefab, rollBasePos);
                newAnimal.transform.position = position;
                newAnimal.GetComponent<baseAnimalScript>().StartState(animalSceneState.inShop);
                newAnimal.GetComponent<baseAnimalScript>().selfIndex = 6 + n;
            }
            n += 1;
        }

    }

    void animalLeave()
    {
        foreach (baseAnimalScript an in animalManager.Instance.shopAnimals)
        {
            if (an != null)
            {
                //an.setNotDragable();
                an.leaveTo(an.transform.position + new Vector3(-20, 0, 0));
            }
        }
    }

    void animalUp()
    {
        for (int i = 0; i < 6; i++)
        {
            baseAnimalScript baseAn = animalManager.Instance.allAnimals[i];
            if (baseAn != null)
            {
                baseAnimalScript currentBaseAn = baseAn;
                int currentIndex = i;
                //currentBaseAn.transform.position = animalManager.Instance.gameStartPos[currentIndex];

                // 在委托中使用局部副本
                currentBaseAn.MoveTo(animalManager.Instance.inShowSixPos[currentIndex], () =>
                {
                    Debug.Log($"BaseAnimal {currentBaseAn.name} 已完成移动！");
                    // 这里可以让 currentBaseAn 执行一些逻辑
                    //currentBaseAn.setDragable();
                    currentBaseAn.StartState(animalSceneState.inShow);
                });
            }



        }

        downUi.MoveTo(downUiUpAnchorPos);
    }

    public void inShopStart_whenGameStart()
    {
        leftRollTime = rollTimePerTurn + 1;
        curTurn = maxTurn;
        turnText.text = curTurn.ToString();
        rollAnimals();
        for (int i = 0; i < 6; i++)
        {
            baseAnimalScript baseAn = animalManager.Instance.allAnimals[i];
            if (baseAn != null)
            {
                baseAnimalScript currentBaseAn = baseAn;
                int currentIndex = i;
                currentBaseAn.transform.position = animalManager.Instance.gameStartPos[currentIndex];

                // 在委托中使用局部副本
                currentBaseAn.MoveTo(animalManager.Instance.inShopSixPos[currentIndex], () =>
                {
                    Debug.Log($"BaseAnimal {currentBaseAn.name} 已完成移动！");
                    // 这里可以让 currentBaseAn 执行一些逻辑
                    //currentBaseAn.setDragable();
                    currentBaseAn.StartState(animalSceneState.inShop);
                });
            }
                


        }

        rightUi.GetComponent<RectTransform>().anchoredPosition = rightUiUpAnchorPos;
        rightUi.GetComponent<rightUiControl>().downInteract();
        rightUi.MoveTo(rightUiDownAnchorPos);
        leftUi.GetComponent<RectTransform>().anchoredPosition = leftUiLeftAnchorPos;
        leftUi.MoveTo(leftUiRightAnchorPos);
        downUi.GetComponent<RectTransform>().anchoredPosition = downUiDownAnchorPos;
        
    }

    public void inShopEnd_WhenTurnEnd()
    {
        if (!animalManager.Instance.canStartShow)
            return;
        animalLeave();

        foreach (baseAnimalScript an in animalManager.Instance.allAnimals)
            an.setNotDragable();
        rightUi.GetComponent<rightUiControl>().downInteract();
        rightUi.MoveTo(rightUiUpAnchorPos);
        //Debug.Log("为什么没动");
        leftUi.MoveTo(leftUiLeftAnchorPos);
        Invoke("animalUp", 1.5f);
        animalManager.Instance.clearCheckForShow();
        animalManager.Instance.isShop = false;

    }

    public void inShopStart_WhenTurnStart()
    {
        leftRollTime = rollTimePerTurn + 1;
        curTurn -= 1;
        turnText.text = curTurn.ToString();


        for (int i = 0; i < 6; i++)
        {
            baseAnimalScript baseAn = animalManager.Instance.allAnimals[i];
            if (baseAn != null)
            {
                baseAnimalScript currentBaseAn = baseAn;
                int currentIndex = i;
                //currentBaseAn.transform.position = animalManager.Instance.gameStartPos[currentIndex];

                // 在委托中使用局部副本
                currentBaseAn.MoveTo(animalManager.Instance.inShopSixPos[currentIndex], () =>
                {
                    //Debug.Log($"BaseAnimal {currentBaseAn.name} 已完成移动！");
                    // 这里可以让 currentBaseAn 执行一些逻辑
                    //currentBaseAn.setDragable();
                    currentBaseAn.StartState(animalSceneState.inShop);
                });
            }



        }
        downUi.MoveTo(downUiDownAnchorPos,1);
        //rightUi.GetComponent<RectTransform>().anchoredPosition = rightUiUpAnchorPos;
        rightUi.GetComponent<rightUiControl>().downInteract();
        rightUi.MoveTo(rightUiDownAnchorPos);
        //leftUi.GetComponent<RectTransform>().anchoredPosition = leftUiLeftAnchorPos;
        leftUi.MoveTo(leftUiRightAnchorPos);
        //downUi.GetComponent<RectTransform>().anchoredPosition = downUiDownAnchorPos;

        Invoke("rollAnimals", 0.3f);
    }
}
