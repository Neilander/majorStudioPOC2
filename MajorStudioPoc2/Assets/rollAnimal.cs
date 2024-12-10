using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

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

    [Header("教程相关")]
    public Vector2 MonkeyStartPos;
    public Vector2 MonkeyEndPos;
    public moveUiControl monkeyUi;
    public float maxAngle = 30f; // 最大旋转角度
    public float duration = 1f; // 整个动画的时长
    public Vector2 pivotOffset = new Vector2(50, 50);
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public List<string> dialogues;
    public int firstStageIndex;
    public List<Button> allButtonsToChangeAble;
    private int curIndex = 0;
    public GameObject tutorialPanel;
    public Sprite indexTwoBgImage;
    public Vector2 indexTwoMonkeyPos;
    public Vector2 indexTwoDialoguePos;
    public Quaternion indexTwoRotation;
    public Sprite indexThreeBgImage;
    public Vector2 indexThreeMonkeyPos;
    public Vector2 indexThreeDialoguePos;
    public Quaternion indexThreeRotation;

    public Sprite indexFourBgImage;
    public Vector2 indexFourMonkeyPos;
    public Vector2 indexFourDialoguePos;

    public Sprite indexFiveBgImage;
    public Vector2 indexFiveMonkeyPos;
    public Vector2 indexFiveDialoguePos;

    public Sprite indexSevenBgImage;
    public Vector2 indexSevenMonkeyPos;
    public Vector2 indexSevenDialoguePos;

    public Vector2 startDialoguePos;

    public Image introBg;
    public Sprite allblack;

    private bool ifGameStart = false;
    private bool dialogueStart = false;

    private bool firstRoll = false;
    private bool bananaIntroEd = false;
    private bool canClick = false;

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

        if (animalManager.Instance.inTutorial&& dialogueStart && canClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("来");
                if (curIndex < firstStageIndex)
                {
                    curIndex += 1;
                    dialogueText.text = dialogues[curIndex];
                    if (curIndex == 2)
                    {
                        monkeyUi.GetComponent<RectTransform>().anchoredPosition = indexTwoMonkeyPos;
                        monkeyUi.GetComponent<RectTransform>().rotation = indexTwoRotation;
                        dialoguePanel.GetComponent<RectTransform>().anchoredPosition = indexTwoDialoguePos;
                        introBg.sprite = indexTwoBgImage;
                    }
                    else if (curIndex == 3)
                    {
                        monkeyUi.GetComponent<RectTransform>().anchoredPosition = indexThreeMonkeyPos;
                        monkeyUi.GetComponent<RectTransform>().rotation = indexThreeRotation;
                        dialoguePanel.GetComponent<RectTransform>().anchoredPosition = indexThreeDialoguePos;
                        introBg.sprite = indexThreeBgImage;
                    }
                    else if (curIndex == 4)
                    {
                        monkeyUi.GetComponent<RectTransform>().anchoredPosition = indexFourMonkeyPos;
                        dialoguePanel.GetComponent<RectTransform>().anchoredPosition = indexFourDialoguePos;
                        introBg.sprite = indexFourBgImage;
                    }
                    else if (curIndex == 5)
                    {
                        monkeyUi.GetComponent<RectTransform>().anchoredPosition = indexFiveMonkeyPos;
                        dialoguePanel.GetComponent<RectTransform>().anchoredPosition = indexFiveDialoguePos;
                        introBg.sprite = indexFiveBgImage;
                    }

                }
                else if ((curIndex + 1) < dialogues.Count)
                {
                    if (!bananaIntroEd)
                    {
                        animalManager.Instance.inTutorial = false;
                        tutorialPanel.SetActive(false);
                        foreach (Button btn in allButtonsToChangeAble)
                        {
                            btn.interactable = true;
                        }
                    }
                    else
                    {
                        curIndex += 1;
                        dialogueText.text = dialogues[curIndex];

                        if (curIndex == 7)
                        {
                            monkeyUi.GetComponent<RectTransform>().anchoredPosition = indexSevenMonkeyPos;
                            dialoguePanel.GetComponent<RectTransform>().anchoredPosition = indexSevenDialoguePos;
                            introBg.sprite = indexSevenBgImage;
                        }
                    }
                }
                else
                {
                    animalManager.Instance.inTutorial = false;
                    tutorialPanel.SetActive(false);
                    foreach (Button btn in allButtonsToChangeAble)
                    {
                        btn.interactable = true;
                    }
                }
            }
        }
            

        
    }

    public void rollAnimals()
    {
        if (animalManager.Instance.inTutorial&& firstRoll)
            return;
        if (leftRollTime < 1)
            return;

        firstRoll = true;
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

        if(animalManager.Instance.BananaEnable)
            downUi.MoveTo(downUiUpAnchorPos);
    }

    public void inShopStart_whenGameStart()
    {
        startExplain();
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
        //leftUi.MoveTo(leftUiRightAnchorPos);
        downUi.GetComponent<RectTransform>().anchoredPosition = downUiDownAnchorPos;
        
    }

    public void inShopEnd_WhenTurnEnd()
    {
        if (animalManager.Instance.inTutorial)
            return;
        if (!animalManager.Instance.canStartShow)
            return;
        animalLeave();

        foreach (baseAnimalScript an in animalManager.Instance.allAnimals)
            an.setNotDragable();
        rightUi.GetComponent<rightUiControl>().downInteract();
        rightUi.MoveTo(rightUiUpAnchorPos);
        //Debug.Log("为什么没动");
        if(animalManager.Instance.BananaEnable)
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

        if (!bananaIntroEd)
        {
            bananaIntroEd = true;
            animalManager.Instance.BananaEnable = true;
            animalManager.Instance.inTutorial = true;
            tutorialPanel.SetActive(true);
            foreach (Button btn in allButtonsToChangeAble)
            {
                btn.interactable = false;
            }
            startExplain();
            curIndex += 1;
            introBg.sprite = allblack;
            firstRoll = false;
        }
        


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
        if(animalManager.Instance.BananaEnable)
            leftUi.MoveTo(leftUiRightAnchorPos);
        //downUi.GetComponent<RectTransform>().anchoredPosition = downUiDownAnchorPos;

        Invoke("rollAnimals", 0.3f);
    }

    void startExplain()
    {
        monkeyUi.GetComponent<RectTransform>().anchoredPosition = MonkeyStartPos;
        dialoguePanel.GetComponent<RectTransform>().anchoredPosition = startDialoguePos;
        dialoguePanel.SetActive(false);
        monkeyUi.MoveTo(MonkeyEndPos);
        canClick = false;
    }

    public void startExplainStageTwo()
    {
        StartCoroutine(RotateAroundPoint());
    }

    private IEnumerator RotateAroundPoint()
    {
        RectTransform rectTransform = monkeyUi.GetComponent<RectTransform>();
        Vector2 originalAnchoredPosition = rectTransform.anchoredPosition;

        // 计算旋转的中心点（相对于 RectTransform 的位置）
        Vector2 rotationCenter = originalAnchoredPosition + pivotOffset;

        // 向右旋转：快到慢
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float currentAngle = Mathf.Lerp(0, maxAngle, Mathf.Sin(t * Mathf.PI * 0.5f)); // 快到慢

            // 计算绕旋转中心点旋转后的位置
            rectTransform.anchoredPosition = RotatePoint(originalAnchoredPosition, rotationCenter, currentAngle);
            rectTransform.rotation = Quaternion.Euler(0, 0, currentAngle);

            yield return null;
        }

        // 回转：慢到快
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float currentAngle = Mathf.Lerp(maxAngle, 0, 1 - Mathf.Cos(t * Mathf.PI * 0.5f)); // 慢到快

            // 计算绕旋转中心点旋转后的位置
            rectTransform.anchoredPosition = RotatePoint(originalAnchoredPosition, rotationCenter, currentAngle);
            rectTransform.rotation = Quaternion.Euler(0, 0, currentAngle);

            yield return null;
        }

        // 确保回到初始位置和角度
        rectTransform.anchoredPosition = originalAnchoredPosition;
        rectTransform.rotation = Quaternion.Euler(0, 0, 0);

        dialoguePanel.SetActive(true);
        dialogueText.text = dialogues[curIndex];
        dialogueStart = true;
        canClick = true;
    }

    // 计算点绕另一个点旋转后的新位置
    private Vector2 RotatePoint(Vector2 point, Vector2 pivot, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radian);
        float sin = Mathf.Sin(radian);

        Vector2 direction = point - pivot;
        Vector2 rotatedDirection = new Vector2(
            cos * direction.x - sin * direction.y,
            sin * direction.x + cos * direction.y
        );

        return pivot + rotatedDirection;
    }
}
