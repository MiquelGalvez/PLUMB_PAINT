using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActivatePlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI namePlayer;
    // Start is called before the first frame update

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    // Update is called once per frame
    void Update()
    {
        // Check if namePlayer text is not empty or null
        if (!string.IsNullOrEmpty(namePlayer.text))
        {
            player.SetActive(true);
        }
        else
        {
            player.SetActive(false);
        }
    }
}
