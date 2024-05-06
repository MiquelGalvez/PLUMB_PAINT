using UnityEngine;

public class SpaceshipActive : MonoBehaviour
{
    public GameObject objectToCount; // El GameObject cuyos hijos se contar�n
    public GameObject objectToActivate; // El GameObject que se activar� cuando objectToCount no tenga hijos
    public GameObject objectToActivate2; // El GameObject que se activar� cuando objectToCount no tenga hijos
    public GameObject objectToActivate3; // El GameObject que se activar� cuando objectToCount no tenga hijos

    // Update is called once per frame
    void Update()
    {
        // Verificar si objectToCount no tiene ning�n hijo
        if (objectToCount.transform.childCount == 0)
        {
            // Activar el GameObject deseado si no hay hijos
            objectToActivate.SetActive(true);
            objectToActivate2.SetActive(true);
            objectToActivate3.SetActive(true);
        }
        else
        {
            // Desactivar el GameObject deseado si hay hijos
            objectToActivate.SetActive(false);
            objectToActivate2.SetActive(false);
            objectToActivate3.SetActive(false);
        }
    }
}
