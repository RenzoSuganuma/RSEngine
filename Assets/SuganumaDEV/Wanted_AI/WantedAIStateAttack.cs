using RSEngine.AI.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary> Wanted AI State : Attack </summary>
public class WantedAIStateAttack : IState
{
    public void Do()
    {
        Debug.Log("���炦�I");
    }

    public void In()
    {
        throw new System.NotImplementedException();
    }

    public void Out()
    {
        throw new System.NotImplementedException();
    }
}
