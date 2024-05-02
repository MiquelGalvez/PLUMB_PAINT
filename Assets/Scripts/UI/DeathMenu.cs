using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject player;

    public void OnRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f; 
        Cursor.visible = false;
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
