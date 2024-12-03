using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showStateMachine : MonoBehaviour
{
    private showState curState;

    public GameObject ballPrefab;
    public float endTurnWaitTime = 1f;

    private ballScript curBall;
    private bool ifBallMoveFinish = false;
    private bool ifendTurnAnimationFinish = false;
    private bool gameFail = false;
    // Start is called before the first frame update
    void Start()
    {
        animalManager.Instance.registerShowManager(this);
        curState = showState.empty;
    }

    // Update is called once per frame
    void Update()
    {
        
        UpdateState();
    }

    void startShow()
    {
        baseAnimalScript startAnimal = animalManager.Instance.returnFirstAnimal();
        if (startAnimal == null)
            Debug.LogError("没有起始动物");
        curBall = Instantiate(ballPrefab).GetComponent<ballScript>();
        curBall.doInitialDrop(startAnimal.AcceptPos.position, startAnimal,this);
    }

    public void StartState(showState newState)
    {
        EndState(curState);
        switch (newState)
        {
            case showState.showStart:
                startShow();
                ifBallMoveFinish = false;
                animalManager.Instance.ifShowEnd = false;
                break;

            case showState.turnStart:
                animalManager.Instance.turnStart();
                ifBallMoveFinish = false;
                break;


            case showState.turnEnd:
                animalManager.Instance.turnEnd();
                ifendTurnAnimationFinish = false;
                Invoke("changeAnimationFinishState", endTurnWaitTime);
                break;

            default:
                break;
        }
        curState = newState;
    }

    private void EndState(showState lastState)
    {
        switch (lastState)
        {
            default:
                break;
        }
    }

    private void UpdateState()
    {
        switch (curState)
        {
            case showState.showStart:
                if (ifBallMoveFinish)
                {
                    StartState(showState.turnStart);

                }
                break;

            case showState.turnStart:
                if (ifBallMoveFinish)
                {
                    
                    StartState(gameFail?showState.gameEnd: showState.turnEnd);

                }
                break;

            case showState.turnEnd:
                if (ifendTurnAnimationFinish)
                {
                    StartState(showState.turnStart);
                    
                }
                break;

            default:
                break;
        }
    }

    public void reportMoveFinish(ballScript ball)
    {
        if (ball == curBall)
            ifBallMoveFinish = true;
    }

    void changeAnimationFinishState()
    {
        ifendTurnAnimationFinish = true;
    }

    public void reportDrop(ballScript ball)
    {
        gameFail = true;
        animalManager.Instance.ifShowEnd = true;
    }
}

public enum showState
{
    empty,
    showStart,
    turnStart,
    turnEnd,
    gameEnd
}
