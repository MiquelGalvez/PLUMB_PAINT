using UnityEngine;

public class SpaceshipActive : MonoBehaviour
{
    public GameObject objectToCount; // El GameObject cuyos hijos se contarán
    public GameObject objectToActivate; // El GameObject que se activará cuando objectToCount no tenga hijos

    // Update is called once per frame
    void Update()
    {
        // Verificar si objectToCount no tiene ningún hijo
        if (objectToCount.transform.childCount == 0)
        {
            // Activar el GameObject deseado si no hay hijos
            objectToActivate.SetActive(true);
        }
        else
        {
            // Desactivar el GameObject deseado si hay hijos
            objectToActivate.SetActive(false);
        }
    }
}
