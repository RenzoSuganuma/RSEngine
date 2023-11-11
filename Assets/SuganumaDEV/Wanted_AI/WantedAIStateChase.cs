using RSEngine.AI.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary> Wanted AI State : Chase </summary>
public class WantedAIStateChase : IState
{
    float _sightRange;
    LayerMask _targetLayer;
    Transform _selfTransform;
    NavMeshAgent _agent;
    public WantedAIStateChase(float sightRange, LayerMask targetLayer, Transform selfTransform, NavMeshAgent agent)
    {
        _sightRange = sightRange;
        _targetLayer = targetLayer;
        _selfTransform = selfTransform;
        _agent = agent;
    }

    public void Update(Transform selfTransform)
    {
        _selfTransform = selfTransform;
    }

    public void Do()
    {
        Debug.Log("Ç‹ÇƒÅI");
        if (Physics.CheckSphere(_selfTransform.position, _sightRange, _targetLayer))
        {
            var cols = Physics.OverlapSphere(_selfTransform.position, _sightRange, _targetLayer);
            _agent.SetDestination(cols[0].transform.position);
        }
        else
        {
            _agent.SetDestination(_selfTransform.position);
        }
    }

    public void In()
    {
        Debug.Log("í«Ç§ÇºÅI");
    }

    public void Out()
    {
        Debug.Log("Ç‡Ç§í«ÇÌÇ»Ç¢");
    }
}
