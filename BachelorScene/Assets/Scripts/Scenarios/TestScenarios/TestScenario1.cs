using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Scenarios
{
    class TestScenario1 : Scenario
    {
        public void Start_OnEnter()
        {
            Debug.Log("Start_OnEnter 1");
        }

        public string Start_OnStay()
        {
            Debug.Log("Start_OnStay 1");

            return "end";
        }

        public void Start_OnLeave()
        {
            Debug.Log("Start_OnLeave 1");
        }

        public void End_OnEnter()
        {
            Debug.Log("End_OnEnter 1");
            Debug.Log(Duration);
            Debug.Log(string.Join(", ", StateMachine.StateChanges.Select(r => $"{r.From} -> {r.To} @ {r.Time}")));

            Stop();
        }

        public string End_OnStay()
        {
            Debug.Log("End_OnStay 1");

            return "start";
        }

        public void End_OnLeave()
        {
            Debug.Log("End_OnLeave 1");
        }

        public TestScenario1()
        {
            Name = "Test 1";
            Description = "Hello\nWorld!";
            VisibleInMenu = false;

            List<State<string>> states = new List<State<string>>
            {
                new State<string>("start", Start_OnEnter, Start_OnStay, Start_OnLeave),
                new State<string>("end", End_OnEnter, End_OnStay, End_OnLeave)
            };

            StateMachine = new StateMachine<string>(states, "start");
        }
    }
}
