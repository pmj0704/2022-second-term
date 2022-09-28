using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateAtkController : MonoBehaviour
{
    public delegate void OnStartStateAtkController();
    public OnStartStateAtkController stateAtkControllerStartHandler;

    public delegate void OnEndStateAtkController();
    public OnEndStateAtkController stateAtkControllerEndHandler; //���� �̺�Ʈ ����� delegate ���� �Լ�
    //�ִϸ����� StateMachineBehaviour�� delegate ����
    
    //atkStateMachineBehaviour ���� ���°� �ִ��� Ȯ�� ����
    public bool getFlagStateAtkController
    {
        get;
        private set;
    }

    private void Start()
    {
        //delegate�� FSMctrl�� ����
        //delegate �ڵ鷯 �ʱ�ȭ stateAtkControllerStart�� �ʱ�ȭ ���� �Լ� ��
        stateAtkControllerStartHandler = new OnStartStateAtkController(stateAtkControllerStart);
        stateAtkControllerEndHandler = new OnEndStateAtkController(stateAtkControllerEnd);
    }

    private void stateAtkControllerStart() { }

    private void stateAtkControllerEnd() { }

    public void EventStateAtkStart() //stateAtkSubStateMachineBehaviour > OnStart()�� �� ȣ��
    {
        getFlagStateAtkController = true; //�ܺο��� ���� ����
        stateAtkControllerStartHandler(); //Delegate �ڵ鷯 ȣ��
    }

    public void EventStateAtkEnd()
    {
        getFlagStateAtkController = false; //�ܺο��� ���� ����
        stateAtkControllerEndHandler(); //Delegate �ڵ鷯 ȣ��
    }

    public void OnCheckAttackCollider(int attackIndex)
    {
        GetComponent<IAtkAble>()?.OnExecuteAttack(attackIndex); //������ ��ų ������ ��ų interface�� ����
    }
}