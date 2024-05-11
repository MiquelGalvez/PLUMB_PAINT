using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    private void Start()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void OnRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        Cursor.visible = false;
    }

    public void OnQuitButton()
    {
        SceneManager.LoadScene(0);
    }
}