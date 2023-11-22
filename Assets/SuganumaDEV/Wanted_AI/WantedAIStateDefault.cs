using RSEngine.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary> Wanted AI State : Default </summary>
/// デフォルトでは特定の経路をパトロールする。
public class WantedAIStateDefault : IState
{
    // 各必要パラメータ
    int _currentPathIndex;
    Transform[] _patrollingPath;
    Transform _selfTransform;

    NavMeshAgent _agent;

    public WantedAIStateDefault(Transform[] patrollingPath, NavMeshAgent agent)
    {
        _agent = agent;
        _patrollingPath = patrollingPath;
        _currentPathIndex = 0;
    }

    public void UpdateSelf(Transform selfTransform)
    {
        _selfTransform = selfTransform;
    }

    void Patroll()
    {
        if (_patrollingPath != null && _patrollingPath.Length > 0)
        {
            var isNearToPoint = (_patrollingPath[_currentPathIndex].position - _selfTransform.position).sqrMagnitude < 2;
            if (isNearToPoint)
            {
                _currentPathIndex = (_currentPathIndex + 1 < _patrollingPath.Length) ? _currentPathIndex + 1 : 0;
            }
            _agent.SetDestination(_patrollingPath[_currentPathIndex].position);
        }
    }

    public void Entry()
    {
        Debug.Log("巡回を始める！");
    }

    public void Update()
    {
        Debug.Log("巡回中");
        Patroll();
    }

    public void Exit()
    {
        Debug.Log("巡回を終わる！");
    }
}
