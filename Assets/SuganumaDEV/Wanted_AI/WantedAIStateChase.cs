using RSEngine.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static RSEngine.OriginalMethods;
/// <summary> Wanted AI State : Chase </summary>
public class WantedAIStateChase : IState
{
    #region __DEBUG__
    bool __DEBUG__ = false;
    #endregion

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

    public void UpdateSelf(Transform selfTransform)
    {
        _selfTransform = selfTransform;
    }

    public void Entry()
    {
        Tap(__DEBUG__,
        () => Debug.Log("’Ç‚¤‚¼I"));
    }

    public void Update()
    {
        Tap(__DEBUG__, 
        () => Debug.Log("‚Ü‚ÄI"));
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

    public void Exit()
    {
        Tap(__DEBUG__,
        ()=> Debug.Log("‚à‚¤’Ç‚í‚È‚¢"));
    }
}
