using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoPlayer : MonoBehaviour
{
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private GameObject balaPrefab;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Disparar();
        }
    }

    private void Disparar()
    {
        GameObject balaInstance = Instantiate(balaPrefab, controladorDisparo.position, controladorDisparo.rotation);

        // Obtener la dirección de disparo basada en la escala del controladorDisparo
        Vector2 direccionDisparo = controladorDisparo.localScale.x > 0 ? Vector2.right : Vector2.left;

        // Enviar la dirección de disparo a la instancia de la bala
        Bala balaScript = balaInstance.GetComponent<Bala>();
        if (balaScript != null)
        {
            balaScript.EstablecerDireccionDeDisparo(direccionDisparo);
        }
        else
        {
            Debug.LogWarning("El prefab de la bala no tiene el componente Bala adjunto.");
        }
    }
}
