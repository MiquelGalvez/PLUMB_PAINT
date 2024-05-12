using UnityEngine;
using TMPro;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private GameObject scorePopUp;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource deadAudioSource;
    [SerializeField] private GameObject pasarescena;
    [SerializeField] private GameObject opendoor;
    private Animator dooranimator;

    public int currentHealth;

    public static event UnityAction OnDeathAnimationStart;
    private SpriteRenderer enemyRenderer;
    private SpawnerController spawnerController;
    private TurretController turretController;
    private Color originalColor;
    private PlayerController playerController;
    private Color damageColor = new Color(1f, 0.5f, 0.5f, 1f); // Light Red
    private int deathSoundCounter = 0;

    // Reference to the score counter
    private TextMeshProUGUI scoreCounter;

    private const float flashDuration = 0.2f;
    private const float fadeDuration = 3f; // Duración de la atenuación gradual

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        turretController = GetComponent<TurretController>();
        spawnerController = FindAnyObjectByType<SpawnerController>();
        currentHealth = maxHealth;
        dooranimator = opendoor.GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        originalColor = enemyRenderer.color;
        // Find the score counter by tag
        scoreCounter = FindTextMeshProUGUIByTag("ScoreCounter");
        if (scoreCounter == null)
        {
            Debug.LogWarning("No score counter found with tag 'ScoreCounter'.");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashColor(damageColor));
        }
    }

    private void Die()
    {
        int scoreValue = 0;

        if (animator != null)
        {
            animator.SetBool("Dead", true);
            animator.SetBool("Idlemov", false);
            animator.SetBool("Active1", false);
            animator.SetBool("Active2", false);
            animator.SetBool("Active3", false);
            scoreValue = 1000;

            if (deathSoundCounter < 2 && deadAudioSource != null)
            {
                deadAudioSource.Play();
                deathSoundCounter++; // Incrementar el contador
            }

            // Inicia la rutina para atenuar el volumen
            StartCoroutine(FadeOutAudio());
        }

        Invoke("EscenaFinal", 7f);
    }

    private IEnumerator FlashColor(Color flashColor)
    {
        enemyRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        enemyRenderer.color = originalColor;
    }

    private IEnumerator FadeOutAudio()
    {
        float startVolume = deadAudioSource.volume; // Volumen inicial
        float currentTime = 0f;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            // Calcula la fracción del tiempo transcurrido
            float fraction = currentTime / fadeDuration;
            // Calcula el volumen actual basado en la fracción
            deadAudioSource.volume = Mathf.Lerp(startVolume, 0f, fraction);
            yield return null; // Espera hasta el siguiente frame
        }

        // Asegúrate de que el volumen esté en silencio al final
        deadAudioSource.volume = 0f;
    }

    private void EscenaFinal()
    {
        dooranimator.SetTrigger("CanOpen");
        pasarescena.SetActive(true);
        this.enabled = false;
    }

    // Helper method to find any object by type
    private T FindAnyObjectByType<T>() where T : Component
    {
        return FindObjectOfType<T>();
    }

    // Helper method to find TextMeshProUGUI by tag
    private TextMeshProUGUI FindTextMeshProUGUIByTag(string tag)
    {
        TextMeshProUGUI[] textMeshes = FindObjectsOfType<TextMeshProUGUI>();
        foreach (TextMeshProUGUI textMesh in textMeshes)
        {
            if (textMesh.CompareTag(tag))
            {
                return textMesh;
            }
        }
        return null; // If no object with the tag is found
    }
}
