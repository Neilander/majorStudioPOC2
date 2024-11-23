using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showStateMachine : MonoBehaviour
{
    private showState curState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    public void StartState(showState newState)
    {
        EndState(curState);
        switch (newState)
        {
            case showState.turnStart:
                animalManager.Instance.turnStart();
                break;


            case showState.turnEnd:
                animalManager.Instance.turnEnd();
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
            case showState.turnStart:
                if (Input.GetKeyDown(KeyCode.T))
                {
                    StartState(showState.turnEnd);

                }
                break;

            case showState.turnEnd:
                if (Input.GetKeyDown(KeyCode.T))
                {
                    StartState(showState.turnStart);

                }
                break;

            default:
                break;
        }
    }
}

public enum showState
{
    turnStart,
    turnEnd
}
