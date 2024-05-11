using UnityEngine;

public class SpaceshipActive : MonoBehaviour
{
    public GameObject objectToCount;
    public GameObject objectToActivate;
    public GameObject objectToActivate2;

    // Update is called once per frame
    void Update()
    {
        bool allChildrenInactive = true;

        // Iterar a través de todos los hijos de objectToCount
        foreach (Transform child in objectToCount.transform)
        {
            // Si algún hijo está activo, establecemos la bandera allChildrenInactive en falso y salimos del bucle
            if (child.gameObject.activeSelf)
            {
                allChildrenInactive = false;
                break;
            }
        }

        // Activar los GameObjects basado en el estado de los hijos
        if (allChildrenInactive)
        {
            objectToActivate.SetActive(true);
            objectToActivate2.SetActive(true);
        }
        else
        {
            objectToActivate.SetActive(false);
            objectToActivate2.SetActive(false);
        }
    }
}
