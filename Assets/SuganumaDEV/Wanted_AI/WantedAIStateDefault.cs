using UnityEngine;
using UnityEngine.Splines;
using RSEngine.StateMachine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;
using System.Linq;
/// <summary> Wanted AI State : Default </summary>
/// デフォルトでは特定の経路をパトロールする。
public class WantedAIStateDefault : IState
{
    // 各必要パラメータ
    Transform _selfTransform;

    NavMeshAgent _agent;

    List<Vector3> _patrolPath = new();

    int _currentPathIndex;

    public WantedAIStateDefault(NavMeshAgent agent, Transform selfTransform, SplineContainer spline)
    {
        _agent = agent;
        _selfTransform = selfTransform;

        foreach (var path in spline.Spline.Knots.ToList())
        {
            _patrolPath.Add(path.Position);
        }
    }

    public void UpdateSelf(Transform selfTransform)
    {
        _selfTransform = selfTransform;
    }

    public void Entry()
    {
        Debug.Log("巡回を始める！");
    }

    public void Update()
    {
        Debug.Log("巡回中");
        if ((_selfTransform.position - _patrolPath[_currentPathIndex]).sqrMagnitude > 1)
        {
            _agent.SetDestination(_patrolPath[_currentPathIndex]);
        }
        else
        {
            _currentPathIndex = (_currentPathIndex < _patrolPath.Count) ? _currentPathIndex + 1 : 0;
        }
    }

    public void Exit()
    {
        Debug.Log("巡回を終わる！");
    }
}