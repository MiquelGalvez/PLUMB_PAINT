using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public TextMeshProUGUI counterText; // Reference to the text object where the counter will be displayed
    public GameObject flashingImage; // Reference to the image object that will flash when time runs out
    [SerializeField] GameObject PassScene; // Reference to the image object that will flash when time runs out

    private GameObject spawner;
    private float remainingTime = 31f; // Initial time in seconds (two minutes)
    private bool timeUp = false; // Flag to check if time is up

    private void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("Spawn1");
        StartCoroutine(UpdateCounter()); // Start the coroutine to update the counter
    }

    private IEnumerator UpdateCounter()
    {
        while (remainingTime > 0)
        {
            // Update the counter text with the formatted remaining time
            counterText.text = FormatTime(remainingTime);

            // Reduce the remaining time
            remainingTime -= Time.deltaTime;

            yield return null; // Wait for one frame before continuing
        }

        // When time is up, show "GO!!!!", deactivate the spawner, and set the time up flag
        counterText.text = "GO!!!!";
        timeUp = true;
        flashingImage.SetActive(true);
        PassScene.SetActive(true);
        StartCoroutine(FlashImage());
    }

    private string FormatTime(float time)
    {
        // Convert time from seconds to minutes and seconds
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        // Format time into minutes and seconds
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        return formattedTime;
    }

    private IEnumerator FlashImage()
    {
        while (timeUp)
        {
            // Toggle between activating and deactivating the flashing image
            flashingImage.SetActive(!flashingImage.activeSelf);

            // Wait for a short period of time before continuing
            yield return new WaitForSeconds(0.5f);
        }
    }
}
