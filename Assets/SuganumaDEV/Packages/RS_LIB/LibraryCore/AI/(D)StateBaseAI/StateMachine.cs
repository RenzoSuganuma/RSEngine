using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RSEngine
{
    namespace AI
    {
        // �X�e�[�g�}�V�� 
        // �����o�^�����J�ڂ𖞂��������������ΑJ�ڂ���
        public class StateMachine
        {
            List<StateTransition<IAIState, IAIState>> _transition = new();
            public delegate void OnStateExit(StateTransitionInfo info);
            public event OnStateExit onStateExit;
            bool _isCurrentStateNow = true;
            bool _nextStateDone = false; // ���݂̑J�ڃy�A�̎��̃X�e�[�g���I������Ȃ�true
            int _currentTransitionIndex = 0; // ���݂̑J�ڂ̃��X�g����̃C���f�b�N�X
            public void Update()
            {
                // ���݃G���g���[���Ă���X�e�[�g�̎��s
                if (_isCurrentStateNow) // �X�e�[�g�y�A�J�ڌ��Ȃ�
                {
                    _transition[_currentTransitionIndex]._current.In();
                    _transition[_currentTransitionIndex]._current.Tick();
                    _transition[_currentTransitionIndex]._current.Out();
                    // current �̏������I����Ď��ɑJ�ڂł��邩����
                    _isCurrentStateNow = !_transition[_currentTransitionIndex]._current.ReadyToGoNext();
                }
                else
                {
                    _transition[_currentTransitionIndex]._next.In();
                    _transition[_currentTransitionIndex]._next.Tick();
                    _transition[_currentTransitionIndex]._next.Out();
                    // current �̏������I����Ď��ɑJ�ڂł��邩����
                    var done = _transition[_currentTransitionIndex]._current.ReadyToGoNext();
                    // �J�ڃy�A���X�e�[�g
                    if (!_nextStateDone && done) { _nextStateDone = true; }
                    // �X�e�[�g�I���������l�ɂ��ǂ�
                    _isCurrentStateNow = done; 
                }
                // �X�e�[�g�y�A�̂ǂ��炩���甲������C�x���g�̔���
                var trans = _transition[_currentTransitionIndex];
                onStateExit(new StateTransitionInfo(trans._current, trans._next));
                // ���̑J�ڃy�A�֑J��
                if (_nextStateDone)
                {
                    GotoNextStatePair();
                    _nextStateDone = false;
                }
            }
            public void AddTransition(IAIState current, IAIState next)
            {
                _transition.Add(new StateTransition<IAIState, IAIState>(current, next));
            }
            public void ClearTransition()
            {
                _transition.Clear();
            }
            public void GotoNextStatePair()
            {
                var work = FindNextFirstEntryPoint(_transition[_currentTransitionIndex]._next);
                if (work != -1)
                {
                    _currentTransitionIndex = work;// �����A�ǂ��ɂ������Ȃ��̂Ȃ炻�̃X�e�[�g�ɂƂǂ܂�
                }
            }
            // �n���ꂽ StateTransition.next �̃X�e�[�g��StateTransition.current �Ƃ��ēo�^����Ă���J�ڃ��X�g�̃C���f�b�N�X��Ԃ�
            // �����̑J�ڂɑΉ����Ă��Ȃ��̂ŒǁX�Ή�������悤��
            int FindNextFirstEntryPoint(IAIState next)
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
            public bool ReadyToGoNext();
            public void SendMessageGotoNext();
        }
        public interface IStateMachineUser
        {
            public void OnStateWasExitted(StateTransitionInfo info);
        }
    }
}