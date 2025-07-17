using System;
using System.Collections.Generic;

public class StateMachine
{
    private IState currentState;

    private Dictionary<Type, List<Transition>> transitions = new Dictionary<Type, List<Transition>>();
    private List<Transition> currentTransitions = new List<Transition>();
    private List<Transition> anyTransitions = new List<Transition>();

    private List<Transition> EmptyTransitions = new List<Transition>(0);

    // Must be called on the AI MonoBehaviour Update()
    public void Tick()
    {
        Transition transition = GetTransition();

        if (transition != null)
        {
            SetState(transition.To);
        }

        currentState?.Tick();
    }

    public void SetState(IState state)
    {
        if (state == currentState) return;

        currentState?.OnExit();
        currentState = state;

        transitions.TryGetValue(currentState.GetType(), out currentTransitions);

        if (currentTransitions == null)
        {
            currentTransitions = EmptyTransitions;
        }

        currentState.OnEnter();
    }

    public void AddTransition(IState from, IState to, Func<bool> predicate)
    {
        if (!transitions.TryGetValue(from.GetType(), out var transitionsList))
        {
            transitionsList = new List<Transition>();
            transitions[from.GetType()] = transitionsList;
        }

        transitionsList.Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(IState state, Func<bool> predicate)
    {
        anyTransitions.Add(new Transition(state, predicate));
    }

    private class Transition
    {
        public Func<bool> Condition { get; }
        public IState To { get; }

        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    private Transition GetTransition()
    {
        foreach (Transition transition in anyTransitions)
        {
            if (transition.Condition())
            {
                return transition;
            }
        }

        foreach (Transition transition in currentTransitions)
        {
            if (transition.Condition())
            {
                return transition;
            }
        }

        return null;
    }
}