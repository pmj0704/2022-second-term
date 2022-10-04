using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFSM<T> : MonoBehaviour
{
    #region State Transition Info

    /// <summary>
    /// ���� ���̿� ���� �Ӽ�
    /// </summary>
    public class StateTransitionInfo
    {
        public T nowState { get; set; }
        public T nextState { get; set; }

        public StateTransitionInfo(T nowState, T nextState)
        {
            this.nowState = nowState;
            this.nextState = nextState;
        }

        /// <summary>
        /// ��ü�� ������ �� ������ Ű�� ��ȯ�ϴ� �Լ�
        /// </summary>
        public override int GetHashCode()
        {
            return 16 + 30 * this.nowState.GetHashCode() + 30 * this.nextState.GetHashCode();
        }

        /// <summary>
        /// Object�� �ڱ� �ڽ��� ��ü�� ������ Ȯ��
        /// </summary>
        public override bool Equals(object obj)
        {
            StateTransitionInfo other = obj as StateTransitionInfo;
            return other != null && this.nowState.Equals(other.nowState) && this.nextState.Equals(other.nextState);
        }
    }

    #endregion

    #region BaseFSM
    protected Dictionary<StateTransitionInfo, T> dictionaryTransitions;
    public T nowState;
    public T previousState;

    protected BaseFSM()
    {
        if (!typeof(T).IsEnum)
        {
            Debug.Log(typeof(T).FullName + "is not state.");
        }
    }

    private T getNextState(T nextState)
    {
        StateTransitionInfo transition = new StateTransitionInfo(nowState, nextState);
        T refNextState;

        //(Key,Value) Key�� Dictionary�� ���� �ϸ� True�� Value�� ��ȯ 
        if (!dictionaryTransitions.TryGetValue(transition, out refNextState))
        {
            Debug.Log("Invalid State transition from " + nowState + "to " + nextState);
        }
        else
        {
            Debug.Log("Next state " + refNextState);
        }

        return refNextState;
    }

    public bool searchNextState(T nextState)
    {
        StateTransitionInfo stateTransition = new StateTransitionInfo(nowState, nextState);
        T refNextState;

        if (!dictionaryTransitions.TryGetValue(stateTransition, out refNextState))
        {
            Debug.Log("Invalid State transition from " + nowState + "to " + refNextState);
            return false;
        }
        else
        {
            return true;
        }
    }

    public T NextStateMove(T nextState)
    {
        previousState = nowState;
        nowState = getNextState(nextState);
        Debug.Log("Change state from " + previousState + "to " + nowState);
        return nowState;
    }

    #endregion
}
