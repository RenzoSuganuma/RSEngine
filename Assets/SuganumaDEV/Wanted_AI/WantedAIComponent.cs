using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RSEngine.AI.StateMachine;

/// <summary> AI�@�\��񋟂���R���|�[�l���g </summary>
public class WantedAIComponent : MonoBehaviour, IStateMachineUser
{
    // �X�e�[�g�}�V��
    /// <summary> AI�̃X�e�[�g�x�[�X�ȏ������T�|�[�g���邽�߂̃X�e�[�g�}�V�� </summary>
    StateMachine _sMachine = new StateMachine();

    // �X�e�[�g
    /// <summary> �f�t�H���g�X�e�[�g�F�A�C�h�� </summary>
    WantedAIStateDefault _sDef = new WantedAIStateDefault();
    /// <summary> �X�e�[�g�F���� </summary>
    WantedAIStateGaze _sGaze = new WantedAIStateGaze();
    /// <summary> �X�e�[�g�F�ǐ� </summary>
    WantedAIStateChase _sChase = new WantedAIStateChase();
    /// <summary> �X�e�[�g�F�U�� </summary>
    WantedAIStateAttack _sAttack = new WantedAIStateAttack();
    /// <summary> �X�e�[�g�F���S </summary>
    WantedAIStateDeath _sDeath = new WantedAIStateDeath();

    // AI�X�e�[�g����
    bool _isDefalultNow = false;
    bool _isGazing = false;
    bool _isChasing = false;
    bool _isAttacking = false;
    bool _isDeathNow = false;

    // AI�g�����W�V�����t���O
    bool _isInsideSightRange = false; // �f�t�H���g���璍������܂ł̏���
    bool _isFoundTargetNow = false; // �������I���A�v���C���[�Ƃ��Ĕ��肵���ꍇ�@�ǐՂ��邩�̏���
    bool _isInsideAttackingRange = false; // �ǐՂ����Ă��čU���\�����Ƀv���C���[���������ꍇ�@�U�����邩�̏���
    bool _isNoHealthNow = false;�@// ���S�������ꍇ

    public void OnStateWasExitted(StateTransitionInfo info)
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        // �C�x���g���X�i�[�o�^
        _sMachine.onStateExit += OnStateWasExitted;
        // �e�X�e�[�g�̓o�^
        _sMachine.AddStates(new List<IState> {
        _sDef,
        _sGaze,
        _sChase,
        _sAttack,
        _sDeath});
        // �ʏ킩�璍��
        _sMachine.AddTransition(_sDef, _sGaze); // default to gaze id{0}
        _sMachine.AddTransition(_sGaze, _sDef); // gaze to default id{1}
        // ��������ǐ�
        _sMachine.AddTransition(_sGaze, _sChase); // gaze to chase id{2}
        _sMachine.AddTransition(_sGaze, _sChase); // chase to gaze id{3}
        //�@�ǐՂ���U��
        _sMachine.AddTransition(_sChase, _sAttack); // chase to attack id{4}
        _sMachine.AddTransition(_sAttack, _sChase); // attack to chase id{5}
        // ���S�X�e�[�g
        _sMachine.AddTransition(_sDef, _sDeath); // id{6}
        _sMachine.AddTransition(_sGaze, _sDeath); // id{7}
        _sMachine.AddTransition(_sChase, _sDeath); // id{8}
        _sMachine.AddTransition(_sAttack, _sDeath); // id{9}
        // �X�e�[�g�}�V���N��
        _sMachine.Initialize();
    }

    private void OnDisable()
    {
        //�@���X�i�[�o�^����
        _sMachine.onStateExit -= OnStateWasExitted;
    }

    private void FixedUpdate()
    {
        // defalut to gaze
        _sMachine.UpdateTransitionCondition(0, _isInsideSightRange);
        _sMachine.UpdateTransitionCondition(1, !_isInsideSightRange);
        // gaze to chase
        _sMachine.UpdateTransitionCondition(2, _isFoundTargetNow);
        _sMachine.UpdateTransitionCondition(3, !_isFoundTargetNow);
        // chase to attack
        _sMachine.UpdateTransitionCondition(4, _isInsideAttackingRange);
        _sMachine.UpdateTransitionCondition(5, !_isInsideAttackingRange);
        // any state to death
        _sMachine.UpdateTransitionCondition(6, _isNoHealthNow);
        _sMachine.UpdateTransitionCondition(7, _isNoHealthNow);
        _sMachine.UpdateTransitionCondition(8, _isNoHealthNow);
        _sMachine.UpdateTransitionCondition(9, _isNoHealthNow);
    }
}