using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class rollAnimal : MonoBehaviour
{
    public animalPrefabsInfo infos;
    public moveUiControl rightUi;
    public moveUiControl leftUi;

    public Vector2 rightUiUpAnchorPos;
    public Vector2 rightUiDownAnchorPos;

    public Vector2 leftUiLeftAnchorPos;
    public Vector2 leftUiRightAnchorPos;

    public Transform rollBasePos;
    public float gap;
    public int rollTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            rollAnimals();
            inShopStart_whenGameStart();
        }
            

        
    }

    public void rollAnimals()
    {
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

    public void inShopStart_whenGameStart()
    {
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
        
    }
}
