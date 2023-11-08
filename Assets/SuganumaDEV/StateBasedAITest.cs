using RSEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StateBasedAITest : MonoBehaviour, IStateMachineUser
{
    // �X�e�[�g�}�V��
    StateMachine _smachine = new();
    // �e�X�e�[�g
    Idle _sidle = new();
    Chase _schase = new();
    Attack _sattack = new();

    public void OnStateWasExitted(StateTransitionInfo info)
    {
        
    }
    private void Awake()
    {
        // �X�e�[�g�̓o�^ [1]
        _smachine.AddState(_sidle);
        _smachine.AddState(_schase);
        _smachine.AddState(_sattack);
        // �X�e�[�g�̑J�ڂ̓o�^ [2]
        _smachine.AddTransition(_sidle, _schase);
        //  �X�e�[�g�C�x���g�̃��X�i�[�̓o�^ [3]����Ɏ��s
        _smachine.onStateExit += OnStateWasExitted;
        //�X�e�[�g�}�V���N�� [3]
        _smachine.Initialize();
        // �e�X�g���s
        _smachine.Update();
    }
    private void FixedUpdate()
    {
        _smachine.UpdateCondition(0, true);
        _smachine.Update();
    }
    private void OnDisable()
    {
        _smachine.onStateExit -= OnStateWasExitted;
    }
}
class Idle : IState
{
    bool _ready2GoNextState = false;
    public void In()
    {
        Debug.Log($"Idle : {nameof(this.In)}");
    }

    public void Out()
    {
        Debug.Log($"Idle : {nameof(this.Out)}");
    }

    public bool ReadyToGoNext()
    {
        return _ready2GoNextState;
    }

    public void SendMessageGotoNext()
    {
        _ready2GoNextState = true;
    }

    public void Tick()
    {
        Debug.Log($"Idle : {nameof(this.Tick)}");
    }
}
class Chase : IState
{
    bool _ready2GoNextState = false;
    public void In()
    {
        Debug.Log($"Chase : {nameof(this.In)}");
    }

    public void Out()
    {
        Debug.Log($"Chase : {nameof(this.Out)}");
    }

    public bool ReadyToGoNext()
    {
        return _ready2GoNextState;
    }

    public void SendMessageGotoNext()
    {
        _ready2GoNextState = true;
    }

    public void Tick()
    {
        Debug.Log($"Chase : {nameof(this.Tick)}");
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

    public void Tick()
    {
    }
}