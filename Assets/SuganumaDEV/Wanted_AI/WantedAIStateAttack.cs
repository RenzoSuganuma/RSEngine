using RSEngine.StateMachine;
using System;
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
    Action onAttack;
    public event Action OnAttack { add { onAttack += value; }remove { onAttack -= value; } }

    public WantedAIStateAttack(float attackingRange, Transform selfTransform, LayerMask targetLayer, NavMeshAgent agent, Action onAttackAction)
    {
        _attackingRange = attackingRange;
        _selfTransform = selfTransform;
        _targetLayer = targetLayer;
        _agent = agent;
        onAttack += onAttackAction;
    }

    public void UpdateSelf(Transform selfTransform)
    {
        _selfTransform = selfTransform;
    }

    void Attack()
    {
        if (Physics.CheckSphere(_selfTransform.position, _attackingRange, _targetLayer))
        {
            _agent.SetDestination(_selfTransform.position);
            if(onAttack != null) { onAttack(); }
            Debug.Log("�U�����I");
        }
    }

    public void Entry()
    {
    }

    public void Update()
    {
        Attack();
    }

    public void Exit()
    {
    }
}
