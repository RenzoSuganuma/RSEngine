using UnityEngine;
using UnityEngine.Splines;
using RSEngine.StateMachine;
using UnityEngine.AI;
using System;
/// <summary> Wanted AI State : Default </summary>
/// デフォルトでは特定の経路をパトロールする。
public class WantedAIStateDefault : IState
{
    // 各必要パラメータ
    Transform _selfTransform;

    SplineContainer _splineContainer;

    NavMeshAgent _agent;

    Tuple<Vector3, Quaternion> _cashedTransform;

    float _length;
    float _current;

    bool _bNeedToGoNear;

    public WantedAIStateDefault(SplineContainer splineContainer, NavMeshAgent agent, ref Transform selfTransform)
    {
        _splineContainer = splineContainer;
        _agent = agent;
        _selfTransform = selfTransform;
    }

    public void UpdateSelf(Transform selfTransform)
    {
        _selfTransform = selfTransform;
    }

    public void Entry()
    {
        Debug.Log("巡回を始める！");
        _length = _splineContainer.CalculateLength();
        if (_cashedTransform == null)
        {
            var pos = _selfTransform.position;
            var rot = _selfTransform.rotation;
            _cashedTransform = Tuple.Create(pos, rot);
        }
    }

    public void Update()
    {
        Debug.Log("巡回中");
        _current += Time.deltaTime;
        var time = Mathf.Min(_current, _length) / _length;

        _splineContainer.Evaluate(time, out var position, out var tangent, out var upVector);
        var rotation = Quaternion.LookRotation(Vector3.Normalize(tangent), upVector);

        _selfTransform.SetPositionAndRotation(position, rotation);
    }

    public void Exit()
    {
        Debug.Log("巡回を終わる！");
        var pos = _selfTransform.position;
        var rot = _selfTransform.rotation;
        _cashedTransform = Tuple.Create(pos, rot);
    }
}