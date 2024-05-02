using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Comptador : MonoBehaviour
{
    public TextMeshProUGUI textoContador; // Referencia al objeto de texto donde se mostrará el contador

    private float tiempoRestante = 120f; // Tiempo inicial en segundos (dos minutos)

    private void Start()
    {
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

        // Cuando el tiempo se acabe, mostrar "Tiempo terminado" o realizar alguna acción adicional
        textoContador.text = "GO!!!!";
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
}
