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
    delegate void OnStateExit(StateTransitionInfo info);
    OnStateExit onStateExit;
    int _currentIndex = 0;
    public void Update()
    {
        // ���݃G���g���[���Ă���X�e�[�g�̎��s
        _transition[_currentIndex]._current.Entry();
        _transition[_currentIndex]._current.Tick();
        _transition[_currentIndex]._current.Exit();
        var trans = _transition[_currentIndex];
        onStateExit(new StateTransitionInfo(trans._current, trans._next));
        // ���������𖞂������̂Ȃ玟�̑J�ڐ�̃X�e�[�g�ւƂ�
        var work = FindNextEntryPoint(_transition[_currentIndex]._next);
        _currentIndex = (work != -1) ? work : 0; // �����A�ǂ��ɂ������Ȃ��̂Ȃ�f�t�H���g�̃X�e�[�g�ւ���
    }
    public void AddTransition(IAIState current, IAIState next)
    {
        _transition.Add(new StateTransition<IAIState, IAIState>(current, next));
    }
    // �n���ꂽ StateTransition.next �̃X�e�[�g��StateTransition.current �Ƃ��ēo�^����Ă���J�ڃ��X�g�̃C���f�b�N�X��Ԃ�
    int FindNextEntryPoint(IAIState next)
    {
        int index = -1;
        for (int i = 0; i < _transition.Count; i++)
        {
            if(_transition[i]._current == next) 
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
    IAIState _current;
    IAIState _next;
    public StateTransitionInfo(IAIState current, IAIState next)
    {
        _current = current;
        _next = next;
    }
}
public interface IAIState
{
    public void Entry();
    public void Tick();
    public void Exit();
}