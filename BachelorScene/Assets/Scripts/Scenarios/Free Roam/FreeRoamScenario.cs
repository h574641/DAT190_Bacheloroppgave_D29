using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Scripts.Scenarios
{
    public class FreeRoamScenario : Scenario
    {
            private Scenario1ObjectCollection objectCollection;

            public void Start_OnEnter()
            {
                Debug.Log("Start_OnEnter");
            }

            public string Start_OnStay()
            {
                Debug.Log("Start_OnStay");

                return "end";
            }

            public void Start_OnLeave()
            {
                Debug.Log("Start_OnLeave");
            }

            public void End_OnEnter()
            {
                Debug.Log("End_OnEnter");
                Debug.Log(Duration);
                Debug.Log(string.Join(", ", StateMachine.StateChanges.Select(r => $"{r.From} -> {r.To} @ {r.Time}")));

                Stop();
            }

            public string End_OnStay()
            {
                Debug.Log("End_OnStay");

                return "start";
            }

            public void End_OnLeave()
            {
                Debug.Log("End_OnLeave");
            }

            public FreeRoamScenario()
            {
                Name = "Free roam, utforsk simulatoren";
                Description = "Du kan utforske simulatoren og gjøre deg kjent med mulighetene i den uten tidsbegrensninger eller mål. Bruk menyen for å starte et annet scenario.";

                List<State<string>> states = new List<State<string>>
                {
                    new State<string>("start", Start_OnEnter, Start_OnStay, Start_OnLeave),
                    new State<string>("end", End_OnEnter, End_OnStay, End_OnLeave)
                };

                StateMachine = new StateMachine<string>(states, "start");
            }
        }
}