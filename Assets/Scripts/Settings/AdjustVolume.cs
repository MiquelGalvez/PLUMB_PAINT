using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AdjustVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;


    public void ControlMusica (float sliderMusica)
    {
        audioMixer.SetFloat("VolumenMaster", Mathf.Log10(sliderMusica) * 20);
    }
}
