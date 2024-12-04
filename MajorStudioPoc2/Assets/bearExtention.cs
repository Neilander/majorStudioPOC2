using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bearExtention : anActionExtention
{
    private int force = 1;
    public int oneTimeScore = 100;
    public override void interactWithBall(baseAnimalScript father)
    {
        //base.interactWithBall();
        father.ball.gameObject.SetActive(true);
        father.ball.MoveBall(father.selfIndex, father.selfIndex + force);
        father.ChangeDisplay(1, false);
        father.ifJustInteract = true;
        father.ifHaveBall = false;
        animalManager.Instance.changeScore(force*oneTimeScore);
        father.ifReady = false;
        force += 1;
    }

    public override void setUpWhenShow(baseAnimalScript father)
    {
        //base.setUpWhenShow(father);
        force = 1;
    }

}
