using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RSEngine.StateMachine;
using System;
public class WantedAIStateBackingToPatrol : IState
{
    Transform _targetTransform; // �p�g���[�����甲�������̍��W

    Transform _selfTransform;

    Action ActionWhenArrivedToPoint; // ���������W�܂ł����Ƃ��ɔ��΂��鏈��

    bool _bIsNearToPoint;

    public WantedAIStateBackingToPatrol(Transform beforeTransform, Action ActionWhenArrivedToPoint)
    {
        _targetTransform = beforeTransform;
        this.ActionWhenArrivedToPoint = ActionWhenArrivedToPoint;
    }

    public void UpdateSelf(Transform selfTransform)
    {
        _selfTransform = selfTransform;
    }

    public void Entry()
    {
        _bIsNearToPoint = (_targetTransform.position - _selfTransform.position).sqrMagnitude < 1;
    }

    public void Update()
    {
        if(_bIsNearToPoint) { ActionWhenArrivedToPoint(); }
    }

    public void Exit()
    {
        _bIsNearToPoint = false;
    }

}