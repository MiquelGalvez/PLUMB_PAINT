using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AdjustVolume : MonoBehaviour
{
    public AudioMixer audioMixer; // Referencia al Audio Mixer
    public Slider slider; // Referencia al Slider para el volumen

    public string parameterName; // Nombre del parámetro en el Audio Mixer

    void Start()
    {
        // Asegurarse de que el slider esté sincronizado con el valor del Audio Mixer al iniciar
        float initialValue;
        audioMixer.GetFloat(parameterName, out initialValue);
    }

    public void SetLevel(float sliderValue)
    {
        // Convierte el valor del slider (0-1) a un valor en decibeles
        float decibelValue = Mathf.Log10(sliderValue);

        // Actualiza el valor del parámetro en el Audio Mixer
        audioMixer.SetFloat(parameterName, decibelValue);
    }
}
