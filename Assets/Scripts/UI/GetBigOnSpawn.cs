using UnityEngine;

public class GetBigOnSpawn : MonoBehaviour
{
    // VARIABLES PRIVADAS
    public bool onStart = true;
    public bool bigToSmall;

    [SerializeField] private float targetSize;
    [SerializeField] private float sizeSpeed;

    void Update()
    {
        //DE MÁS PEQUEÑO A GRANDE
        if (!bigToSmall)
        {
            // SI LA BANDERA 'BIGSMALL' ES FALSA
            // Y LA ESCALA ACTUAL ES MENOR QUE EL TAMAÑO DESEADO, Y ONSTART ESTÁ ACTIVO
            if (transform.localScale.x < targetSize && onStart)
            {
                // AUMENTAR GRADUALMENTE EL TAMAÑO DEL OBJETO
                transform.localScale += new Vector3(sizeSpeed, sizeSpeed, sizeSpeed) * Time.deltaTime;
            }
            else
            {
                // REDUCIR GRADUALMENTE EL TAMAÑO DEL OBJETO
                Destroy(gameObject);
            }
        }
        else
        {
            // SI LA BANDERA 'BIGSMALL' ES VERDADERA
            // Y LA ESCALA ACTUAL ES MAYOR QUE CERO
            if (transform.localScale.x > 0 && onStart)
            {
                // REDUCIR GRADUALMENTE EL TAMAÑO DEL OBJETO
                transform.localScale -= new Vector3(sizeSpeed, sizeSpeed, sizeSpeed) * Time.deltaTime;
            }
            else
            {
                // DESTRUIR EL INICIO UNA VEZ QUE SE ALCANZA EL TAMAÑO DESEADO
                Destroy(gameObject);
            }
        }
    }
}
