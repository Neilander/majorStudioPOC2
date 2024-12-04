using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class explainText : MonoBehaviour
{

    public static explainText Instance { get; private set; }
    private explainType tp;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI expText;
    //public Image display;
    public MonoBehaviour curReporter;

    public RectTransform totalTrans;
    public GameObject controlObj;

    public List<string> names;
    public List<string> explainTexts;
    public List<GameObject> imgs;

    private GameObject lastImg;
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

        nameText.text = names[(int)type];
        expText.text = explainTexts[(int)type];
        if (lastImg != null)
            lastImg.SetActive(false);
        imgs[(int)type].SetActive(true);
        //display.GetComponent<RectTransform>().anchoredPosition = imgLocalAnchors[(int)type];
        lastImg = imgs[(int)type];
        Vector2 baseP = (Vector2)Camera.main.WorldToScreenPoint(reporter.transform.position) - new Vector2(960,540);
        Debug.Log(baseP);
        totalTrans.anchoredPosition = baseP+ (baseP.x<0 ?new Vector2(400,100):new Vector2(-400,100));
        controlObj.SetActive(true);
        tp = type;
        curReporter = reporter;
    }

    public void undoExplain(explainType type, MonoBehaviour reporter)
    {
        if (reporter == curReporter)
        {
            //expText.text = "Let them do the show\nand earn more score!";
            curReporter = null;
            controlObj.SetActive(false);
            if (lastImg != null)
                lastImg.SetActive(false);
        }
           
    }
}

public enum explainType
{
    monkey,
    elephant
}
