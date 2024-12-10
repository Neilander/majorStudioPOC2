using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tigerExtention : anActionExtention
{
    public int oneTimeScore = 100;
    public override void interactWithBall(baseAnimalScript father)
    {
        //base.interactWithBall();
        father.ball.gameObject.SetActive(true);
        father.ball.MoveBall(father.selfIndex, animalManager.Instance.returnTheIndexWithTheMostRestTurn(father.selfIndex));
        father.ChangeDisplay(1, false);
        father.ifJustInteract = true;
        father.ifHaveBall = false;
        animalManager.Instance.changeScore(oneTimeScore, father.selfIndex);
        father.ifReady = false;
        //force += 1;
    }

    
}
