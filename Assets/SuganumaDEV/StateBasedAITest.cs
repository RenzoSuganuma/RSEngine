using RSEngine.AI.StateMachine;
using UnityEngine;
public class StateBasedAITest : MonoBehaviour, IStateMachineUser
{
    // �X�e�[�g�}�V��
    StateMachine _smachine = new();
    // �e�X�e�[�g
    Idle _sidle = new();
    Chase _schase = new();
    Attack _sattack = new();

    [SerializeField] Transform _targetTransform;
    [SerializeField, Range(0, 50)] float sightR;
    [SerializeField, Range(0, 50)] float attackR;

    bool isSightable = false;
    bool isAttackable = false;

    public void OnStateWasExitted(StateTransitionInfo info)
    {

    }

    private void Awake()
    {
        // �X�e�[�g�̓o�^  - [1]
        _smachine.AddState(_sidle);
        _smachine.AddState(_schase);
        _smachine.AddState(_sattack);
        // �X�e�[�g�̑J�ڂ̓o�^  - [2]
        _smachine.AddTransition(_sidle, _schase); // id - 0
        _smachine.AddTransition(_schase, _sidle); // id - 1
        _smachine.AddTransition(_schase, _sattack); // id - 2
        _smachine.AddTransition(_sattack, _schase); // id - 3
        //  �X�e�[�g�C�x���g�̃��X�i�[�̓o�^  - [3]����Ɏ��s
        _smachine.onStateExit += OnStateWasExitted;
        //�X�e�[�g�}�V���N��  - [3]
        _smachine.Initialize();
        // �e�X�g���s
        _smachine.Update();
    }

    private void FixedUpdate()
    {
        isSightable = Vector3.Distance(_targetTransform.position, transform.position) < sightR;
        isAttackable = Vector3.Distance(_targetTransform.position, transform.position) < attackR;

        // �e�g�����W�V�����̃R���f�B�V�������X�V [1]
        _smachine.UpdateTransitionCondition(0, isSightable);
        _smachine.UpdateTransitionCondition(1, !isSightable);
        _smachine.UpdateTransitionCondition(2, isAttackable);
        _smachine.UpdateTransitionCondition(3, !isAttackable);
        // �X�e�[�g�}�V���̍X�V [2]
        _smachine.Update();
    }

    private void OnDisable()
    {
        _smachine.onStateExit -= OnStateWasExitted;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightR);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackR);
    }
}
class Idle : IState
{
    bool _ready2GoNextState = false;
    public void In()
    {

    }

    public void Out()
    {

    }

    public bool ReadyToGoNext()
    {
        return _ready2GoNextState;
    }

    public void SendMessageGotoNext()
    {
        _ready2GoNextState = true;
    }

    public void Do()
    {
        Debug.Log("���Ȃ�...");
    }
}
class Chase : IState
{
    bool _ready2GoNextState = false;
    public void In()
    {

    }

    public void Out()
    {

    }

    public bool ReadyToGoNext()
    {
        return _ready2GoNextState;
    }

    public void SendMessageGotoNext()
    {
        _ready2GoNextState = true;
    }

    public void Do()
    {
        Debug.Log("���������I");
    }
}
class Attack : IState
{
    bool _ready2GoNextState = false;
    public void In()
    {
    }

    public void Out()
    {
    }

    public bool ReadyToGoNext()
    {
        return _ready2GoNextState;
    }

    public void SendMessageGotoNext()
    {
        _ready2GoNextState = true;
    }

    public void Do()
    {
        Debug.Log("����...!");
    }
}