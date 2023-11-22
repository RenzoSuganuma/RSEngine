using RSEngine.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;
/// <summary> Wanted AI State : Default </summary>
/// デフォルトでは特定の経路をパトロールする。
public class WantedAIStateDefault : IState
{
    // 各必要パラメータ
    Transform _selfTransform;

    int _currentPathIndex;

    NavMeshAgent _agent;

    SplineContainer _splineContainer;

    List<Vector3> _patrolPath = new();

    SplineAnimate _splineAnimate;

    Transform _cashedTransform;

    public WantedAIStateDefault(NavMeshAgent agent, SplineContainer splineContainer, SplineAnimate splineAnimate)
    {
        _agent = agent;
        _currentPathIndex = 0;
        _splineContainer = splineContainer;
        var tmp = splineContainer.Spline.Knots.ToList();
        foreach (var item in tmp)
        {
            _patrolPath.Add(item.Position);
        }
        _splineAnimate = splineAnimate;
    }

    public void UpdateSelf(Transform selfTransform)
    {
        _selfTransform = selfTransform;
    }

    void Patroll()
    {
    }

    public void Entry()
    {
        Debug.Log("巡回を始める！");
        _splineAnimate.Play();
    }

    public void Update()
    {
        Debug.Log("巡回中");
        Patroll();
    }

    public void Exit()
    {
        Debug.Log("巡回を終わる！");
        _splineAnimate.Pause();
        _cashedTransform = _selfTransform;
    }
}
