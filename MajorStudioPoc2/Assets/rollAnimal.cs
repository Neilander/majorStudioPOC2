using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rollAnimal : MonoBehaviour
{
    public animalPrefabsInfo infos;

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
            rollAnimals();
    }

    public void rollAnimals()
    {
        // 清理之前的子对象（可选）
        foreach (Transform child in rollBasePos)
        {
            Destroy(child.gameObject);
        }

        // 判断奇偶数
        bool isOdd = rollTime % 2 != 0;

        // 计算起始位置
        float offset = isOdd ? 0 : gap / 2; // 奇数从中心开始，偶数偏移一半间距

        // 生成动物
        for (int i = 0; i < rollTime; i++)
        {
            // 奇数分布公式
            int index = i - rollTime / 2;
            float posX = rollBasePos.position.x + index * gap + (isOdd ? 0 : offset);

            // 获取一个随机动物Prefab（假设infos提供了Prefab数组）
            GameObject animalPrefab = infos.GetRandomAnimal(); // infos需要实现此方法

            // 实例化动物
            if (animalPrefab != null)
            {
                GameObject newAnimal = Instantiate(animalPrefab, rollBasePos);
                newAnimal.transform.position = new Vector3(posX, rollBasePos.position.y, rollBasePos.position.z);
            }
        }
    }

    public void inShopStart()
    {

    }
}
