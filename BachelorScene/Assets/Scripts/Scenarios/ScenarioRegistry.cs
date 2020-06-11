using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ScenarioRegistry
{
    private static Dictionary<string, Func<Scenario>> scenariosFunctions = new Dictionary<string, Func<Scenario>>();
    private static ReadOnlyDictionary<string, Func<Scenario>> scenariosFunctionsView = new ReadOnlyDictionary<string, Func<Scenario>>(scenariosFunctions);

    // Used for grabbing defaults from ctors, not for use as actual scenarios
    private static Dictionary<string, Scenario> sampleScenarios = new Dictionary<string, Scenario>();
    private static ReadOnlyDictionary<string, Scenario> sampleScenariosView = new ReadOnlyDictionary<string, Scenario>(sampleScenarios);

    public static List<String> ScenarioNames
    {
        get
        {
            return scenariosFunctions.Keys.ToList();
        }
    }

    public static ReadOnlyDictionary<string, Func<Scenario>> Scenarios
    {
        get
        {
            return scenariosFunctionsView;
        }
    }

    public static ReadOnlyDictionary<string, Scenario> SampleScenarios
    {
        get
        {
            return sampleScenariosView;
        }
    }

    // Use reflection to grab all existing scenarios
    static ScenarioRegistry()
    {
        List<ConstructorInfo> ctorInfos = ReflectionMagic.GetAllTypeDefaultConstructors(typeof(Scenario));

        foreach (ConstructorInfo info in ctorInfos)
        {
            if (info != null)
            {
                AddScenario(() => (Scenario)info.Invoke(new object[] { }));
            }
        }
    }

    public static void AddScenario(Func<Scenario> func)
    {
        // Construct a scenario to get its name
        try
        {
            Scenario scenario = func.Invoke();

            scenariosFunctions.Add(scenario.Name, func);
            sampleScenarios.Add(scenario.Name, scenario);
        }
        catch (TargetParameterCountException)
        {
            // No 0 argument ctor, ignore, this is most likely the base class
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}