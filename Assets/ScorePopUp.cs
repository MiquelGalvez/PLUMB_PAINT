using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePopUp : MonoBehaviour
{
    [SerializeField] private float movSpeed = 1f;
    private TextMeshPro textMesh;
    private string text;
    public void SetText(string textValue) { text = textValue; }

    private void Awake(){textMesh = GetComponent<TextMeshPro>();}
    private void Start(){textMesh.text = text;}
    private void Update(){transform.Translate((Vector2.up * movSpeed) * Time.deltaTime);}
}
