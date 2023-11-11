using RSEngine.AI.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary> Wanted AI State : Death </summary>
public class WantedAIStateDeath : IState
{
    public void Do()
    {
        Debug.Log("Ç»ÇÒÅcÇæÇ∆Åc");
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
