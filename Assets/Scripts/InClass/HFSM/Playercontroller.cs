using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    void Start()
    {
        BaseFSM_Sub fsm = new BaseFSM_Sub();
        Debug.Log("Now State = " + fsm.nowState);
        Debug.Log("Command.Begin: Now State = " + fsm.NextStateMove(PlayerState.Run));
        Debug.Log("Invalid transition: " + fsm.searchNextState(PlayerState.Idle));
        Debug.Log("Previous State = " + fsm.previousState);
    }
}
