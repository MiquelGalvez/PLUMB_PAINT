using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

public class RestartLogic : MonoBehaviour
{
    [SerializeField] private GameObject youdied;
    [SerializeField] private GameObject hud;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Reproduir el so de col·lisió si l'objecte amb el qual ha col·lisionat és el jugador
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            enabled = false;
            youdied.SetActive(true);
            hud.SetActive(false);
            Cursor.visible = true;
        }
    }
}
