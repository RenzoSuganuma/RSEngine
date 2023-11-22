using RSEngine.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary> Wanted AI State : Death </summary>
public class WantedAIStateDeath : IState
{
    Transform _selfTransform;
    NavMeshAgent _agent;

    public WantedAIStateDeath(Transform selfTransform, NavMeshAgent agent)
    {
        _selfTransform = selfTransform;
        _agent = agent;
    }

    public void UpdateSelf(Transform selfTransform)
    {
        _selfTransform = selfTransform;
    }

    void Death()
    {
        _agent.SetDestination(_selfTransform.position);
        Debug.Log("Ç»ÇÒÅcÇæÇ∆Åc");
    }

    public void Entry()
    {
    }

    public void Update()
    {
        Death();
    }

    public void Exit()
    {
    }
}
