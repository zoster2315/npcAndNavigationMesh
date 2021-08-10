using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public FSMStateType StartState = FSMStateType.Patrol;
    private IFSMState[] StatePool;
    private IFSMState CurrentState;
    private readonly IFSMState EmptyAction = new EmptyAction();

    private void Awake()
    {
        StatePool = GetComponents<IFSMState>();
    }

    private void Start()
    {
        CurrentState = EmptyAction;
        TransitionToState(StartState);
    }

    private void TransitionToState(FSMStateType stateName)
    {
        CurrentState.OnExit();
        CurrentState = GetState(stateName);
        CurrentState.OnEnter();
        Debug.LogFormat("Transition to {0}", CurrentState.StateName);
    }

    IFSMState GetState(FSMStateType stateName)
    {
        foreach (var state in StatePool)
            if (state.StateName == stateName)
                return state;
        return EmptyAction;
    }

    private void Update()
    {
        CurrentState.DoAction();
        FSMStateType transitionState = CurrentState.ShouldTransitionToState();

        if (transitionState != CurrentState.StateName)
            TransitionToState(transitionState);
    }
}
