using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class animalManager : MonoBehaviour
{
    public static animalManager Instance { get; private set; }

    private baseAnimalScript[] allAnimals;
    public Transform rightPos;
    public Transform leftpos;
    public List<Transform> basePos;
    public bool ifShowEnd = false;

    private scoreScript curDisplay;
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
        if(Input.GetKeyDown(KeyCode.R))
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

    public baseAnimalScript returnFirstAnimal()
    {
        foreach (baseAnimalScript an in allAnimals)
        {
            if (an != null)
                return an;
        }
        return null;
    }

    public bool returnAnimalWithIndex(int i, bool ifThrowPos,out Vector3 pos)
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
}
