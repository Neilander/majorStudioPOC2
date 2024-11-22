using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animalManager : MonoBehaviour
{
    public static animalManager Instance { get; private set; }

    private baseAnimalScript[] allAnimals;

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

    public void registerAnimal(int n, baseAnimalScript an)
    {
        if(allAnimals == null)
            allAnimals = new baseAnimalScript[6];
        allAnimals[n] = an;
    }

    public void ballToIndex(ballScript ball, int index)
    {
        Debug.Log("来了");
        if (index < allAnimals.Length && (allAnimals[index] != null))
        {
            allAnimals[index].TakeBall(ball);
        }
    }
}
