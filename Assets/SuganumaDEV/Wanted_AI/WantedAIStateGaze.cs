// 管理者 菅沼
using SLib.StateSequencer;
using System;
using UnityEngine;
using UnityEngine.AI;
using static SLib.OriginalMethods;
/// <summary> Wanted AI State : Gaze(注視) </summary>
public class WantedAIStateGaze : IState
{
    #region __DEBUG__
    bool __DEBUG__ = false;
    #endregion

    float _sigthRange;
    float _gazingLimitTime;
    Transform _selfTransform;
    LayerMask _targetLayer;
    NavMeshAgent _agent;
    Action<Transform> _onTargetGazing;
    Action<Transform> _onTargetFound;

    float _gazingTime = 0f;

    public WantedAIStateGaze(float sigthRange, float gazingTimeLimit, Transform selfTransform, LayerMask targetLayer, NavMeshAgent agent, Action<Transform> onTargetGazing, Action<Transform> onTargetFound)
    {
        _sigthRange = sigthRange;
        _gazingLimitTime = gazingTimeLimit;
        _selfTransform = selfTransform;
        _targetLayer = targetLayer;
        _agent = agent;
        _onTargetGazing = onTargetGazing;
        _onTargetFound = onTargetFound;
    }

    public void UpdateSelf(Transform selfTransform)
    {
        _selfTransform = selfTransform;
    }

    void GazeOrNot()
    {
        if (Physics.CheckSphere(_selfTransform.position, _sigthRange, _targetLayer))
        {
            Knock(__DEBUG__,
            () => Debug.Log("ん？"));
            _agent.SetDestination(_selfTransform.position);
            _gazingTime += Time.deltaTime;
            var cols = Physics.OverlapSphere(_selfTransform.position, _sigthRange, _targetLayer);
            _onTargetGazing(cols[0].transform);

            if (_gazingTime > _gazingLimitTime)
            {
                Knock(__DEBUG__,
                () => Debug.Log("みつけたぞ！"));
                _onTargetFound(cols[0].transform);
            }
        }
    }

    public void Entry()
    {
        Knock(__DEBUG__,
        () => Debug.Log("睨むぞ！"));
    }

    public void Update()
    {
        GazeOrNot();
    }

    public void Exit()
    {
        Knock(__DEBUG__,
        () => Debug.Log("睨んだ！"));
        _gazingTime = 0;
    }
}
