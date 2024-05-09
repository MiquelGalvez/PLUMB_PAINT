using UnityEngine;

public class SpaceshipActive : MonoBehaviour
{
    public GameObject objectToCount;
    public GameObject objectToActivate;
    public GameObject objectToActivate2;

    // Update is called once per frame
    void Update()
    {
        // Verificar si objectToCount no tiene ningún hijo
        if (objectToCount.transform.childCount == 0)
        {
            // Activar el GameObject deseado si no hay hijos
            objectToActivate.SetActive(true);
            objectToActivate2.SetActive(true);
        }
        else
        {
            // Desactivar el GameObject deseado si hay hijos
            objectToActivate.SetActive(false);
            objectToActivate2.SetActive(false);
        }
    }
}
