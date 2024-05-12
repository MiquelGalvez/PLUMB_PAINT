using UnityEngine;
using TMPro;
using System.Collections;
using System.Runtime.CompilerServices;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] private GameObject scorePopUp; // Prefab for displaying score popup
    [SerializeField] private int maxHealth = 100; // Maximum health of the enemy
    private int currentHealth; // Current health of the enemy

    private SpriteRenderer enemyRenderer; // Reference to the enemy's SpriteRenderer component
    private SpawnerController spawnerController; // Reference to the spawner controller
    private TurretController turretController; // Reference to the turret controller
    private Color originalColor; // Original color of the enemy
    private PlayerController playerController; // Reference to the player controller
    private Color damageColor = new Color(1f, 0.5f, 0.5f, 1f); // Light Red

    // Reference to the score counter
    private TextMeshProUGUI scoreCounter;

    private const float flashDuration = 0.2f; // Duration of color flash when taking damage

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>(); // Find the player controller
        turretController = GetComponent<TurretController>(); // Get the turret controller component if present
        spawnerController = FindAnyObjectByType<SpawnerController>(); // Find the spawner controller
        currentHealth = maxHealth; // Set current health to maximum health
        enemyRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component of the enemy
        originalColor = enemyRenderer.color; // Store the original color of the enemy

        // Find the score counter by tag
        scoreCounter = FindTextMeshProUGUIByTag("ScoreCounter");
        if (scoreCounter == null)
        {
            Debug.LogWarning("No score counter found with tag 'ScoreCounter'.");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce the current health by the damage amount
        if (currentHealth <= 0) // If current health drops to or below zero
        {
            Die(); // Enemy dies
        }
        else
        {
            StartCoroutine(FlashColor(damageColor)); // Flash enemy with damage color
        }
    }

    private void Die()
    {
        if (gameObject.CompareTag("Turret")) // If the enemy is a turret
        {
            Animator animator = GetComponent<Animator>(); // Get the Animator component
            if (animator != null)
            {
                animator.SetTrigger("Explode"); // Trigger explosion animation
                Invoke("DestroyGameObject", 0.1f); // Destroy the GameObject after a short delay
            }
        }
        else // If the enemy is not a turret
        {
            DestroyGameObject(); // Destroy the GameObject immediately
        }
    }

    private void DestroyGameObject()
    {
        int scoreValue = 0; // Initialize score value
        if (spawnerController != null) // If spawner controller is found
        {
            spawnerController.EnemyDestroyed(); // Notify spawner controller that enemy is destroyed
        }
        // Determine score value based on enemy tag
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
        // Instantiate score popup to display earned score
        GameObject popUp = Instantiate(scorePopUp, transform.position + (Vector3.up * 0.5f), Quaternion.identity);
        popUp.GetComponent<ScorePopUp>().SetText("+ " + scoreValue.ToString());

        // Update score counter
        if (scoreCounter != null)
        {
            int currentScore = int.Parse(scoreCounter.text);
            currentScore += scoreValue;
            scoreCounter.text = currentScore.ToString();
        }
        Destroy(gameObject); // Destroy the enemy GameObject
    }

    private IEnumerator FlashColor(Color flashColor)
    {
        enemyRenderer.color = flashColor; // Set enemy color to flash color
        yield return new WaitForSeconds(flashDuration); // Wait for flash duration
        enemyRenderer.color = originalColor; // Restore original enemy color
    }

    // Helper method to find any object by type
    private T FindAnyObjectByType<T>() where T : Component
    {
        return FindObjectOfType<T>(); // Find and return object of specified type
    }

    // Helper method to find TextMeshProUGUI by tag
    private TextMeshProUGUI FindTextMeshProUGUIByTag(string tag)
    {
        TextMeshProUGUI[] textMeshes = FindObjectsOfType<TextMeshProUGUI>(); // Find all TextMeshProUGUI objects
        foreach (TextMeshProUGUI textMesh in textMeshes)
        {
            if (textMesh.CompareTag(tag)) // If object has specified tag
            {
                return textMesh; // Return the object
            }
        }
        return null; // If no object with the tag is found
    }
}
