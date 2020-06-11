using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    public String ScenarioName;
    public static Scenario CurrentScenario { get; protected set; }

    public void Start()
    {
        StartScenario(ScenarioName);
    }

    public void Update()
    {
        CurrentScenario?.Update();
    }

    public static void StartScenario(string name)
    {
        Func<Scenario> scenario;

        if (ScenarioRegistry.Scenarios.TryGetValue(name, out scenario))
        {
            StartScenario(scenario.Invoke());
        }
        else
        {
            Debug.LogError($"ScenarioController: {name} Scenario not found");
        }
    }

    public static void StartScenario(Scenario scenario)
    {
        CurrentScenario = scenario;
        CurrentScenario?.Start();
    }

    public static void StopScenario()
    {
        CurrentScenario?.Stop();
    }
}
