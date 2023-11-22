using RSEngine.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class WantedCPU : MonoBehaviour
{
    // �X�e�[�g�}�V��
    /// <summary> AI�̃X�e�[�g�x�[�X�ȏ������T�|�[�g���邽�߂̃X�e�[�g�}�V�� </summary>
    StateMachineCore _stateMachine;

    // �X�e�[�g
    /// <summary> �f�t�H���g�X�e�[�g�F�A�C�h�� </summary>
    WantedAIStateDefault _sDef;
    /// <summary> �X�e�[�g�F���� </summary>
    WantedAIStateGaze _sGaze;
    /// <summary> �X�e�[�g�F�ǐ� </summary>
    WantedAIStateChase _sChase;
    /// <summary> �X�e�[�g�F�U�� </summary>
    WantedAIStateAttack _sAttack;
    /// <summary> �X�e�[�g�F���S </summary>
    WantedAIStateDeath _sDeath;

    // �������̂ɕK�v
    NavMeshAgent _agent;

    // �e�����W
    [SerializeField, Range(0f, 50f)] float _sightRange;
    [SerializeField, Range(0f, 50f)] float _attackRange;
    // �e���C���}�X�N
    [SerializeField] LayerMask _targetLayer;
    [SerializeField] LayerMask _groundLayer;
    // �^�[�Q�b�g
    [SerializeField] Transform _target;
    [SerializeField] int _targetLayerNum;
    // �ړ����x
    [SerializeField] float _movespeed;
    // �p�j�o�H
    [SerializeField] List<Transform> _patrollingPath;

    [SerializeField] float _health;

    // AI�g�����W�V�����t���O
    [SerializeField]
    bool _isInsideSightRange = false; // �f�t�H���g���璍������܂ł̏���
    [SerializeField]
    bool _isFoundTargetNow = false; // �������I���A�v���C���[�Ƃ��Ĕ��肵���ꍇ�@�ǐՂ��邩�̏���
    [SerializeField]
    bool _isInsideAttackingRange = false; // �ǐՂ����Ă��čU���\�����Ƀv���C���[���������ꍇ�@�U�����邩�̏���
    [SerializeField]
    bool _isNoHealthNow = false;�@// ���S�������ꍇ

    // �ʏ�J�ڃ^�C�v
    StateMachineTransitionType _tTStd = StateMachineTransitionType.StandardState;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        // �X�e�[�g�}�V��������
        _stateMachine = new();

        // �e�X�e�[�g������
        _sDef = new(_patrollingPath.ToArray(), _agent);
        _sGaze = new(_sightRange, 2, transform, _targetLayer, _agent
            , (tTransform) =>
            {
                var dir = (tTransform.position - transform.position).normalized;
                dir.y = 0;
                transform.forward = dir;
            } // On Gazing
            , (tTransform) =>
            {
                var dir = (tTransform.position - transform.position).normalized;
                dir.y = 0;
                transform.forward = dir;
                if (!_isFoundTargetNow) _isFoundTargetNow = true;
            } // On Target Found
            );
        _sChase = new(_sightRange, _targetLayer, transform, _agent);
        _sAttack = new(_attackRange, transform, _targetLayer, _agent, () => { Debug.Log("�U����...."); });
        _sDeath = new(transform, _agent);

        // �e�X�e�[�g�̓o�^
        _stateMachine.ResistStates(new List<IState> {
        _sDef,
        _sGaze,
        _sChase,
        _sAttack,
        _sDeath});

        _stateMachine.ResistTransition(_sDef, _sGaze, "D2G"); // default to gaze id{0}
        _stateMachine.ResistTransition(_sGaze, _sDef, "G2D"); // gaze to default id{1}

        _stateMachine.ResistTransition(_sGaze, _sChase, "G2C"); // gaze to chase id{2}
        _stateMachine.ResistTransition(_sChase, _sGaze, "C2G"); // chase to default id{3}

        _stateMachine.ResistTransition(_sChase, _sAttack, "C2A"); // chase to attack id{4}
        _stateMachine.ResistTransition(_sAttack, _sChase, "A2C"); // attack to chase id{5}

        _stateMachine.ResistTransition(_sDef, _sDeath, "D2d"); // id{6}
        _stateMachine.ResistTransition(_sGaze, _sDeath, "G2d"); // id{7}
        _stateMachine.ResistTransition(_sChase, _sDeath, "C2d"); // id{8}
        _stateMachine.ResistTransition(_sAttack, _sDeath, "A2d"); // id{9}

    }

    private void FixedUpdate()
    {
        // ��������̔���
        _isInsideSightRange = Physics.CheckSphere(transform.position, _sightRange, _targetLayer);
        _isInsideAttackingRange = Physics.CheckSphere(transform.position, _attackRange, _targetLayer);
        if (!_isInsideSightRange && _isFoundTargetNow) _isFoundTargetNow = false;
        _isNoHealthNow = _health <= 0;

        // �e�X�e�[�g�X�V
        _sDef.UpdateSelf(transform);
        _sGaze.UpdateSelf(transform);
        _sChase.UpdateSelf(transform);
        _sAttack.UpdateSelf(transform);
        _sDeath.UpdateSelf(transform);

        // defalut to gaze
        _stateMachine.UpdateConditionOfTransition("D2G", ref _isInsideSightRange);

        // gaze to deafult
        _stateMachine.UpdateConditionOfTransition("G2D", ref _isInsideSightRange, _tTStd, !true);

        // gaze to chase
        _stateMachine.UpdateConditionOfTransition("G2C", ref _isFoundTargetNow);

        // chase to gaze
        _stateMachine.UpdateConditionOfTransition("C2G", ref _isFoundTargetNow, _tTStd, !true);

        // chase to attack
        _stateMachine.UpdateConditionOfTransition("C2A", ref _isInsideAttackingRange);

        // attack to chase
        _stateMachine.UpdateConditionOfTransition("A2C", ref _isInsideAttackingRange, _tTStd, !true);

        //// any state to death
        _stateMachine.UpdateConditionOfTransition("D2d", ref _isNoHealthNow, StateMachineTransitionType.AnyState);
        _stateMachine.UpdateConditionOfTransition("G2d", ref _isNoHealthNow, StateMachineTransitionType.AnyState);
        _stateMachine.UpdateConditionOfTransition("C2d", ref _isNoHealthNow, StateMachineTransitionType.AnyState);
        _stateMachine.UpdateConditionOfTransition("A2d", ref _isNoHealthNow, StateMachineTransitionType.AnyState);
    }

#if UNITY_EDITOR_64
    private void OnDrawGizmos()
    {
        // ����
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sightRange);

        // �U���͈�
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
#endif
}
