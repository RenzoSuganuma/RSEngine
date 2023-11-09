using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace RSEngine
{
    namespace AI
    {
        // a => b, b => c : �̕����ᖡ���K�v
        // �X�e�[�g�}�V�� 
        // �����o�^�����J�ڂ𖞂��������������ΑJ�ڂ���
        /// <summary>  </summary>
        public class StateMachine
        {
            /// <summary> �X�e�[�g�y�A�̑J�ڃ��X�g�i�d��X�j </summary>
            /// �d�������Ȃ�
            HashSet<StatePairedTransition> _htransition = new();

            /// <summary> �X�e�[�g�y�A�̑J�ڃ��X�g </summary>
            List<StatePairedTransition> _transition = new();

            /// <summary> �O�t���[���Ŏ��s�����J�ڂ̏�Ԃ�ێ����Ă���ϐ� </summary>
            StatePairedTransition _ptransition;

            /// <summary> �X�e�[�g�̃��X�g�i�d��X�j </summary>
            /// �d�������Ȃ�
            HashSet<IState> _hstates = new();
            /// <summary> �X�e�[�g�̃��X�g </summary>

            List<IState> _states = new();

            /// <summary> ���ݎ��s���̃X�e�[�g </summary>
            int _currentTransitionIndex = -1;

            /// <summary> �X�e�[�g���甲�����Ƃ��ɃX�e�[�g�}�V�����ŌĂяo�����C�x���g </summary>
            /// <param name="info"></param>
            public delegate void OnStateExit(StateTransitionInfo info);

            /// <summary> �R�[���o�b�N���X�i�[�̓o�^��̃f���Q�[�g </summary>
            public event OnStateExit onStateExit;

            #region Machine
            /// <summary> �X�e�[�g�}�V���N�����ɌĂяo���B�X�e�[�g�}�V�����N������ </summary>
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
                currentState.Do();
                currentState.Out();
            }
            #endregion

            #region Transition
            /// <summary> �J��id���w�肵�Ă���ɑΉ��������������� </summary>
            /// <param name="transitionID"></param>
            /// <param name="condition"></param>
            public void UpdateTransitionCondition(int transitionID, bool condition)
            {
                // ���O�ɍs�����J�ڂŐ���������ꂽ��@�ۂ�����
                if (_ptransition == null) // �������Ȃ��Ȃ�o�^
                {
                    var ptransition = _transition[transitionID];
                    _ptransition = new(ptransition.GetState(0), ptransition.GetState(1), transitionID);
                }
                else
                {
                    var tID = _ptransition.GetTransitionId();
                    //if()
                }
                var transition = _transition[transitionID];
                // �����𖞂�������X�e�[�g�ɓ�������Ԃ��ێ�����
                if (transition.GetCurrentState() == 0) // �܂��J�ڂ��Ă��Ȃ��Ȃ�
                {
                    transition.GotoNextCondition(condition);
                }
                else // �J�ڂ��I���A�X�e�[�g�𔲂����Ȃ�
                {
                    List<int> work = new(); // �i�߂�J��id���i�[
                    for (int i = 0; i < _transition.Count; i++)
                    {
                        if (transition.Current == _transition[i].GetState(0))
                        {
                            work.Add(i);
                        }
                    } // ���ɂƂ� �J�ڐ悪�J�ڌ��Ƃ��Ă��Ă�����Ă���J�ڂ�T��

                }
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
            #endregion

            #region State
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
            #endregion

            #region States
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
            #endregion
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
            /// <summary> �����������������ꂽ��J�ڐ�X�e�[�g�ֈڂ��ĂƂǂ܂�B </summary>
            /// <param name="condition"></param>
            public void GotoNextCondition(bool condition)
            {
                /*|| �������𖞂����Ă���Ƃ��ɂ̂ݑJ�ڐ�X�e�[�g�����s���邽�߈ꎞ������ 11 / 09 ||*/
                #region 11/09 | �ꎞ����������
                /*
                if (condition)
                {
                    _current = _to;
                }
                else
                {
                    _current = _from;
                }
                */
                #endregion
                /*|| --- ||*/
                // ���������^�ł��܂�����̃X�e�[�g���J�ڌ��̎��ɂ̂ݎ��s�B
                // ��x�����J�ڐ�Ɉڂ�B
                _current = (condition && _current == _from) ? _to : _current;
            }
            /// <summary> ���݂���X�e�[�g��Ԃ� </summary>
            /// <returns>0 : (�X�e�[�g�y�A�̑J�ڌ�) 1 : (�X�e�[�g�y�A�J�ڐ�)</returns>
            public int GetCurrentState()
            {
                return (_current == _from) ? 0 : 1; // from => 0 : to => 1
            }
            /// <summary> �J��id��Ԃ� </summary>
            /// <returns></returns>
            public int GetTransitionId()
            {
                return transitionID;
            }
            /// <summary> �w�肳�ꂽ�X�e�[�gid�̃X�e�[�g��Ԃ� </summary>
            /// <param name="stateId"></param>
            /// <returns></returns>
            public IState GetState(int stateId)
            {
                return (stateId == 0) ? _from : _to;
            }
        }

        /// <summary> ����G���g���[���Ă���X�e�[�g�y�A�̑J�ڂ̃X�e�[�g�̏���ێ�����\���� </summary>
        public struct StateTransitionInfo // �� �C�����K�v���v�ᖡ
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
            public void Do();
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