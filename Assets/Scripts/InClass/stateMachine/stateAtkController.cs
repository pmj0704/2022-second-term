using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateAtkController : MonoBehaviour
{
    public delegate void OnStartStateAtkController();
    public OnStartStateAtkController stateAtkControllerStartHandler;

    public delegate void OnEndStateAtkController();
    public OnEndStateAtkController stateAtkControllerEndHandler; //공격 이벤트 종료시 delegate 종료 함수
    //애니메이터 StateMachineBehaviour와 delegate 연동
    
    //atkStateMachineBehaviour 내에 상태가 있는지 확인 해줌
    public bool getFlagStateAtkController
    {
        get;
        private set;
    }

    private void Start()
    {
        //delegate는 FSMctrl에 있음
        //delegate 핸들러 초기화 stateAtkControllerStart는 초기화 해줄 함수 명
        stateAtkControllerStartHandler = new OnStartStateAtkController(stateAtkControllerStart);
        stateAtkControllerEndHandler = new OnEndStateAtkController(stateAtkControllerEnd);
    }

    private void stateAtkControllerStart() { }

    private void stateAtkControllerEnd() { }

    public void EventStateAtkStart() //stateAtkSubStateMachineBehaviour > OnStart()일 때 호출
    {
        getFlagStateAtkController = true; //외부에서 공격 상태
        stateAtkControllerStartHandler(); //Delegate 핸들러 호출
    }

    public void EventStateAtkEnd()
    {
        getFlagStateAtkController = false; //외부에서 공격 상태
        stateAtkControllerEndHandler(); //Delegate 핸들러 호출
    }

    public void OnCheckAttackCollider(int attackIndex)
    {
        GetComponent<IAtkAble>()?.OnExecuteAttack(attackIndex); //공격의 스킬 종류를 스킬 interface로 구현
    }
}