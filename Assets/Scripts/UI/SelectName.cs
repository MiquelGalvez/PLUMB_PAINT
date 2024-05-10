using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoCanvas;

    // Esta función se llama cuando el usuario presiona Enter en el campo de entrada de texto
    public void ActualizarTexto(TMP_InputField input)
    {
        Debug.Log("Input text: " + input.text);
        textoCanvas.text = input.text;
        Debug.Log("Canvas text: " + textoCanvas.text);
    }
}
