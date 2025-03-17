using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        FindAnyObjectByType<AudioManager>().Play("ButtonClick");
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        FindAnyObjectByType<AudioManager>().Play("ButtonClick");
        Application.Quit();
    }
}
