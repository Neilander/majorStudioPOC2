using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giraffeExtention : anActionExtention
{
    public int oneTimeScore = 100;
    public int banana;
    public int baseTurn;
    private int curTurn = -1;
    public override void interactWithBall(baseAnimalScript father)
    {
        //base.interactWithBall();
        father.ball.gameObject.SetActive(true);
        father.ball.MoveBall(father.selfIndex, father.selfIndex+1);
        father.ChangeDisplay(1, false);
        father.ifJustInteract = true;
        father.ifHaveBall = false;
        animalManager.Instance.changeScore(oneTimeScore, father.selfIndex);
        father.ifReady = false;
        animalManager.Instance.changeBanana(banana);
        //force += 1;
    }

    public override void changeToRest(baseAnimalScript father)
    {
        if (curTurn == -1)
        {
            curTurn = baseTurn;
        }
        else
        {
            curTurn += 1;
        }
        father.ChangeRestCount(curTurn);
    }

    public override void setUpWhenShow(baseAnimalScript father)
    {
        curTurn = -1;
    }
}
