using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RSEngine.StateMachine;
public class SMachineUserTestClass : MonoBehaviour
{
    [SerializeField] bool _a2b;
    [SerializeField] bool _b2a;
    [SerializeField] bool _b2c;
    [SerializeField] bool _b2d;
    [SerializeField] bool _d2a;
    StateMachineCore _stateMachine = new();
    DummyBehaviourClass _dclassA = new("CLASS_A_"), _dclassB = new("CLASS_B_");
    DummyBehaviourClass _dclassC = new("CLASS_C_"), _dclassD = new("CLASS_D_");
    private void Awake()
    {
        _stateMachine.ResistState(_dclassA);
        _stateMachine.ResistState(_dclassB);
        _stateMachine.ResistState(_dclassC);
        _stateMachine.ResistState(_dclassD);

        _stateMachine.ResistTransition(_dclassA, _dclassB, "Dummy_Transition_A2B");
        _stateMachine.ResistTransition(_dclassB, _dclassA, "Dummy_Transition_B2A");

        _stateMachine.ResistTransition(_dclassB, _dclassC, "Dummy_Transition_B2C");

        _stateMachine.ResistTransition(_dclassB, _dclassD, "Dummy_Transition_B2D");

        _stateMachine.ResistTransition(_dclassD, _dclassA, "Dummy_Transition_D2A");

        _stateMachine.PopStateMachine();
    }
    private void Update()
    {
        _stateMachine.UpdateConditionOfTransition("Dummy_Transition_A2B", ref _a2b);
        _stateMachine.UpdateConditionOfTransition("Dummy_Transition_B2A", ref _b2a);

        _stateMachine.UpdateConditionOfTransition("Dummy_Transition_B2C", ref _b2c);

        _stateMachine.UpdateConditionOfTransition("Dummy_Transition_B2D", ref _b2d);

        _stateMachine.UpdateConditionOfTransition("Dummy_Transition_D2A", ref _d2a);
    }
}
public class DummyBehaviourClass : IState
{
    string _name;
    public DummyBehaviourClass(string name) => _name = name;
    public void Entry()
    {
        Debug.Log($"Istate class : {_name} Entried");
    }

    public void Exit()
    {
        Debug.Log($"Istate class : {_name} Exitted");
    }

    public void Update()
    {
        Debug.Log($"Istate class : {_name} Update");
    }
}