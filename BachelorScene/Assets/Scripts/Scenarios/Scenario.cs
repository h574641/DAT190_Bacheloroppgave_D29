using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public bool VisibleInMenu { get; protected set; }

    public StateMachine<string> StateMachine { get; protected set; }

    public long StartTime { get; protected set; } = -1L;
    public long EndTime { get; protected set; } = -1L;

    public bool Running { get; protected set; }

    public long Duration {
        get
        {
            if (Running && StateMachine != null && StartTime != -1L)
            {
                return StateMachine.TimingFunction.Invoke() - StartTime;
            }
            else
            {
                return -1L;
            }
        }
    }

    public Scenario(string name=null, string description=null, StateMachine<string> stateMachine=null)
    {
        Name = name;
        Description = description;
        StateMachine = stateMachine;

        StartTime = -1L;
        EndTime = -1L;

        Running = false;
        VisibleInMenu = true;
    }

    public virtual void Update()
    {
        if (Running)
        {
            StateMachine.Update();
        }
    }

    public virtual void Start()
    {
        StartTime = StateMachine?.TimingFunction.Invoke() ?? -1L;
        Running = true;
    }

    public virtual void Stop()
    {
        EndTime = StateMachine?.TimingFunction.Invoke() ?? -1L;
        Running = false;
    }
}
