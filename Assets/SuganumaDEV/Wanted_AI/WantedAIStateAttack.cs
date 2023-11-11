using RSEngine.AI.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary> Wanted AI State : Attack </summary>
public class WantedAIStateAttack : IState
{
    float _attackingRange;
    LayerMask _targetLayer;
    Transform _selfTransform;
    NavMeshAgent _agent;

    public WantedAIStateAttack(float attackingRange, Transform selfTransform, LayerMask targetLayer, NavMeshAgent agent)
    {
        _attackingRange = attackingRange;
        _selfTransform = selfTransform;
        _targetLayer = targetLayer;
        _agent = agent;
    }

    public void Update(Transform selfTransform)
    {
        _selfTransform = selfTransform;
    }

    void Attack()
    {
        if (Physics.CheckSphere(_selfTransform.position, _attackingRange, _targetLayer))
        {
            _agent.SetDestination(_selfTransform.position);
            Debug.Log("çUåÇÇ°ÅI");
        }
    }

    public void Do()
    {
        Attack();
    }

    public void In()
    {

    }

    public void Out()
    {
    }
}
