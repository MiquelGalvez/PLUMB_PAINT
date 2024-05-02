using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIFadeController : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private bool fadeIn;
    [SerializeField] private bool alternateFade = true;
    
    [Space(2)]
    [Header("EVENTS")]
    [Space(2)]

    [SerializeField] private bool activateObjects;
    [SerializeField] private GameObject[] gameObjectsToActivate;
    [SerializeField] private bool loadScene;
    [SerializeField] private string sceneName;

    private Image image;

    void Awake(){image = GetComponent<Image>();}
    private void OnEnable()
    {
        ForceFade();
    }
    public void ForceFade()
    {
        //INICIAR FADE AL COMENZAR
        if (fadeIn) {image.color = new Color(image.color.r, image.color.g, image.color.b, 0f); FadeIn(); }
        else {  image.color = new Color(image.color.r, image.color.g, image.color.b, 1f); FadeOut(); }
    }
    void FadeIn()
    {StartCoroutine(FadeTo(1.0f, fadeDuration));}

    void FadeOut()
    {StartCoroutine(FadeTo(0.0f, fadeDuration));}

    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        Color currentColor = image.color;
        float alphaChangeRate = Mathf.Abs(currentColor.a - targetAlpha) / duration;

        while (!Mathf.Approximately(image.color.a, targetAlpha))
        {
            currentColor = image.color;
            float newAlpha = Mathf.MoveTowards(currentColor.a, targetAlpha, alphaChangeRate * Time.deltaTime);
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }

        if (fadeIn)
        {
            //CUANDO HAGA FADE IN COMPRUEBA SI HA DE CAMBIAR ESCENAS
            if (loadScene && sceneName != null) { SceneManager.LoadScene(sceneName); }
        }
        else
        {   
            //CUANDO HAGA FADE OUT COMPRUEBA SI HA DE ACTIVAR OBJETOS
            if (activateObjects) { foreach (GameObject obj in gameObjectsToActivate) { obj.SetActive(true); } }
            gameObject.SetActive(false);
        }
        if (alternateFade)
        {
            //ALTERNA ENTRE FADE IN Y FADE OUT
            fadeIn = !fadeIn;
        }
    }
    public void SetSceneName(string sceneName) { this.sceneName = sceneName; }
}
