using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            // Mantén este GameObject en todas las escenas.
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            // Si ya existe una instancia de este GameObject, destruye este para evitar duplicados.
            Destroy(gameObject);
        }
    }
}
