using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    private float previousTimescale = 1f;

    public GameObject MenuObject;
    public GameObject PauseMenuUI;
    public Dropdown ScenarioList;
    public Text ScenarioDescription;
    public bool StopTimeDuringPause = false;
    public ScenarioController scenarioScript;
    public Scenario1ObjectCollection objectCollection;

    private string lastScenarioOption = null;

    void Start()
    {
        MenuObject?.SetActive(false);
        PopulateScenarioList();
    }

    public void PopulateScenarioList()
    {
        int index = 0;
        int currentIndex = 0;
        string currentScenario = ScenarioController.CurrentScenario?.Name ?? scenarioScript.ScenarioName;

        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        foreach (string name in ScenarioRegistry.ScenarioNames)
        {
            if (ScenarioRegistry.SampleScenarios[name].VisibleInMenu)
            {
                options.Add(new Dropdown.OptionData(name));

                if (name == currentScenario)
                {
                    currentIndex = index;
                }
            }

            index++;
        }

        ScenarioList.ClearOptions();
        ScenarioList.AddOptions(options);

        ScenarioList.value = currentIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        objectCollection.EndEvaluationController.showEvaluation = GameIsPaused;
    }

    public void SetPositionInfrontOfPlayer(float distance = 1.25f)
    {
        Camera camera = Camera.current ?? Camera.allCameras?[0];

        if (camera != null)
        {
            transform.position = camera.transform.position + camera.transform.forward * distance;
            transform.rotation = Quaternion.Euler(camera.transform.rotation.eulerAngles.x, camera.transform.rotation.eulerAngles.y, 0f);
        }
    }

    public void Resume()
    {
        MenuObject?.SetActive(false);
        GameIsPaused = false;

        if (StopTimeDuringPause)
        {
            Time.timeScale = previousTimescale;
        }
    }

    public void Pause()
    {
        SetPositionInfrontOfPlayer();
        MenuObject?.SetActive(true);
        previousTimescale = Time.timeScale;
        GameIsPaused = true;

        if (StopTimeDuringPause)
        {
            Time.timeScale = 0f;
        }
    }

    public void Restart()
    {
        //restart the game
        //SceneManager.LoadScene("SampleScene");
        Resume();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public string ScenarioDropdownSelect()
    {
        return ScenarioList.options[ScenarioList.value].text;
    }

    public void Button_Pause()
    {
        Pause();
    }

    public void Button_Resume()
    {
        Resume();
    }

    public void Button_Restart()
    {
        Restart();
    }

    public void Button_StartScenario()
    {
        string scenarioName = ScenarioDropdownSelect();

        ScenarioController.StartScenario(scenarioName);
    }

    void OnGUI()
    {
        string scenarioName = ScenarioDropdownSelect();

        if (lastScenarioOption != scenarioName)
        {
            ScenarioDescription.text = ScenarioRegistry.SampleScenarios[scenarioName].Description;
            lastScenarioOption = scenarioName;
        }
    }
}
