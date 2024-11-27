using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class explainText : MonoBehaviour
{

    public static explainText Instance { get; private set; }
    private explainType tp;
    public TextMeshProUGUI text;
    public MonoBehaviour curReporter;
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

    public void doExplain(explainType type, MonoBehaviour reporter)
    {
        if ((tp == type) && (reporter == curReporter))
            return;

        switch (type)
        {
            case explainType.dog1:
                text.text = "A common cute dog\n\npass the ball to the right,\ngain 50 score,\nand rest 2 turn";
                break;

            case explainType.dog2:
                text.text = "A special dog\n\npass the ball to the left 2,\ngain 50 score,\nand rest 3 turn";
                break;
        }

        tp = type;
        curReporter = reporter;
    }

    public void undoExplain(explainType type, MonoBehaviour reporter)
    {
        if (reporter == curReporter)
        {
            text.text = "Let them do the show\nand earn more score!";
            curReporter = null;
        }
           
    }
}

public enum explainType
{
    dog1,
    dog2,
    banana
}
