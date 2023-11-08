using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace RSEngine
{
    namespace AI
    {
        // �X�e�[�g�}�V�� 
        // �����o�^�����J�ڂ𖞂��������������ΑJ�ڂ���
        /// <summary>  </summary>
        public class StateMachine
        {
            /// <summary> �X�e�[�g�y�A�̑J�ڃ��X�g�i�d��X�j </summary>
            /// �d�������Ȃ�
            HashSet<StatePairedTransition> _htransition = new();
            List<StatePairedTransition> _transition = new();

            /// <summary> �X�e�[�g�̃��X�g�i�d��X�j </summary>
            /// �d�������Ȃ�
            HashSet<IState> _hstates = new();
            List<IState> _states = new();

            /// <summary> ���ݎ��s���̃X�e�[�g </summary>
            int _currentTransitionIndex = -1;

            /// <summary> �X�e�[�g���甲�����Ƃ��ɃX�e�[�g�}�V�����ŌĂяo�����C�x���g </summary>
            /// <param name="info"></param>
            public delegate void OnStateExit(StateTransitionInfo info);

            /// <summary> �R�[���o�b�N���X�i�[�̓o�^��̃f���Q�[�g </summary>
            public event OnStateExit onStateExit;

            /// <summary> �X�e�[�g�}�V���N�����ɌĂяo�� </summary>
            public void Initialize()
            {
                _transition = _htransition.ToList();
                _states = _hstates.ToList();
                _currentTransitionIndex = 0;
            }

            /// <summary> ���t���[���Ăяo�����\�b�h </summary>
            public void Update()
            {
                // �X�e�[�g�̎��s
                var currentState = _transition[_currentTransitionIndex].Current;
                currentState.In();
                currentState.Tick();
                currentState.Out();
            }

            /// <summary> �J��ID���w�肵�Ă���ɑΉ��������������� </summary>
            /// <param name="transitionId"></param>
            /// <param name="condition"></param>
            public void UpdateCondition(int transitionId, bool condition)
            {
                _transition[transitionId].UpdateCondition(condition);
            }

            /// <summary> �X�e�[�g�̓o�^������ </summary>
            /// <param name="state"></param>
            public void AddState(IState state)
            {
                _hstates.Add(state);
            }

            /// <summary> �X�e�[�g�̓o�^���������� </summary>
            /// <param name="state"></param>
            public void RemoveState(IState state)
            {
                _hstates.Remove(state);
            }

            /// <summary> ���X�g�`���œn���ꂽ�X�e�[�g��o�^���� </summary>
            /// <param name="states"></param>
            public void AddStates(List<IState> states)
            {
                foreach (IState state in states)
                {
                    _hstates.Add(state);
                }
            }

            public void ClearAllStates()
            {
                _hstates.Clear();
            }

            /// <summary> �J�ڌ��ƑJ�ڐ�̏���ێ�����X�e�[�g�y�A��o�^����B </summary>
            /// <param name="from"></param>
            /// <param name="to"></param>
            public void AddTransition(IState from, IState to)
            {
                _htransition.Add(new StatePairedTransition(from, to, _htransition.Count /* id => 0 ~ */));
            }

            /// <summary> ���ׂẴX�e�[�g�y�A�̑J�ڂ��N���A���� </summary>
            public void ClearTransition()
            {
                _htransition.Clear();
            }
        }

        // �J�ڌ��ƑJ�ڐ�
        /// <summary> �J�ڌ��ƑJ�ڐ�̏���ێ�����N���X </summary>
        /// <typeparam name="Tcurrent"> �J�ڌ� </typeparam>
        /// <typeparam name="Tnext"> �J�ڐ� </typeparam>
        public class StatePairedTransition// �� �J�ڂ̖��ɓ����� �C������Ȃ�
        {
            public StatePairedTransition(IState from, IState to, int transitionID)
            {
                _from = from;
                _to = to;
                _current = _from;
                this.transitionID = transitionID;
            }
            IState _from; // id = 0
            IState _to; // id = 1
            IState _current; // id => current State
            public IState Current => _current;
            int transitionID; // transition id non duplication
            /// <summary> ���t���[���Ăяo���B�������X�V����B </summary>
            /// <param name="condition"></param>
            public void UpdateCondition(bool condition)
            {
                if (condition)
                {
                    _current = _to;
                }
                else
                {
                    _current = _from;
                }
            }
            /// <summary> ���݂���X�e�[�g��Ԃ� </summary>
            /// <returns></returns>
            public int GetCurrentState()
            {
                return (_current == _from) ? 0 : 1; // from => 0 : to => 1
            }
            /// <summary> �J�ڂh�c��Ԃ� </summary>
            /// <returns></returns>
            public int GetTransitionId()
            {
                return transitionID;
            }
        }

        /// <summary> ����G���g���[���Ă���X�e�[�g�y�A�̑J�ڂ̃X�e�[�g�̏���ێ�����\���� </summary>
        public struct StateTransitionInfo // �� �킩��Ȃ�
        {
            public IState _from;
            public IState _to;
            public int _id;
            public StateTransitionInfo(IState from, IState to, int id)
            {
                _from = from;
                _to = to;
                _id = id;
            }
        }

        /// <summary> �X�e�[�g�}�V���������J�ڃ��X�g�ɓo�^����N���X���p�����ׂ��C���^�[�t�F�C�X </summary>
        public interface IState // �� �C������Ȃ�
        {
            /// <summary> �X�e�[�g�˓����ɌĂяo����� </summary>
            public void In();
            /// <summary> �X�e�[�g�ʉߎ��ɌĂяo����� </summary>
            public void Tick();
            /// <summary> �X�e�[�g�E�o���ɌĂяo����� </summary>
            public void Out();
            /// <summary> ���̃X�e�[�g�֑J�ڂ��Ă��ǂ��̂��̔��� </summary>
            /// <returns></returns>
            public bool ReadyToGoNext();
            /// <summary> �����I�Ɏ��X�e�[�g�֍s�� </summary>
            public void SendMessageGotoNext();
        }

        /// <summary> �X�e�[�g�}�V�����p���N���X���p������ </summary>
        public interface IStateMachineUser // �� �C������Ȃ�
        {
            /// <summary> �X�e�[�g�� In(),Tick(),Out() �Ăяo������ɔ��΂���C�x���g�̃��X�i�[ </summary>
            /// <param name="info"></param>
            public void OnStateWasExitted(StateTransitionInfo info);
        }
    }
}