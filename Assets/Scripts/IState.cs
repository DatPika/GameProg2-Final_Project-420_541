using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle,
    Patrol,
    Chase,
    Attack
}
public interface IState
{
    StateType Type { get; }
    void Enter();
    void Execute();
    void Exit();
}
