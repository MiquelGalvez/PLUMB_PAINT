using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Comptador : MonoBehaviour
{
    public TextMeshProUGUI textoContador; // Referencia al objeto de texto donde se mostrará el contador
    public GameObject imagenParpadeante; // Referencia al objeto de la imagen que parpadeará cuando el tiempo llegue a cero
    [SerializeField] GameObject PassScene; // Referencia al objeto de la imagen que parpadeará cuando el tiempo llegue a cero

    private GameObject spawner;
    private float tiempoRestante = 10f; // Tiempo inicial en segundos (dos minutos)
    private bool tiempoTerminado = false; // Flag para verificar si el tiempo ha terminado

    private void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("Spawn1");
        StartCoroutine(ActualizarContador()); // Comenzar la coroutine para actualizar el contador
    }

    private IEnumerator ActualizarContador()
    {
        while (tiempoRestante > 0)
        {
            // Actualizar el texto del contador con el tiempo restante formateado
            textoContador.text = FormatearTiempo(tiempoRestante);

            // Reducir el tiempo restante
            tiempoRestante -= Time.deltaTime;

            yield return null; // Esperar un frame antes de continuar
        }

        // Cuando el tiempo se acabe, mostrar "GO!!!!", desactivar el spawner y activar el flag de tiempo terminado
        textoContador.text = "GO!!!!";
        tiempoTerminado = true;
        imagenParpadeante.SetActive(true);
        PassScene.SetActive(true);
        StartCoroutine(ParpadearImagen());
    }

    private string FormatearTiempo(float tiempo)
    {
        // Convertir el tiempo de segundos a minutos y segundos
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);

        // Formatear el tiempo en minutos y segundos
        string tiempoFormateado = string.Format("{0:00}:{1:00}", minutos, segundos);

        return tiempoFormateado;
    }

    private IEnumerator ParpadearImagen()
    {
        while (tiempoTerminado)
        {
            // Alternar entre activar y desactivar la imagen parpadeante
            imagenParpadeante.SetActive(!imagenParpadeante.activeSelf);

            // Esperar un corto período de tiempo antes de continuar
            yield return new WaitForSeconds(0.5f);
        }
    }
}
