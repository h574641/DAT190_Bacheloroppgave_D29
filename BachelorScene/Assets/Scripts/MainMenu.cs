using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void StartTutorial()
    {
        SceneManager.LoadScene(1);
    }

    public void StartSimulation()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartMenu()
    {
        SceneManager.LoadScene(0);
    }
}
