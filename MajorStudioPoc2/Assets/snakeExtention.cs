using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snakeExtention : anActionExtention
{
    public int oneTimeScore = 100;
    public override void interactWithBall(baseAnimalScript father)
    {
        //base.interactWithBall();
        father.ball.gameObject.SetActive(true);
        father.ball.MoveBall(father.selfIndex,father.selfIndex+1);
        father.ChangeDisplay(1, false);
        father.ifJustInteract = true;
        father.ifHaveBall = false;
        animalManager.Instance.changeScore(oneTimeScore, father.selfIndex);
        father.ifReady = false;
        animalManager.Instance.getADouble();
        //force += 1;
    }
}
