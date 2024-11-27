using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseAnimalScript : MonoBehaviour
{
    internal bool ifReady = true;
    public List<Sprite> displaySprites;
    public SpriteRenderer renderer;
    internal bool ifHaveBall;
    internal bool ifJustInteract;
    internal int curRestTurn;
    internal ballScript ball;

    [Header("动物基础数值")]
    public int restTurn;
    public int selfIndex;
    public int baseIndexAdd;
    public float throwTime;
    public float throwHeight;
    public Transform AcceptPos;
    public Transform throwPos;
    public int interactionScore = 50;

    // Start is called before the first frame update
    void Start()
    {
        animalManager.Instance.registerAnimal(selfIndex, this);
    }

    // Update is called once per frame
    void Update()
    {
        
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
            DoDrop();
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
        curRestTurn = turn;
    }

    /// <summary>
    /// 0代表充能态，1代表动作态，2代表休息态
    /// </summary>
    /// <param name="toState"></param>
    public virtual void ChangeDisplay(int toState)
    {
        switch (toState)
        {
            case 0:
                renderer.color = Color.white;
                break;

            case 1:
                renderer.color = Color.green;
                break;

            case 2:
                renderer.color = Color.red;
                break;

            default:
                break;
        }
    }
}
