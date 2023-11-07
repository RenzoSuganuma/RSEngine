using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �X�e�[�g�}�V�� 
// �����o�^�����J�ڂ𖞂��������������ΑJ�ڂ���
// 
public class StateMachine
{
    List<StateTransition<IAIState, IAIState>> _transition = new();
    public delegate void OnStateExit(StateTransitionInfo info);
    public event OnStateExit onStateExit;
    int _currentTransitionIndex = 0; // ���݂̑J�ڂ̃��X�g����̃C���f�b�N�X
    public void Update()
    {
        // ���݃G���g���[���Ă���X�e�[�g�̎��s
        _transition[_currentTransitionIndex]._current.In();
        _transition[_currentTransitionIndex]._current.Tick();
        _transition[_currentTransitionIndex]._current.Out();
        //
        _transition[_currentTransitionIndex]._next.In();
        _transition[_currentTransitionIndex]._next.Tick();
        _transition[_currentTransitionIndex]._next.Out();
        // 
        var trans = _transition[_currentTransitionIndex];
        onStateExit(new StateTransitionInfo(trans._current, trans._next));
        // ���������𖞂������̂Ȃ玟�̑J�ڐ�̃X�e�[�g�ւƂ�
        var work = FindNextEntryPoint(_transition[_currentTransitionIndex]._next);
        _currentTransitionIndex = (work != -1) ? work : _currentTransitionIndex; // �����A�ǂ��ɂ������Ȃ��̂Ȃ炻�̃X�e�[�g�ɂƂǂ܂�
    }
    public void AddTransition(IAIState current, IAIState next)
    {
        _transition.Add(new StateTransition<IAIState, IAIState>(current, next));
    }
    public void GotoNextState()
    {

    }
    // �n���ꂽ StateTransition.next �̃X�e�[�g��StateTransition.current �Ƃ��ēo�^����Ă���J�ڃ��X�g�̃C���f�b�N�X��Ԃ�
    int FindNextEntryPoint(IAIState next)
    {
        int index = -1;
        for (int i = 0; i < _transition.Count; i++)
        {
            if (_transition[i]._current == next)
            {
                index = i;
                break;
            }
        }
        return index;
    }
}
// �J�ڌ��ƑJ�ڐ�
public class StateTransition<Tcurrent, Tnext>
{
    public StateTransition(Tcurrent current, Tnext next)
    {
        _current = current;
        _next = next;
    }
    public Tcurrent _current;
    public Tnext _next;
}
public struct StateTransitionInfo
{
    public IAIState _current;
    public IAIState _next;
    public StateTransitionInfo(IAIState current, IAIState next)
    {
        _current = current;
        _next = next;
    }
}
public interface IAIState
{
    public void In();
    public void Tick();
    public void Out();
}
public interface IStateMachineUser
{
    public void OnStateWasExitted(StateTransitionInfo info);
}