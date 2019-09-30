using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool P1Selected = true;
    public void playGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void P1SelectToggle()
    {
        P1Selected = !P1Selected;
        Debug.Log(P1Selected);
    }

    public void closeGame()
    {
        Application.Quit();
    }
    
    
}
    