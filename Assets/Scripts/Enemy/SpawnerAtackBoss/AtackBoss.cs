using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtackBoss : MonoBehaviour
{
    [SerializeField] GameObject prefab; // Prefab a instanciar
    private float tiempoMinimo = 1f; // Tiempo mínimo entre instancias
    private float tiempoMaximo = 10f; // Tiempo máximo entre instancias
    private float velocidadMovimiento = 2f; // Velocidad de movimiento del prefab hacia la izquierda

    private float tiempoSiguienteInstancia; // Tiempo para la próxima instancia

    void Start()
    { 
        // Inicializar el tiempo para la primera instancia
        tiempoSiguienteInstancia = Time.time + Random.Range(tiempoMinimo, tiempoMaximo);
    }

    void Update()
    {
        // Verificar si es tiempo de generar una nueva instancia
        if (Time.time >= tiempoSiguienteInstancia)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, 180);
            // Instanciar el prefab
            GameObject nuevaInstancia = Instantiate(prefab, transform.position, rotation);
            // Aplicar movimiento hacia la izquierda
            nuevaInstancia.GetComponent<Rigidbody2D>().velocity = Vector2.left * velocidadMovimiento;
            // Actualizar el tiempo para la próxima instancia
            tiempoSiguienteInstancia = Time.time + Random.Range(tiempoMinimo, tiempoMaximo);
        }
    }
}
