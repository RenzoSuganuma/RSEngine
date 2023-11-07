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
    int _current = 0;
    public void Update()
    {
        _transition[_current]._current.Entry();
        _transition[_current]._current.Tick();
        _transition[_current]._current.Exit();
        _current = FindNextEntryPoint(_transition[_current]._next);
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
public interface IAIState
{
    public void Entry();
    public void Tick();
    public void Exit();
}