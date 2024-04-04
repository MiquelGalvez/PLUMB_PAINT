using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLogic : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Reproduir el so de col�lisi� si l'objecte amb el qual ha col�lisionat �s el jugador
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
