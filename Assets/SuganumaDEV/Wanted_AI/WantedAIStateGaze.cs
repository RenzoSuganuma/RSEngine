using RSEngine.AI.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary> Wanted AI State : Gaze(íçéã) </summary>
public class WantedAIStateGaze : IState
{
    float _sigthRange;
    float _gazingLimitTime;
    Transform _selfTransform;
    LayerMask _targetLayer;
    NavMeshAgent _agent;
    Action<Transform> _onTargetFound;
    float _gazingTime = 0f;

    public WantedAIStateGaze(float sigthRange, float gazingTimeLimit, Transform selfTransform, LayerMask targetLayer, NavMeshAgent agent, Action<Transform> onTargetFound)
    {
        _sigthRange = sigthRange;
        _gazingLimitTime = gazingTimeLimit;
        _selfTransform = selfTransform;
        _targetLayer = targetLayer;
        _agent = agent;
        _onTargetFound = onTargetFound;
    }

    public void Update(Transform selfTransform)
    {
        _selfTransform = selfTransform;
    }

    void GazeOrNot()
    {
        if (Physics.CheckSphere(_selfTransform.position, _sigthRange, _targetLayer))
        {
            Debug.Log("ÇÒÅH");
            _agent.SetDestination(_selfTransform.position);
            _gazingTime += Time.deltaTime;
            if (_gazingTime > _gazingLimitTime)
            {
                Debug.Log("Ç›Ç¬ÇØÇΩÇºÅI");
                var cols = Physics.OverlapSphere(_selfTransform.position, _sigthRange, _targetLayer);
                _onTargetFound(cols[0].transform);
            }
        }
        else
        {
            _gazingTime = 0;
        }
    }

    public void Do()
    {
        GazeOrNot();
    }

    public void In()
    {
    }

    public void Out()
    {
    }
}
