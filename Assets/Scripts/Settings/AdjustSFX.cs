using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class aDJUSTsfx : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;


    public void ControlMusica (float sliderSFX)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sliderSFX) * 20);
    }
}
