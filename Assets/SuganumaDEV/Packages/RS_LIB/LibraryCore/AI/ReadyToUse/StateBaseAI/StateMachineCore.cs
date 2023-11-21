using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RSEngine
{
    namespace StateMachine
    {
        public partial class StateMachineCore
        {
            // �X�e�[�g
            HashSet<IState> _states = new HashSet<IState>();
            // �g�����W�V����
            HashSet<StateMachineTransition> _transitions = new HashSet<StateMachineTransition>();
            // ���ݓ˓����Ă���X�e�[�g
            IState _currentPlayingState;
            // ���ݓ˓����Ă���g�����W�V������
            string _currentTransitionName;
            // �X�e�[�g�}�V�����ꎞ��~�����̃t���O
            bool _bIsPausing;

            #region �o�^����
            public void ResistState(IState state)
            {
                _states.Add(state);
                if (_currentPlayingState == null) { _currentPlayingState = state; }
            }

            // ���W�X�g���邽�тɏI�_�X�e�[�g�����_�X�e�[�g�Ƃ��Ċ��蓖�Ă��Ă���X�e�[�g������������̃f�[�^�\���̃f�[�^��ێ�����
            public void ResistTransition(IState from, IState to, string name)
            {
                var tmp = new StateMachineTransition(from, to, name);
                _transitions.Add(tmp);
            }
            #endregion

            #region �X�V����
            public void UpdateConditionOfTransition(string name, ref bool condition2transist)
            {
                if (_bIsPausing) return; // �����ꎞ��~���Ȃ�X�V�����͂��Ȃ��B
                foreach (var t in _transitions)
                {
                    // �J�ڂ���ꍇ // * �����𖞂����Ă���Ȃ�O�g�����W�V�����𖳎����Ă��܂��̂ł��̔��菈�����͂��ނ��� *
                    // �����J�ڏ����𖞂����Ă��đJ�ږ�����v����Ȃ�
                    if (condition2transist && t.Name == name) 
                    {
                        if (t.SFrom == _currentPlayingState) // ���ݍ��X�e�[�g�Ȃ�
                        {
                            _currentPlayingState.Exit(); // �E�X�e�[�g�ւ̑J�ڏ����𖞂������̂Ŕ�����
                            condition2transist = false; // �J�ڏ�����������(false��)
                            _currentPlayingState = t.STo; // ���݂̃X�e�[�g���E�X�e�[�g�ɍX�V�A�J�ڂ͂��̂܂�
                            _currentPlayingState.Entry(); // ���݂̃X�e�[�g�̏���N���������Ă�
                            _currentTransitionName = name; // ���݂̑J�ڃl�[�����X�V
                        }
                    }
                    // �J�ڂ̏����𖞂����Ă͂��Ȃ����A�J�ڃl�[������v�i�X�V����Ă��Ȃ��Ȃ�j���݂̃X�e�[�g�̍X�V�������Ă�
                    else if (t.Name == name)
                    {
                        _currentPlayingState.Update();
                    }
                } // �S�J�ڂ������B
            }
            #endregion

            #region �N������
            public void PopStateMachine()
            {
                _bIsPausing = false;
                _currentPlayingState.Entry();
            }
            #endregion

            #region �ꎞ��~����
            public void PushStateMachine()
            {
                _bIsPausing = true;
            }
            #endregion
        }
        // �e�g�����W�V�����͖��O�����蓖�ĂĂ���
        public partial class StateMachineTransition
        {
            IState _from;
            public IState SFrom => _from;
            IState _to;
            public IState STo => _to;
            string _name;
            public string Name => _name;
            public StateMachineTransition(IState from, IState to, string name)
            {
                _from = from;
                _to = to;
                _name = name;
            }
        }

        public interface IState
        {
            public void Entry();
            public void Update();
            public void Exit();
        }

        #region �X�e�[�g�}�V���A���p���\�z
        // �C�j�V�����C�Y����
        // �X�e�[�g�}�V���C���X�^���X���X�e�[�g�̓o�^
        // �g�����W�V�����̓o�^
        // �X�e�[�g�}�V���̍X�V

        // ���t���[������
        // �g�����W�V�����̏�Ԃ̍X�V

        //�J����
        // partial class�Ƃ��Đ錾���ĕ��Ƃ̓�Փx��������
        #endregion
    }
}