using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle, Run, Jump
}

public class BaseFSM_Sub : BaseFSM<PlayerState>
{
    public BaseFSM_Sub() : base()
    {
        this.nowState = PlayerState.Idle;
        this.dictionaryTransitions = new Dictionary<StateTransitionInfo, PlayerState>
        {
            {new StateTransitionInfo(PlayerState.Idle, PlayerState.Run), PlayerState.Run },
            {new StateTransitionInfo(PlayerState.Run, PlayerState.Jump), PlayerState.Jump },
        };
    }
}
