using UnityEngine;
using TMPro;
using System.Collections;
using System.Runtime.CompilerServices;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] private GameObject scorePopUp;
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private SpriteRenderer enemyRenderer;
    private SpawnerController spawnerController;
    private TurretController turretController;
    private Color originalColor;
    private PlayerController playerController;
    private Color damageColor = new Color(1f, 0.5f, 0.5f, 1f); // Light Red

    // Reference to the score counter
    private TextMeshProUGUI scoreCounter;

    private const float flashDuration = 0.2f;

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        turretController = GetComponent<TurretController>();
        spawnerController = FindAnyObjectByType<SpawnerController>();
        currentHealth = maxHealth;
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
        if (gameObject.CompareTag("Turret"))
        {
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Explode");
                Invoke("DestroyGameObject", 0.1f);
            }
        }
        else
        {
            DestroyGameObject();
        }
    }

    private void DestroyGameObject()
    {
        int scoreValue = 0;
        if (spawnerController != null)
        {
            spawnerController.EnemyDestroyed();
        }
        if (gameObject.CompareTag("Turret"))
        {
            scoreValue = 100;
        }
        if (gameObject.CompareTag("CopSpawn1"))
        {
            scoreValue = 100;
        }
        if (gameObject.CompareTag("Spaceship"))
        {
            scoreValue = 500;
        }
        GameObject popUp = Instantiate(scorePopUp, transform.position + (Vector3.up * 0.5f), Quaternion.identity);
        popUp.GetComponent<ScorePopUp>().SetText("+ " + scoreValue.ToString());

        if (scoreCounter != null)
        {
            int currentScore = int.Parse(scoreCounter.text);
            currentScore += scoreValue;
            scoreCounter.text = currentScore.ToString();
        }

        Destroy(gameObject);
    }

    private IEnumerator FlashColor(Color flashColor)
    {
        enemyRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        enemyRenderer.color = originalColor;
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
