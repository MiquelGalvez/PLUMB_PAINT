using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // El prefab que quieres instanciar
    private float tiempoMinimo = 4f; // Tiempo m�nimo entre cada instancia
    private float tiempoMaximo = 10f; // Tiempo m�ximo entre cada instancia

    void Start()
    {
        // Inicializamos el tiempo para la primera instancia
        Invoke("SpawnPrefab", Random.Range(tiempoMinimo, tiempoMaximo));
    }

    void SpawnPrefab()
    {
        // Instanciamos el prefab en la posici�n del GeneradorPrefab
        GameObject car = Instantiate(prefab, transform.position, Quaternion.identity);
        // Obtener el script de movimiento del carro
        CarMovement carMovement = car.GetComponent<CarMovement>();
        // Configurar la velocidad del carro
        carMovement.SetSpeed(7f);

        // Llamar al m�todo SpawnPrefab de nuevo despu�s de un tiempo aleatorio
        Invoke("SpawnPrefab", Random.Range(tiempoMinimo, tiempoMaximo));
    }
}
