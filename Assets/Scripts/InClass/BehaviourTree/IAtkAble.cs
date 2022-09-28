public interface IAtkAble
{
    AtkBehaviour nowAtkBehaviour
    {
        get;
    }

    void OnExecuteAttack(int atkIdx); //어떤 공격을 할지 인덱스
}
