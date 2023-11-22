using RSEngine.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;
/// <summary> Wanted AI State : Default </summary>
/// �f�t�H���g�ł͓���̌o�H���p�g���[������B
public class WantedAIStateDefault : IState
{
    // �e�K�v�p�����[�^
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
        Debug.Log("������n�߂�I");
        _splineAnimate.Play();
    }

    public void Update()
    {
        Debug.Log("����");
        Patroll();
    }

    public void Exit()
    {
        Debug.Log("������I���I");
        _splineAnimate.Pause();
        _cashedTransform = _selfTransform;
    }
}
