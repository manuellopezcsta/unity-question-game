using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameManager GMscript;
    public void PlayButton()
    {
        GMscript.StartGame();
    }
    public void HowToButton()
    {
        
    }
    public void SubmitQuestions()
    {
        
    }
    public void ExitApp()
    {
        Application.Quit();
        ////Debug.Log("Closing Game");
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
